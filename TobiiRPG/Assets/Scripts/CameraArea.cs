using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class CameraArea : MonoBehaviour
{
    public CameraController.CameraState cameraState;

    [SerializeField]
    private Vector3 centerOffset;
    [SerializeField]
    private Vector3 cameraOffset;
    [SerializeField]
    private float followSpeed;
    [SerializeField]
    private float rotationSpeed;
    public Vector3 CameraPosition { get { return transform.position + centerOffset + cameraOffset; } }
    public Vector3 CenterPosition { get { return transform.position + centerOffset; } }
    public Vector3 CameraOffset { get { return cameraOffset; } }
    public float RotationSpeed { get { return rotationSpeed; } }
    public float FollowSpeed { get { return followSpeed; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            CameraController.ChangeState(this);
            //Debug.Log("Change camera state to : " + cameraState);
        }
    }
}
