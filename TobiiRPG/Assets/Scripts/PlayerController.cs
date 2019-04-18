using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public enum PlayerState
    {
        MoveState, IdleState, SpellPick
    }
    public PlayerState playerState = PlayerState.IdleState;

    //public float preSelectionTime;
    //public float selectionTime;
    public float maxHealth;
    private float currentHealth;
    public static float CurrentHealth { get { return Instance.currentHealth; } }
    [Header("Walking")]
    public float walkSpeed = 1;
    public float walkAcceleration = 10;
    public float rotationSpeed = 1;
    public float preWalkSelTime;
    public float walkSelTime;
    private float walkSelTimer = 0;
    public float corrDistance;
    private Vector3 walkToPos;
    [Header("Spell Casting")]
    public GameObject spell1;
    public GameObject spell2;
    public float spellSpawnDist;
    private NavMeshAgent agent;

    private void Awake()
    {
        Instance = this;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GazeManager.OnEyeGestureAboveScreen += () => ToggleMoveState();

        walkToPos = transform.position;
        agent.angularSpeed = rotationSpeed;
        agent.speed = walkSpeed;
        agent.acceleration = walkAcceleration;
        currentHealth = maxHealth;
    }

    void Update()
    {
        switch (playerState)
        {
            case PlayerState.IdleState:
                //Look for enemy
                Collider col = GazeManager.RaycastCollider();
                if (col != null && col.CompareTag("Enemy"))
                {
                    //Debug.Log("ENEMY");
                    UIManager.TargetEnemy(col.transform);
                }
                EnemyMarkerSelectUpdate();
                break;
            case PlayerState.MoveState:
                WalkSelection();
                break;
            case PlayerState.SpellPick:
                break;
            default:
                break;
        }
    }

    private void AnimationUpdate()
    {
        animator.SetInteger("State", (int)Instance.playerState);

        if (agent.hasPath)
        {
            Vector3 corner = agent.path.corners[1];
            corner = new Vector3(corner.x, transform.position.y, corner.z);
            Vector3 delta = (corner - transform.position).normalized;
            float z = Vector3.Dot(transform.forward, delta);
            float x = Vector3.Dot(transform.right, delta);
            animator.SetFloat("Velocity_Z", z);
            animator.SetFloat("Velocity_X", x);
        }

        if (Vector3.Distance(agent.destination, transform.position) <= 2f)
        {
            animator.SetFloat("Velocity_Z", Mathf.Lerp(animator.GetFloat("Velocity_Z"), 0, Time.deltaTime * 8));
            animator.SetFloat("Velocity_X", Mathf.Lerp(animator.GetFloat("Velocity_X"), 0, Time.deltaTime * 8));
        }
    }

    private void EnemyMarkerSelectUpdate()
    {
        Graphic graphic = GazeManager.RaycastUI();
        if (graphic == null)
            return;

        if (graphic.Equals(UIManager.Instance.enemyMarker))
        {
            playerState = PlayerState.SpellPick;
            UIManager.ShowSpellWheel(true);
        }
    }

    private void WalkSelection()
    {
        Collider col = GazeManager.RaycastCollider();
        if (col != null && col.gameObject.layer == 9)
        {
            walkSelTimer += Time.deltaTime;
            if (walkSelTimer >= preWalkSelTime)
            {
                UIManager.SetSelectionMarker(Mathf.Clamp01((walkSelTimer - preWalkSelTime) / (walkSelTime - preWalkSelTime)), walkToPos);
            }
            if (walkSelTimer >= walkSelTime)
            {
                agent.SetDestination(new Vector3(walkToPos.x, transform.position.y, walkToPos.z));
                UIManager.SetSelectionMarker(0, walkToPos);
            }

            Vector3 newPos = GazeManager.RaycastPosition();
            if (Vector3.Distance(newPos, walkToPos) >= corrDistance)
            {
                walkSelTimer = 0;
                walkToPos = newPos;
                UIManager.SetSelectionMarker(0, walkToPos);
            }
        }
    }

    private void ToggleMoveState()
    {
        switch (playerState)
        {
            case PlayerState.IdleState:
                playerState = PlayerState.MoveState;
                break;
            case PlayerState.MoveState:
                playerState = PlayerState.IdleState;
                UIManager.SetSelectionMarker(0, walkToPos);
                break;
            case PlayerState.SpellPick:
                playerState = PlayerState.MoveState;
                UIManager.ShowSpellWheel(false);
                break;
            default:
                break;
        }
        UIManager.SetStateSprite();
        //Debug.Log("TOGGLE STATE");
    }

    public static void ThrowSpell(int spellIndex)
    {
        //Vector3 spawnPos = Instance.transform.position + Instance.transform.forward * Instance.spellSpawnDist;
        Vector3 spawnPos = Instance.transform.position + (UIManager.Target.position - Instance.transform.position).normalized * Instance.spellSpawnDist;
        GameObject spell = Instantiate(spellIndex == 1 ? Instance.spell1 : Instance.spell2, spawnPos, Quaternion.identity);
        spell.GetComponent<SpellProjectile>().SetInfo(UIManager.Target);
    }

    public void Damage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth <= 0)
            Debug.Log("U dead, son!");
    }

}
