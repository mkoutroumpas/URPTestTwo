using Assets.Scripts.Classes;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct ZTranslationJob : IJobParallelFor
{
    public NativeArray<float> MoveSpeeds;
    public NativeArray<float3> Positions;

    [ReadOnly]
    public float DeltaTime;

    [ReadOnly]
    public bool MinusDirection;

    public void Execute(int index)
    {
        Positions[index] += new float3(0f, 0f, (MinusDirection ? -1 : 1) * MoveSpeeds[index] * DeltaTime);
        Helpers.AddDummyHeavyTask();
    }
}
