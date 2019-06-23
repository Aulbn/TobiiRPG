using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMarker : MonoBehaviour
{
    public float hideDistance;
    public Vector3 offset;

    private MeshRenderer mRenderer;
    private ParticleSystem particleSys;

    private void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        particleSys = GetComponent<ParticleSystem>();
        particleSys.Stop();
        mRenderer.enabled = false;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= hideDistance)
        {
            mRenderer.enabled = false;
            particleSys.Stop();
        }
        else if (!mRenderer.enabled)
        {
            mRenderer.enabled = true;
            particleSys.Play();
        }

        if (PlayerController.Instance.agent.destination != null)
            transform.position = PlayerController.Instance.agent.destination + offset;
    }
}
