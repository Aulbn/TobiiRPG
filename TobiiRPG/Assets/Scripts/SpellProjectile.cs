using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public float damage;
    public float movementSpeed;
    public bool following;
    public float lifeTime;

    private Transform target;
    private Vector3 targetPosition;

    private void Start()
    {
        if (lifeTime <= 0)
            StartCoroutine(LifeClock());
    }

    void Update()
    {
        //transform.position += (following ? transform.position - target.position : transform.position - targetPosition) * Time.deltaTime * movementSpeed;
        //transform.position = Vector3.MoveTowards(transform.position, (following ? transform.position - target.position : transform.position - targetPosition), Time.deltaTime * movementSpeed);
        transform.position = Vector3.MoveTowards(transform.position, (following ? target.position : targetPosition), Time.deltaTime * movementSpeed);
        //transform.position += (transform.position - target.position).normalized * 3f;
    }

    private IEnumerator LifeClock()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void SetInfo(Transform target)
    {
        this.target = target;
        targetPosition = target.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (enemy != null)
            enemy.Damage(damage);
        if (player != null)
            player.Damage(damage);
        //Spawna explosion
        Debug.Log("Destroy spell");
        Destroy(gameObject);
    }

}
