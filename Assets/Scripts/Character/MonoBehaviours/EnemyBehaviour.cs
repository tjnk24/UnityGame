using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    static protected EnemyBehaviour s_EnemyBehaviourInstance;
    static public EnemyBehaviour EnemyBehaviourInstance { get { return s_EnemyBehaviourInstance; } }

    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;
    Animator animator;

    public LayerMask obstacleLayerMask;
    public LayerMask scanPlayerLayerMask;

    public GameObject projectilePrefab;
    public GameObject bloodPrefab;
    public Transform shootingOrigin;

    public float movementSpeed = 2.0f;
    public float throwForce = 5f;

    public float viewDistance;

    Vector2 throwDirection = Vector2.right;

    int criticalHealth = 2;

    float leftViewFov = 120f;
    float rightViewFov = 260f;

    float rightViewDirection = 180f;
    float leftViewDirection = -180f;

    float randomTime,
          patrolTimer,
          spriteDirection,
          forwardDistance;

    [HideInInspector]
    public int horizontalDirection;

    protected CharacterController2D characterController2D;
    protected Damageable damageable;
    protected ObjectPool bulletPool;
    protected ObjectPool bloodPool;

    protected readonly int HashHorizontalDirection = Animator.StringToHash("HorizontalDirection");
    protected readonly int HashPatrol = Animator.StringToHash("Patrol");
    protected readonly int HashSpotted = Animator.StringToHash("Spotted");
    protected readonly int HashAttack = Animator.StringToHash("Attack");
    protected readonly int HashChase = Animator.StringToHash("Chase");
    protected readonly int HashFlee = Animator.StringToHash("Flee");
    protected readonly int HashDamaged = Animator.StringToHash("Damaged");
    protected readonly int HashDie = Animator.StringToHash("Death");

    private void Awake()
    {
        s_EnemyBehaviourInstance = this;

        characterController2D = GetComponent<CharacterController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        randomTime = Random.Range(1, 4);
        
        forwardDistance = capsuleCollider2D.size.x + 0.2f;

        if (bloodPrefab != null)
        {
            bloodPool = ObjectPool.GetObjectPool(bloodPrefab.gameObject, 6);
        }

        if (projectilePrefab != null)
        {
            bulletPool = ObjectPool.GetObjectPool(projectilePrefab.gameObject, 8);
        }
    }

    void Start()
    {
        SceneLinkedSMB<EnemyBehaviour>.Initialize(animator, this);
        spriteRenderer.flipX = false;

        
    }

    public void UpdateFacing()
    {
        bool faceLeft = horizontalDirection < 0;

        if (faceLeft)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public bool CheckForObstacles()
    {
        Vector2 boxCastSize = capsuleCollider2D.bounds.extents * 0.2f;

        Debug.DrawLine(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.center + Vector3.right * forwardDistance * horizontalDirection, Color.blue);

        if (Physics2D.BoxCast(capsuleCollider2D.bounds.center, boxCastSize, 0f, Vector2.right * horizontalDirection, forwardDistance, obstacleLayerMask))
        {
            return true;
        }

        Vector3 checkForGapPosition = (Vector2)capsuleCollider2D.bounds.center + Vector2.right * horizontalDirection * (capsuleCollider2D.bounds.extents.x + 0.2f); //(Vector2) это метод конверсии из Vector3 в Vector2

        Debug.DrawLine(checkForGapPosition, checkForGapPosition + Vector3.down * (capsuleCollider2D.bounds.extents.y + 0.2f), Color.blue);

        //проверка на пропасть внизу перед собой
        if (!Physics2D.CircleCast(checkForGapPosition, 0.1f, Vector2.down, capsuleCollider2D.bounds.extents.y + 0.2f, characterController2D.groundedLayerMask.value))
        {
            return true;
        }

        return false;
    }

    public bool ScanForPlayer(bool left = true)
    {
        //расстояние между врагом и игроком
        Vector3 dir = PlayerCharacter.PlayerInstance.transform.position - transform.position;

        //если расстояние между врагом и игроком больше чем дистанция обзора, то return
        if (dir.sqrMagnitude > viewDistance * viewDistance)
        {
            return false;
        }

        Vector2 linecastStart = (Vector2)capsuleCollider2D.bounds.center + Vector2.up * (capsuleCollider2D.bounds.extents.y * 0.5f);

        Vector2 playerPosition = PlayerCharacter.PlayerInstance.transform.position;

        RaycastHit2D result = Physics2D.Linecast(linecastStart, playerPosition, scanPlayerLayerMask);

        //Debug.DrawLine(linecastStart, playerPosition, Color.blue);

        //нужно, чтобы игрок не был виден сквозь стены и препятствия
        if (result)
        {
            return false;
        }

        //почему-то работает только такой вилкой-дублем
        if (left)
        {
            //вычисление прямого вектора, исходящего из врага по центру
            //The forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.

            Vector3 testForward = Quaternion.Euler(0, 0, leftViewDirection) * Vector2.right;

            float angle = Vector3.Angle(testForward, dir);

            if (angle > leftViewFov * 0.5f)
            {
                return false;
            }

            horizontalDirection = -1;

            return true;
        }
        else
        {
            Vector3 testForward = Quaternion.Euler(0, 0, rightViewDirection) * Vector2.right;

            float angle = Vector3.Angle(testForward, dir);

            if (angle < rightViewFov * 0.5f)
            {
                return false;
            }

            horizontalDirection = 1;

            return true;
        }
    }
    public void SetPatrol()
    {
        animator.SetTrigger(HashPatrol);
    }
    public void SetSpotted()
    {
        animator.SetTrigger(HashSpotted);
    }

    public void SetAttack()
    {
        animator.SetTrigger(HashAttack);
    }

    public void SetChase()
    {
        animator.SetTrigger(HashChase);
    }

    public void SetFlee()
    {
        animator.SetTrigger(HashFlee);
    }

    public void HorizontalMovement(int direction)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right * direction, movementSpeed * Time.deltaTime);
    }
    public void Patrol(int direction)
    {
        patrolTimer += Time.deltaTime;

        animator.SetFloat(HashHorizontalDirection, direction);

        if (patrolTimer < randomTime)
        {
            Vector3 patrolDestination = transform.position + Vector3.right * direction; 

            transform.position = Vector3.MoveTowards(transform.position, patrolDestination, movementSpeed * Time.deltaTime);
        }
        else
        {
            patrolTimer = 0;
            randomTime = Random.Range(1, 3);

            if (direction != 0)
            {
                horizontalDirection = 0;
            }
            else
            {
                horizontalDirection = Random.Range(-1, 2);
            }
            
        }
        
    }

    public void Shooting()
    {
        //позиция точки спауна бутылки
        Vector2 shootPosition = shootingOrigin.transform.localPosition;
        

        if (throwDirection.x > 0 && horizontalDirection < 0)
        {
            shootingOrigin.transform.localPosition = new Vector2(shootPosition.x * -1, shootPosition.y);
            throwDirection = -throwDirection;

        } else if (throwDirection.x < 0 && horizontalDirection > 0)
        {
            shootingOrigin.transform.localPosition = new Vector2(shootPosition.x * -1, shootPosition.y);
            throwDirection = -throwDirection;
        }

        //достаем из пула предмет
        PoolingObject obj = bulletPool.Pop(shootingOrigin.transform.position);

        //придаём ему движение
        obj.rigidbody2D.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }

    public void Hit()
    {
        animator.SetTrigger(HashDamaged);
    }

    public void Bleed()
    {
        PoolingObject obj = bloodPool.Pop(transform.position);

        bool facingDirection = horizontalDirection < 0;
        bool bleedDirection = obj.transform.rotation.eulerAngles.y == 90;

        //флипаем партикл в нужную сторону
        if (bleedDirection != facingDirection)
        {
            obj.transform.eulerAngles = new Vector3(obj.transform.rotation.eulerAngles.x, obj.transform.rotation.eulerAngles.y * -1, obj.transform.rotation.eulerAngles.z);
        }
    }
    public bool CheckCurrentHealth()
    {
        if (damageable.CurrentHealth < criticalHealth)
        {
            return true;
        }

        return false;
    }

    public void OnDamagedMove()
    {
        Vector3 direction = Vector3.left * horizontalDirection;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movementSpeed * Time.deltaTime);
    }

    public void Die(Damager damager, Damageable damageable)
    {
        animator.SetTrigger(HashDie);
    }

    public void OnDeadColliderResize()
    {
        capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;
        capsuleCollider2D.offset = new Vector2(-0.08f, -1.6f);
        capsuleCollider2D.size = new Vector2(3.6f, 0.6f);
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        
    }
#endif

}
