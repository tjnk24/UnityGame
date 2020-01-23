using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    [Serializable]
    public class DamagableEvent : UnityEvent<Damager, Damageable>
    { }


    [Serializable]
    public class NonDamagableEvent : UnityEvent<Damager>
    { }

    public int damage = 1;

    public Vector2 offset = new Vector2(1.5f, 1f);
    public Vector2 size = new Vector2(2.5f, 1f);

    public DamagableEvent OnDamageableHit;
    public NonDamagableEvent OnNonDamageableHit;

    public LayerMask hittableLayers;

    public SpriteRenderer spriteRenderer;
    public bool spriteFlip; //для DamagerEditor, чтобы флипался квадрат в редакторе
    public bool disableDamageAfterHit = false;

    protected bool canDamage = true;
    protected ContactFilter2D attackContactFilter;
    protected Collider2D[] attackOverlapResults = new Collider2D[10];
    protected Transform damagerTransform;
    protected Collider2D lastHit;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //spriteOriginallyFlipped = spriteRenderer.flipX;

        attackContactFilter.layerMask = hittableLayers;
        attackContactFilter.useLayerMask = true;
        attackContactFilter.useTriggers = true;

        damagerTransform = transform;
    }

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    private void FixedUpdate()
    {
        if (!canDamage)
            return;

        Vector2 scale = damagerTransform.lossyScale;

        //умножение offset на scale покомпонентно
        Vector2 facingOffset = Vector2.Scale(offset, scale);

        if (spriteRenderer.flipX)
        {
            facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

            spriteFlip = true;
        }
        else
        {
            spriteFlip = false;
        }

        //умножение scale на size покомпонентно
        Vector2 scaledSize = Vector2.Scale(size, scale);

        Vector2 pointA = (Vector2)damagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;

        int hitCount = Physics2D.OverlapArea(pointA, pointB, attackContactFilter, attackOverlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            lastHit = attackOverlapResults[i];
            Damageable damageable = lastHit.GetComponent<Damageable>();

            if (damageable && !damageable.isDead)
            {
                OnDamageableHit.Invoke(this, damageable);
                damageable.TakeDamage(this);

                if (disableDamageAfterHit)
                {
                    DisableDamage();
                }

            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }
        }
    }
}
