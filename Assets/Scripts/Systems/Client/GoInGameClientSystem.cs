using Unity.Entities;
using Unity.NetCode;

// When client has a connection with network id, go in game and tell server to also go in game
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class GoInGameClientSystem : SystemBase {
    BeginInitializationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate() {
        ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate() {
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();

        Entities
            .WithNone<NetworkStreamInGame>()
            .ForEach((Entity ent, int entityInQueryIndex, ref NetworkIdComponent id) => {
                ecb.AddComponent<NetworkStreamInGame>(entityInQueryIndex, ent);
                var req = ecb.CreateEntity(entityInQueryIndex);
                ecb.AddComponent<GoInGameRequest>(entityInQueryIndex, req);
                ecb.AddComponent(entityInQueryIndex, req, new SendRpcCommandRequestComponent { TargetConnection = ent });
            })
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}