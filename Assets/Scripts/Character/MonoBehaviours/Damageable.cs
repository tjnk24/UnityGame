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

    public int startingHealth = 5;

    [HideInInspector]
    public bool isDead;

    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;

    public int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(Damager damager)
    {
        currentHealth -= damager.damage;

        OnTakeDamage.Invoke(damager, this);

        if (CurrentHealth <= 0)
        {
            isDead = true;
            OnDie.Invoke(damager, this);
        }
    }
}
