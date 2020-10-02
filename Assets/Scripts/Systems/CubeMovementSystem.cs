using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class CubeMovementSystem : SystemBase {
    protected override void OnUpdate() {
        var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
        var tick = group.PredictingTick;
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((DynamicBuffer<CubeInput> inputBuffer, ref Translation trans, ref PredictedGhostComponent prediction) => {
            if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                return;

            CubeInput input;
            inputBuffer.GetDataAtTick(tick, out input);
            if (input.horizontal > 0)
                trans.Value.x += deltaTime;
            if (input.horizontal < 0)
                trans.Value.x -= deltaTime;
            if (input.vertical > 0)
                trans.Value.z += deltaTime;
            if (input.vertical < 0)
                trans.Value.z -= deltaTime;

            if (input.up)
                trans.Value.y += deltaTime;
        })
            .ScheduleParallel();
    }
}
