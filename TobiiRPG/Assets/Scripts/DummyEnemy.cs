using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : Killable
{
    private bool isDead = false;
    private Animator animator;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
            isDead = true;

        animator.SetBool("isDead", isDead);
    }

    public override void Damage(float damage)
    {
        //Debug.Log(gameObject + " Got Hit!");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            TargetPractice.Triggered();
        }
    }

    public void RiseTargetDummyRise()
    {
        currentHealth = 1;
        isDead = false;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

}
