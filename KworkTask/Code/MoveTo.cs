using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public Transform objectMovement;
    public Vector3 shift;
    private Vector3 position;

    private void Update() => Move();
    private void Move() 
    {
        position = objectMovement.position + shift;
        position.y = 0;
        position.z = -10;

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
    }
}
