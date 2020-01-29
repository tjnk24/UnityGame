using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Serializable]
    public class DamageEvent : UnityEvent<Damager, Damageable>
    { }

    [Serializable]
    public class HealthEvent : UnityEvent<Damageable>
    { }

    public int startingHealth = 5;

    [HideInInspector]
    public bool isDead;

    public HealthEvent OnHealthSet;
    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;

    public int currentHealth;

    private void OnEnable()
    {
        currentHealth = startingHealth;

        OnHealthSet.Invoke(this);
    }

    public void TakeDamage(Damager damager)
    {
        currentHealth -= damager.damage;
        OnHealthSet.Invoke(this);

        OnTakeDamage.Invoke(damager, this);

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDie.Invoke(damager, this);
        }
    }
}
