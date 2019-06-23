using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public enum CameraState
    {
        Static, Follow, SideScroll
    }
    public CameraState cameraState;

    public CameraArea cameraArea;

    public Vector3 offset;
    public float followSpeed = 1;
    public float rotationSpeed = 1;

    private Vector3 followPosition;
    private Vector3 lookAtPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ChangeState(cameraArea);
    }

    void Update()
    {
        switch (cameraState)
        {
            case CameraState.Static:
                StateUpdate_Static();
                break;
            case CameraState.Follow:
                StateUpdate_Follow();
                break;
            case CameraState.SideScroll:
                StateUpdate_SideScroll();
                break;
            default:
                break;
        }
    }

    private void StateUpdate_Follow()
    {
        followPosition = PlayerController.Instance.transform.position + cameraArea.CameraOffset;
        transform.position = Vector3.Lerp(transform.position, followPosition, Time.deltaTime * followSpeed);
        lookAtPosition = Vector3.MoveTowards(lookAtPosition, PlayerController.Instance.transform.position, Time.deltaTime * rotationSpeed);
        transform.LookAt(lookAtPosition);
    }
    private void StateUpdate_SideScroll()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        followPosition = new Vector3 (followPosition.x, followPosition.y, playerPos.z + cameraArea.CameraOffset.z);
        lookAtPosition = new Vector3 (lookAtPosition.x, lookAtPosition.y, playerPos.z);
        transform.position = Vector3.Lerp(transform.position, followPosition, Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookAtPosition - transform.position), Time.deltaTime * rotationSpeed);
    }
    private void StateUpdate_Static()
    {
        transform.position = Vector3.Lerp(transform.position, followPosition, Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookAtPosition - transform.position), Time.deltaTime * rotationSpeed);
    }

    public static void LookAtRespawn()
    {
        Instance.followPosition = GameManager.Instance.spawnPoint.position + Instance.cameraArea.CameraOffset;
        Instance.transform.position = Instance.followPosition;
        Instance.lookAtPosition = GameManager.Instance.spawnPoint.position;
        Instance.transform.LookAt(Instance.lookAtPosition);
    }

    public static void ChangeState(CameraArea newArea)
    {
        Instance.cameraArea = newArea;
        Instance.cameraState = newArea.cameraState;
        Instance.followSpeed = newArea.FollowSpeed;
        Instance.rotationSpeed = newArea.RotationSpeed;
        
        switch (newArea.cameraState)
        {
            case CameraState.Static:
                Instance.followPosition = newArea.CameraPosition;
                Instance.lookAtPosition = newArea.CenterPosition;
                break;
            case CameraState.Follow:
                Instance.followPosition = PlayerController.Instance.transform.position + newArea.CameraOffset;
                Instance.lookAtPosition = PlayerController.Instance.transform.position;
                break;
            case CameraState.SideScroll:
                    Instance.followPosition = newArea.CameraPosition;
                    Instance.lookAtPosition = newArea.CenterPosition;
                break;
            default:
                break;
        }
    }

}
