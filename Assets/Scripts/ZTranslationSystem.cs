using Assets.Scripts.Classes;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ZTranslationSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(CubeComponent))]
    struct TranslateJob : IJobForEach<Translation, MoveSpeedComponentData>
    {
        [ReadOnly]
        public float DeltaTime;

        [ReadOnly]
        public bool MinusDirection;

        public void Execute(ref Translation translation, ref MoveSpeedComponentData moveSpeed)
        {
            translation.Value += new float3(0f, 0f, (MinusDirection ? -1 : 1) * moveSpeed.Value * DeltaTime);
            Helpers.AddDummyHeavyTask();
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TranslateJob
        {
            MinusDirection = true,
            DeltaTime = Time.DeltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
