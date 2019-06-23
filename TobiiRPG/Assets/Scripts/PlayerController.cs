using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class PlayerController : Killable
{
    public static PlayerController Instance;

    public enum PlayerState
    {
        MoveState, IdleState, SpellPick
    }
    public PlayerState playerState = PlayerState.IdleState;

    //public float preSelectionTime;
    //public float selectionTime;
    //public float maxHealth;
    //[HideInInspector]
    //public float currentHealth;
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
    [HideInInspector]
    public NavMeshAgent agent;

    private Animator animator;

    private void Awake()
    {
        Instance = this;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    public new void Start()
    {
        base.Start();
        GazeManager.OnEyeGestureAboveScreen += () => ToggleMoveState();

        walkToPos = transform.position;
        agent.angularSpeed = rotationSpeed;
        agent.speed = walkSpeed;
        agent.acceleration = walkAcceleration;
        //currentHealth = maxHealth;
    }

    void Update()
    {
        AnimationUpdate();

        switch (playerState)
        {
            case PlayerState.IdleState:
                //Look for target
                Collider col = GazeManager.RaycastCollider();
                Graphic gr = GazeManager.RaycastUI();

                if (gr != null)
                {
                    if (gr.CompareTag("Target"))
                    {
                        UIManager.SetTarget(gr.transform);
                    }
                } else if (col != null && col.CompareTag("Enemy"))
                {
                    UIManager.SetTarget(col.transform);
                } 

                TargetMarkerSelectUpdate();
                break;
            case PlayerState.MoveState:
                gr = GazeManager.RaycastUI();
                if (gr != null && gr.CompareTag("Target"))
                {
                    UIManager.SetTarget(gr.transform);
                    return;
                }
                TargetMarkerSelectUpdate();
                WalkSelection();
                break;
            case PlayerState.SpellPick:
                ////Look for target
                //col = GazeManager.RaycastCollider();
                //gr = GazeManager.RaycastUI();

                //if (gr != null)
                //{
                //    if (gr.CompareTag("Target"))
                //    {
                //        UIManager.SetTarget(gr.transform);
                //        UIManager.Instance.targetMarker.color = playerState == PlayerState.MoveState ? UIManager.Instance.combatColor : UIManager.Instance.walkColor;
                //        UIManager.ShowSpellWheel(false);
                //        //UIManager.Instance.markerTimer = UIManager.Instance.markerLifeTime;
                //    }
                //}
                //else if (col != null && col.CompareTag("Enemy"))
                //{
                //    UIManager.SetTarget(col.transform);
                //    UIManager.ShowSpellWheel(false);
                //    //UIManager.Instance.markerTimer = UIManager.Instance.markerLifeTime;
                //}
                //TargetMarkerSelectUpdate();
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

    private void TargetMarkerSelectUpdate()
    {
        Graphic graphic = GazeManager.RaycastUI();
        if (graphic == null)
            return;

        if (graphic.Equals(UIManager.Instance.targetMarker))
        {
            if (UIManager.Target != null)
                {
                bool isKillable = UIManager.TargetType().IsSubclassOf(typeof(Killable));
                if (!isKillable)
                {
                    ToggleMoveState();
                } else
                {
                    playerState = PlayerState.SpellPick;
                    //UIManager.ShowStateWheel(isPlayer);
                    UIManager.ShowSpellWheel(isKillable);
                }
            }
        }
    }

    private void WalkSelection()
    {
        Collider col = GazeManager.RaycastCollider();
        Vector3 newPos = GazeManager.RaycastPosition();

        if (GazeManager.RaycastUI() != null)
            return;

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
                //walkSelTimer = 0;
                //newPos = Vector3.zero;
            }

            if (Vector3.Distance(newPos, walkToPos) >= corrDistance)
            {
                walkSelTimer = 0;
                walkToPos = newPos;
                UIManager.SetSelectionMarker(0, walkToPos);
            }
        }
    }

    public static void Respawn(Vector3 respawnPoint)
    {
        Instance.transform.position = respawnPoint;
        Instance.currentHealth = Instance.maxHealth;
        UIManager.Instance.target = null;
        Instance.agent.SetDestination(respawnPoint);
    }

    private void ToggleMoveState()
    {
        if (!GameManager.isRunning)
            return;
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
        UIManager.ShowTargetMarker(false);
        //UIManager.Instance.targetMarker.color = playerState == PlayerState.MoveState ? UIManager.Instance.combatColor : UIManager.Instance.walkColor;
        //Debug.Log("TOGGLE STATE");
    }

    public static void SetState(PlayerState state)
    {
        Instance.playerState = state;
        switch (state)
        {
            case PlayerState.IdleState:
                UIManager.SetSelectionMarker(0, Instance.walkToPos);
                break;
            case PlayerState.MoveState:
                UIManager.ShowSpellWheel(false);
                break;
            case PlayerState.SpellPick:
                UIManager.SetSelectionMarker(0, Instance.walkToPos);
                break;
            default:
                break;
        }
    }

    public static void ThrowSpell(int spellIndex)
    {
        //Vector3 spawnPos = Instance.transform.position + Instance.transform.forward * Instance.spellSpawnDist;
        Vector3 spawnPos = Instance.transform.position + (UIManager.Target.position - Instance.transform.position).normalized * Instance.spellSpawnDist;
        GameObject spell = Instantiate(spellIndex == 1 ? Instance.spell1 : Instance.spell2, spawnPos, Quaternion.identity);
        spell.GetComponent<SpellProjectile>().SetInfo(UIManager.Target);
        Instance.animator.SetTrigger("ThrowSpell");
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        if (currentHealth <= 0)
            UIManager.ShowDeathScreen();
        //Debug.Log("Damage: " + damage);
    }

}
