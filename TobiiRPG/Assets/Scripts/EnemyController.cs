using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private enum EnemyStates
    {
        Idle, Combat, Chasing
    }
    private EnemyStates state = EnemyStates.Idle;

    [Header("Stats")]
    public float maxHealth;
    public float currentHealth;
    private NavMeshAgent agent;
    [Header("Spellcasting")]
    public GameObject spellPrefab;
    public float spellCooldown;
    public float castDistance;
    public float spellSpawnDist = 0.2f;

    private float spellCooldownTimer = 0;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyStates.Idle:
                State_Idle();
                break;
            case EnemyStates.Combat:
                State_Combat();
                break;
            case EnemyStates.Chasing:
                State_Chasing();
                break;
            default:
                break;
        }

        animator.SetInteger("State", (int)state);

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private void EnterState_Idle()
    {
        state = EnemyStates.Idle;
        Debug.Log("Enter idle");
    }
    private void State_Idle()
    {

    }
    private void EnterState_Combat()
    {
        state = EnemyStates.Combat;
        agent.isStopped = true;
        Debug.Log("Enter combat");
    }
    private void State_Combat()
    {
        if (PlayerDistance() > castDistance)
        {
            EnterState_Chasing();
        }

        if (spellCooldownTimer <= spellCooldown)
            spellCooldownTimer += Time.deltaTime;
        else
        {
            ThrowSpell();
            spellCooldownTimer = 0;
        }
    }
    private void EnterState_Chasing()
    {
        state = EnemyStates.Chasing;
        agent.isStopped = false;
        Debug.Log("Enter chasing");
    }
    private void State_Chasing()
    {
        if (PlayerDistance() <= castDistance)
            EnterState_Combat();
        agent.SetDestination(PlayerController.Instance.transform.position);
    }

    private float PlayerDistance()
    {
       return Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
    }

    public void Damage(float damage)
    {
        if (state == EnemyStates.Idle)
            EnterState_Combat();
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        Debug.Log("Damage!");
    }

    private void ThrowSpell()
    {
        Vector3 spawnPos = transform.position + (PlayerController.Instance.transform.position - transform.position).normalized * spellSpawnDist;
        GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);
        spell.GetComponent<SpellProjectile>().SetInfo(PlayerController.Instance.transform);
    }
}
