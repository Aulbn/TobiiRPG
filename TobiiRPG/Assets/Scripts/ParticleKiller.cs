using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour
{
    private ParticleSystem particles;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
