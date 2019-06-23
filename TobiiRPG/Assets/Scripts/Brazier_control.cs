using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brazier_control : MonoBehaviour
{
    public ParticleSystem firePs;
    public bool isLit = false;

    // Start is called before the first frame update
    void Start()
    {
        firePs.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (firePs.isStopped)
        {
            if (isLit)
                firePs.Play();
        }
    }

    public void SetIsLit(bool lit)
    {
        isLit = lit;
    }
    public bool GetIsLit()
    {
        return isLit;
    }
}
