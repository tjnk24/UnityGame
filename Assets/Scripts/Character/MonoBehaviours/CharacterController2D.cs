using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterController2D : MonoBehaviour
{
    public bool IsGrounded { get; protected set; }

    public ContactFilter2D contactFilter;
    public LayerMask groundedLayerMask;
    public float groundedRaycastDistance = 0.1f;

    new Rigidbody2D rigidbody2D;
    CapsuleCollider2D capsuleCollider2D;

    RaycastHit2D[] foundHits = new RaycastHit2D[3];
    Collider2D[] groundColliders = new Collider2D[3];
    Vector2[] raycastPositions = new Vector2[3];

    protected Animator animator;
    public float checkCollisionsRadius = 0.3f;

    Vector2 overlapStartBottom;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckCapsuleEndCollisions();
    }

    //!!!вот здесь рабочий код!
    //поиск коллизий, изменение состояния IsGrounded
    //public void CheckCollisions()
    //{
    //    Vector2 overlapStart = rigidbody2D.position + capsuleCollider2D.offset;
    //    overlapStartBottom = new Vector2(overlapStart.x, capsuleCollider2D.bounds.min.y);

    //    //checkCollisionsRadius = capsuleCollider2D.size.x * 0.5f;
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(overlapStartBottom, checkCollisionsRadius, groundedLayerMask);

    //    IsGrounded = colliders.Length > 0;
    //}

    public void CheckCapsuleEndCollisions()
    {

        Vector2 raycastStart;
        Vector2 raycastDirection;
        float raycastDistance;

        RaycastHit2D[] hitBuffer = new RaycastHit2D[3];
        int totalRaycastCount = 0;

        //направление луча
        raycastDirection = Vector2.down;

        raycastStart = rigidbody2D.position + capsuleCollider2D.offset;
        raycastDistance = capsuleCollider2D.size.x * 0.5f + groundedRaycastDistance * 2f;

        Vector2 raycastStartBottomCentre = raycastStart + Vector2.down * (capsuleCollider2D.size.y * 0.5f - capsuleCollider2D.size.x * 0.5f);

        raycastPositions[0] = raycastStartBottomCentre + Vector2.left * capsuleCollider2D.size.x * 0.5f; //левая крайняя точка снизу
        raycastPositions[1] = raycastStartBottomCentre; //нижний центр
        raycastPositions[2] = raycastStartBottomCentre + Vector2.right * capsuleCollider2D.size.x * 0.5f; //правая крайняя точка снизу

        //Debug.DrawLine(raycastStartBottomCentre, raycastPositions[0], Color.red); //левая крайняя точка снизу
        //Debug.DrawLine(raycastStartBottomCentre, raycastPositions[1], Color.green); //нижний центр
        //Debug.DrawLine(raycastStartBottomCentre, raycastPositions[2], Color.blue); //правая крайняя точка снизу

        //пускаем лучи, находим коллайдеры
        for (int i = 0; i < raycastPositions.Length; i++)
        {
            int raycastCount = Physics2D.Raycast(raycastPositions[i], raycastDirection, contactFilter, hitBuffer, raycastDistance);

            Debug.DrawLine(raycastPositions[i], new Vector3(raycastPositions[i].x, raycastPositions[i].y - raycastDistance), Color.green);

            totalRaycastCount += raycastCount;
        }

        //если хотя бы один из трех не ноль, то isgrounded = true
        IsGrounded = totalRaycastCount > 0;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

    }
#endif
}
