using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public float damage;
    public float movementSpeed;
    public bool following;
    public float lifeTime;
    public GameObject explosionPrefab;

    private Transform target;
    private Vector3 targetDirection;
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        if (lifeTime <= 0)
            StartCoroutine(LifeClock());
    }

    void Update()
    {
        transform.position += (following && target != null ? (target.position - transform.position).normalized : targetDirection) * movementSpeed * Time.deltaTime;
        if (!particles.IsAlive())
            Destroy(gameObject);
    }

    private IEnumerator LifeClock()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void SetInfo(Transform target)
    {
        this.target = target;
        targetDirection = (target.position - transform.position).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
       //Debug.Log(collision.gameObject);
        Killable killable = collision.collider.GetComponent<Killable>();
        if (killable != null)
            killable.Damage(damage);
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.Euler(collision.GetContact(0).normal));

        particles.Stop();
        GetComponent<Light>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

}
