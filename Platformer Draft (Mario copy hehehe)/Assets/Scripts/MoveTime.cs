using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTime : MonoBehaviour
{

    public float speed = 5f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;

    public bool facingRight = true;

    public float jumpSpeed = 5f;

    bool isJumping = false;

    private float rayCastLength = 0.005f;

    private float width;
    private float height;

    private float jumpButtonPressTime;

    private float maxJumpTime = 0.2f;

    public float wallJumpY = 10f;



    void FixedUpdate()
    {
        float horMove = Input.GetAxisRaw("Horizontal");
        Vector2 vect = rb.velocity;
        rb.velocity = new Vector2(horMove * speed, vect.y);

        if(WallSide() && !IsOnGround() && horMove == 1)
        {
            rb.velocity = new Vector2(-GetWallDirection() * speed * -.75f, wallJumpY);
        }

        animator.SetFloat("Speed", Mathf.Abs(horMove));

        if(horMove > 0 && !facingRight) {
            FlipMC();
            }
        
        else if (horMove < 0 && facingRight)
        {
            FlipMC();
        }

        float vertMove = Input.GetAxis("Jump"); //Refers to the control menu

        if(IsOnGround() && isJumping == false)
        {
            if (vertMove > 0f)
            {
                isJumping = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.jump);
            }
        }

        if(jumpButtonPressTime > maxJumpTime) //If I hold down the jump button, I keep hopping
        {
            vertMove = 0f;  
        }

        if (isJumping && (jumpButtonPressTime < maxJumpTime))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed); //It keeps the x vector the same but the y vector changes in regards to speed
        }

        if(vertMove >= 1f)
        {
            jumpButtonPressTime += Time.deltaTime;
        }

        else
        {
            isJumping = false;
            jumpButtonPressTime = 0;
        }
    }
        

    void Awake() //This appears to be the closest thing to an initializer
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f; //Gets dimensions of hitbox
    }

    void FlipMC()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public bool IsOnGround()
    {
        //Checks if you're on the ground
        bool groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height),
            -Vector2.up, rayCastLength);

        //Checks if you hit a wall to your right when jumping
        bool groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height),
            -Vector2.up, rayCastLength);

        //Checks if you hit a wall to your left when jumping
        bool groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height),
            -Vector2.up, rayCastLength);

        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        
        return false;
    }

    void OnBecameInvisible()  //Seems to be a specific method name, can't change it
    {
        Debug.Log("MC died lololol");
        Destroy(gameObject);
    }

    public bool IsWallOnLeft()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y), 
            -Vector2.right, rayCastLength);
    }

    public bool IsWallOnRight()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y),
            Vector2.right, rayCastLength);
    }

    public bool WallSide()
    {
        if(IsWallOnLeft() || IsWallOnRight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWallDirection()
    {
        if(IsWallOnLeft())
        {
            return -1;
        } else if (IsWallOnRight())
        {
            return 1;
        } else
        {
            return 0;
        }
    }

}
