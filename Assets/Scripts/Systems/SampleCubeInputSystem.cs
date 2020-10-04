using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class SampleCubeInputSystem : SystemBase {
    BeginInitializationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate() {
        ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate() {
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();

        var localInput = GetSingleton<CommandTargetComponent>().targetEntity;
        if (localInput == Entity.Null) {
            var localPlayerId = GetSingleton<NetworkIdComponent>().Value;
            var commandTarget = GetSingletonEntity<CommandTargetComponent>();

            Entities
                .WithAll<MovableCube>()
                .WithNone<CubeInput>()
                .ForEach((Entity ent, int entityInQueryIndex, ref GhostOwnerComponent ghostOwner) => {
                    if (ghostOwner.NetworkId == localPlayerId) {
                        ecb.AddBuffer<CubeInput>(entityInQueryIndex, ent);
                        ecb.SetComponent(entityInQueryIndex, commandTarget, new CommandTargetComponent { targetEntity = ent });
                    }
                })
                .ScheduleParallel();
            return;
        }

        var input = default(CubeInput);
        input.Tick = World.GetExistingSystem<ClientSimulationSystemGroup>().ServerTick;
        if (Input.GetKey("a"))
            input.horizontal -= 1;
        if (Input.GetKey("d"))
            input.horizontal += 1;
        if (Input.GetKey("s"))
            input.vertical -= 1;
        if (Input.GetKey("w"))
            input.vertical += 1;

        input.up = Input.GetKey(KeyCode.Space);

        var inputBuffer = EntityManager.GetBuffer<CubeInput>(localInput);
        inputBuffer.AddCommandData(input);

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            var req = EntityManager.CreateEntity();
            EntityManager.AddComponentData(req, new TestRPCCommand { data = (int)input.Tick });
            EntityManager.AddComponentData(req, new SendRpcCommandRequestComponent());
        }

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}