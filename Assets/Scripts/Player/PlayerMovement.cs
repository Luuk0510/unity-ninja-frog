using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [NonSerialized] public bool hasInfiniteJump = false;
    [NonSerialized] public bool IsAbleToDoubleJump = false;
    [NonSerialized] public bool isAbleToWallJump = false;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource respawnSoundEffect;
    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    private bool canDoubleJump = false;
    private bool isTouchingWall;
    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private enum MovementState { idle, running, jumping, falling, doubleJumping, wall }

    public void EnableDoubleJump()
    {
        IsAbleToDoubleJump = true;
    }

    public void EnableWallJump()
    {
        isAbleToWallJump = true;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        respawnSoundEffect.Play();
        if (Checkpoint.LastCheckpointPosition != Vector3.zero)
        {
            transform.position = Checkpoint.LastCheckpointPosition;
            IsAbleToDoubleJump = Checkpoint.CanDoubleJumpAtLastCheckpoint;
            isAbleToWallJump = Checkpoint.CanWallJumpAtLastCheckpoint;
        }
    }

    private void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(dirX * moveSpeed, rigidBody.velocity.y);

        CheckWall();

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded())
            {
                Jump(jumpForce);
            }
            else if (isTouchingWall && !isGrounded() && isAbleToWallJump)
            {
                WallJump();
            }
            else if (hasInfiniteJump || (!isGrounded() && IsAbleToDoubleJump && canDoubleJump))
            {
                Jump(jumpForce);
                canDoubleJump = false;
            }
        }

        UpdateAnimation();
    }

    private void Jump(float jumpForce)
    {
        jumpSoundEffect.Play();
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        canDoubleJump = IsAbleToDoubleJump;
    }

    private void WallJump()
    {
        int wallJumpDirection = isTouchingWallLeft ? 1 : -1;

        Vector2 forceToAdd = new Vector2(wallJumpDirection * moveSpeed * 0.75f, jumpForce);
        rigidBody.velocity = forceToAdd;

        canDoubleJump = IsAbleToDoubleJump;
    }


    private void CheckWall()
    {
        isTouchingWallLeft = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.left, .1f, jumpableGround);
        isTouchingWallRight = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, .1f, jumpableGround);
        isTouchingWall = isTouchingWallLeft || isTouchingWallRight;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDoubleJump = false;
            isTouchingWall = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isTouchingWall = false;
        }
    }


    private void UpdateAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Appear"))
        {
            return;
        }

        MovementState state;

        if (dirX != 0f)
        {
            state = MovementState.running;
            spriteRenderer.flipX = dirX < 0f;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rigidBody.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rigidBody.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }
        else if (!isGrounded() && rigidBody.velocity.y > 0 && IsAbleToDoubleJump)
        {
            state = MovementState.doubleJumping;
        }

        if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, new Vector2(dirX, 0), .1f, jumpableGround) && isAbleToWallJump)
        {
            state = MovementState.wall;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


}
