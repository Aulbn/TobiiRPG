using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : Target
{
    [Header("Stats")]
    public float maxHealth;
    public float currentHealth;
    public float HealthPercentage { get { return maxHealth <= 0 ? 1 : currentHealth / maxHealth; } }

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void Damage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
    }
}
