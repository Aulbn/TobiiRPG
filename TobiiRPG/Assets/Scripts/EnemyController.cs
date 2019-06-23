using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Killable
{
    private enum EnemyStates
    {
        Idle, Combat, Chasing, Death
    }
    private EnemyStates state = EnemyStates.Idle;

    [Header("Movement")]
    public float rotationSpeed = 2f;
    private NavMeshAgent agent;
    [Header("Spellcasting")]
    public float spellCooldown;
    public float castDistance;

    private float spellCooldownTimer = 0;
    private Animator animator;

    public static int LivingEnemies = 0;

    private void Awake()
    {
        LivingEnemies++;
    }
    private new void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (PlayerController.CurrentHealth == 0 && state != EnemyStates.Idle)
            EnterState_Idle();
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
            case EnemyStates.Death:
                break;
            default:
                break;
        }

        animator.SetInteger("State", (int)state);
    }

    private void EnterState_Idle()
    {
        state = EnemyStates.Idle;
        spellCooldownTimer = 0;
        animator.SetInteger("State", 0);
        //Debug.Log("Enter idle");
    }
    private void State_Idle()
    {

    }
    private void EnterState_Combat()
    {
        state = EnemyStates.Combat;
        agent.isStopped = true;
        //Debug.Log("Enter combat");
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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(PlayerController.Instance.transform.position - transform.position), Time.deltaTime * rotationSpeed);
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

    public override void Damage(float damage)
    {
        if (state == EnemyStates.Idle)
            EnterState_Combat();
        base.Damage(damage);
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            LivingEnemies--;
            //Destroy(gameObject);
            gameObject.tag = "Untagged";
            state = EnemyStates.Death;
            animator.SetTrigger("Die");
        }
    }

    private void ThrowSpell()
    {
        //Vector3 spawnPos = transform.position + (PlayerController.Instance.transform.position - transform.position).normalized * spellSpawnDist;
        //GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);
        //spell.GetComponent<SpellProjectile>().SetInfo(PlayerController.Instance.transform);
        animator.SetTrigger("ThrowSpell");
    }
}
