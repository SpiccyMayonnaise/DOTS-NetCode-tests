using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

public class GameSystem : SystemBase {
    const int GamePort = 1234;

    struct InitGameComponent : IComponentData { }
    
    protected override void OnCreate() {
        RequireSingletonForUpdate<InitGameComponent>();

        EntityManager.CreateEntity(typeof(InitGameComponent));
    }

    protected override void OnUpdate() {
        EntityManager.DestroyEntity(GetSingletonEntity<InitGameComponent>());
        
        foreach (var world in World.All) {
            var network = world.GetExistingSystem<NetworkStreamReceiveSystem>();
            
            if (world.GetExistingSystem<ClientSimulationSystemGroup>() != null) {
                NetworkEndPoint ep = NetworkEndPoint.LoopbackIpv4;
                ep.Port = GamePort;
                network.Connect(ep);
            }
#if UNITY_EDITOR
            else if (world.GetExistingSystem<ServerSimulationSystemGroup>() != null) {
                NetworkEndPoint ep = NetworkEndPoint.AnyIpv4;
                ep.Port = GamePort;
                network.Listen(ep);
            }
#endif
        }
    }
}

public struct GoInGameRequest : IRpcCommand { }