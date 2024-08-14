using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public class InterpolateTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;

    [SerializeField]
    private Transform _target;

    private UnityEngine.Jobs.TransformAccessArray TransformAccessArray;
    private NativeArray<Vector3> _pos;
    private NativeArray<Quaternion> _rot;

    // Start is called before the first frame update
    void Awake()
    {
        TransformAccessArray = new TransformAccessArray(new Transform[] { _transform });
        _pos = new NativeArray<Vector3>(1, Allocator.TempJob);
        _rot= new NativeArray<Quaternion>(1, Allocator.TempJob);
    }

    // Update is called once per frame
    void Update()
    {
        _pos[0] = _target.transform.position;
        _rot[0] = _target.transform.rotation;

        JobTransform jobTransform = new JobTransform()
        {
            NewPositions = _pos,
            NewRotations = _rot,
            DeltaTime = Time.deltaTime,
        };

        var handle = jobTransform.Schedule(TransformAccessArray);
        handle.Complete();  
    }

    private void OnDestroy()
    {
        _pos.Dispose();
        _rot.Dispose();
        TransformAccessArray.Dispose();
    }
}
