using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float followSpeed = 1;
    public float rotationSpeed = 1;

    private Vector3 followPosition;
    private Vector3 lookAtPosition;
    
    void Update()
    {
        followPosition = PlayerController.Instance.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, followPosition, Time.deltaTime * followSpeed);
        lookAtPosition = Vector3.MoveTowards(lookAtPosition, PlayerController.Instance.transform.position, Time.deltaTime * rotationSpeed);
        transform.LookAt(lookAtPosition);
    }
}
