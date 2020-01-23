using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    static protected PlayerCharacter s_PlayerInstance;
    static public PlayerCharacter PlayerInstance { get { return s_PlayerInstance; } }

    public SpriteRenderer spriteRenderer;
    public Damager meeleeDamager;

    public float movementSpeed = 30.0f;
    public float jumpForce = 6.0f;

    [HideInInspector]
    public float horizontalMovementIndex;

    float verticalMovementIndex = 1;

    float previousPositionY;
    float currentPositionY;

    bool jumpFlag;

    protected CharacterController2D characterController2D;
    protected Animator animator;
    protected Rigidbody2D rigidBody2D;
    protected CapsuleCollider2D capsuleCollider2D;

    protected readonly int HashHorizontalSpeed = Animator.StringToHash("HorizontalSpeed");
    protected readonly int HashVerticalSpeed = Animator.StringToHash("VerticalSpeed");
    protected readonly int HashGrounded = Animator.StringToHash("IsGrounded");
    protected readonly int HashCrouching = Animator.StringToHash("IsCrouching");
    protected readonly int HashMeleeAttack = Animator.StringToHash("MeleeAttack");
    protected readonly int HashWalkMeleeAttack = Animator.StringToHash("WalkMeleeAttack");
    protected readonly int HashGroundedDamaged = Animator.StringToHash("GroundedDamaged");
    protected readonly int HashAirborneDamaged = Animator.StringToHash("AirborneDamaged");
    protected readonly int HashDied = Animator.StringToHash("Died");

    private void Awake()
    {
        s_PlayerInstance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();

        characterController2D = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        meeleeDamager.DisableDamage();

        SceneLinkedSMB<PlayerCharacter>.Initialize(animator, this);

        previousPositionY = transform.position.y;

    }

    private void FixedUpdate()
    {
        animator.SetFloat(HashHorizontalSpeed, horizontalMovementIndex);
        animator.SetFloat(HashVerticalSpeed, verticalMovementIndex);

        if (jumpFlag)
        {
            verticalMovementIndex = 0; //нужно, чтобы возвести BlendTree в нужное состояние, чтобы не было лага в анимации
            rigidBody2D.velocity = new Vector2(0, jumpForce);

            jumpFlag = false;
        }
    }

    public void UpdateFacing()
    {
        bool faceLeft = Input.GetAxis("Horizontal") < 0;
        bool faceRight = Input.GetAxis("Horizontal") > 0;

        if (faceLeft)
        {
            spriteRenderer.flipX = true;

        }
        else if (faceRight)
        {
            spriteRenderer.flipX = false;
        }
    }

    public bool CheckForGrounded()
    {
        bool grounded = characterController2D.IsGrounded;

        animator.SetBool(HashGrounded, grounded);

        return grounded;
    }

    public bool CheckForCrouching()
    {
        bool crouching = PlayerInput.Instance.Vertical.Value < 0f;
        animator.SetBool(HashCrouching, crouching);

        return crouching;
    }

    public bool CheckForJumpInput()
    {
        return Input.GetKeyDown(PlayerInput.Instance.Jump);
    }

    public bool CheckForMeleeAttackInput()
    {
        return Input.GetKeyDown(PlayerInput.Instance.MeleeAttack);
    }

    public void SetMeleeAttack()
    {

        animator.SetTrigger(HashMeleeAttack);
    }

    public void SetWalkMeleeAttack()
    {

        animator.SetTrigger(HashWalkMeleeAttack);
    }

    public void EnableMeeleeAttack()
    {
        meeleeDamager.EnableDamage();
        meeleeDamager.disableDamageAfterHit = true;
    }

    public void DisableMeeleeAttack()
    {
        meeleeDamager.DisableDamage();
        
    }

    public void OnDamaged(Damager damager, Damageable damageable)
    {
        if (characterController2D.IsGrounded)
        {
            animator.SetTrigger(HashGroundedDamaged);
        }
        else
        {
            animator.SetTrigger(HashAirborneDamaged);
        }

    }

    public void Die(Damager damager, Damageable damageable)
    {
        capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;
        animator.SetTrigger(HashDied);
    }

    public void HorizontalMovement()
    {
        Vector3 direction = Vector3.right * PlayerInput.Instance.Horizontal.Value;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movementSpeed * Time.deltaTime);

        horizontalMovementIndex = PlayerInput.Instance.Horizontal.Value * movementSpeed;
    }

    public void VerticalAnimation()
    {
        currentPositionY = transform.position.y;

        if (currentPositionY > previousPositionY)
        {
            verticalMovementIndex++;
        }

        previousPositionY = currentPositionY;
    }

    public void Jump()
    {
        //characterController2D.IsGrounded = false;
        //rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //transform.Translate(Vector3.up * jumpForce * Time.deltaTime, Space.World);

        jumpFlag = true;
    }

    //на случай, если приседать захочется методом
    //public void OnCrouchModifyCollider()
    //{
    //    Vector2 newCrouchCollHeight = new Vector2(0, crouchCollHeight);

    //    if (CheckForCrouching())
    //    {
    //        newCrouchCollHeight = -newCrouchCollHeight;
    //    }

    //    capsuleCollider2D.size += newCrouchCollHeight;
    //    capsuleCollider2D.offset += newCrouchCollHeight * 0.5f;
    //}


}
