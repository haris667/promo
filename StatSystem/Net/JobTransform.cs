using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct JobTransform : IJobParallelForTransform
{
    /// <summary>
    /// Целевые позиции
    /// </summary>
    [ReadOnly]
    public NativeArray<Vector3> NewPositions;

    [ReadOnly]
    public NativeArray<Quaternion> NewRotations;

    public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        float c = 0.05f;

        transform.position = Vector3.Lerp(transform.position, NewPositions[index], DeltaTime + c);

        transform.rotation = Quaternion.Lerp(transform.rotation, NewRotations[index], DeltaTime + c);
    }

    
}
