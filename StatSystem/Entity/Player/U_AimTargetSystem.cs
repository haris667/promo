using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class U_AimTargetSystem : MonoBehaviour
{
    public List<string> collisionTags;
    public Action<Transform> onHitTarget;
    private Camera _camera;
    private Vector3 RayCenterPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    private Ray ray;
    private void Awake() => _camera = Camera.main;

    private void FixedUpdate() => ReleaseCenterRay();
    private void ReleaseCenterRay()
    {
        ray = _camera.ScreenPointToRay(RayCenterPosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform == null)
            return;
        else if(collisionTags.Contains(hit.transform.tag))
            onHitTarget?.Invoke(hit.transform);
    }
}