using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.NetCode;

public class SampleCubeInputSystem : SystemBase {
    protected override void OnCreate() {
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate() {
        var localInput = GetSingleton<CommandTargetComponent>().targetEntity;
        if (localInput == Entity.Null) {
            var localPlayerId = GetSingleton<NetworkIdComponent>().Value;

            Entities
                .WithAll<MovableCube>()
                .WithNone<CubeInput>()
                .ForEach((Entity entity, ref GhostOwnerComponent ghostOwner) => { 
                    if (ghostOwner.NetworkId == localPlayerId) {
                        
                    }
                });
        }
    }
}
