using Unity.Entities;
using Unity.NetCode;

// When server receives go in game request, go in game and delete request
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class GoInGameServerSystem : SystemBase {
    BeginInitializationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate() {
        ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate() {
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();

        var networkIdLookup = GetComponentDataFromEntity<NetworkIdComponent>();
        var movableCubeLookup = GetComponentDataFromEntity<MovableCube>();

        var ghostPrefabBufferLookup = GetBufferFromEntity<GhostPrefabBuffer>();
        
        var ghostPrefabCollectionSingleton = GetSingletonEntity<GhostPrefabCollectionComponent>();

        Entities
            .WithReadOnly(networkIdLookup)
            .WithReadOnly(movableCubeLookup)

            .WithReadOnly(ghostPrefabBufferLookup)
            
            .WithNone<SendRpcCommandRequestComponent>()
            .ForEach((Entity reqEnt, int entityInQueryIndex, ref GoInGameRequest req, ref ReceiveRpcCommandRequestComponent reqSrc) => {

                ecb.AddComponent<NetworkStreamInGame>(entityInQueryIndex, reqSrc.SourceConnection);
                UnityEngine.Debug.Log(string.Format("Server setting connection {0} to in game", networkIdLookup[reqSrc.SourceConnection].Value));
                var prefab = Entity.Null;
                var prefabs = ghostPrefabBufferLookup[ghostPrefabCollectionSingleton];
                for (int ghostId = 0; ghostId < prefabs.Length; ++ghostId) {
                    if (movableCubeLookup.HasComponent(prefabs[ghostId].Value))
                        prefab = prefabs[ghostId].Value;
                }
                var player = ecb.Instantiate(entityInQueryIndex, prefab);
                ecb.SetComponent(entityInQueryIndex, player, new GhostOwnerComponent { NetworkId = networkIdLookup[reqSrc.SourceConnection].Value });
                ecb.AddBuffer<CubeInput>(entityInQueryIndex, player);

                ecb.SetComponent(entityInQueryIndex, reqSrc.SourceConnection, new CommandTargetComponent { targetEntity = player });

                ecb.DestroyEntity(entityInQueryIndex, reqEnt);
            })
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}
