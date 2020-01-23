using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolingComponent
{
    /*[HideInInspector]
    public PoolingObject projectilePoolObject;*/

    protected Damager projectileDamager;
    protected Animator animator;

    protected readonly int HashBroken = Animator.StringToHash("isBroken");

    private void Awake()
    {
        projectileDamager = GetComponent<Damager>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartAttack();
    }

    private void OnDisable()
    {
        EndAttack();
    }
    public void SetBroken()
    {
        animator.SetTrigger(HashBroken);
    }

    /*public void ReturnToPool()
    {
        projectilePoolObject.ReturnToPool();
    }
*/
    public void StartAttack()
    {

        projectileDamager.EnableDamage();
        projectileDamager.disableDamageAfterHit = true;
    }

    public void EndAttack()
    {
        projectileDamager.DisableDamage();

        
    }
}
