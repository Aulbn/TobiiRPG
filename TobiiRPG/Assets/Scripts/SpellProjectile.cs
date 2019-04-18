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

    private void Start()
    {
        if (lifeTime <= 0)
            StartCoroutine(LifeClock());
    }

    void Update()
    {
        
        transform.position += (following ? (target.position - transform.position).normalized : targetDirection) * movementSpeed * Time.deltaTime;
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
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (enemy != null)
            enemy.Damage(damage);
        if (player != null)
            player.Damage(damage);
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.Euler(collision.GetContact(0).normal));
        Destroy(gameObject);
    }

}
