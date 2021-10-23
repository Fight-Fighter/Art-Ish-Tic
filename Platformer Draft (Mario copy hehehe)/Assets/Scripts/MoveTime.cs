using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTime : MonoBehaviour
{

    public float speed = 5f;
    public LayerMask groundMask;
    private ContactFilter2D groundFilter;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D collider2d;

    public bool facingRight = true;

    public float jumpSpeed = 5f;

    bool isJumping = false;

    //private float rayCastLength = 0.01f;
    
    private float width;
    private float height;


    private float jumpButtonPressTime;

    private float maxJumpTime = 0.2f;

    public float wallJumpY = 10f;

    public Camera mainCamera;
    public LineRenderer lineRend;
    public DistanceJoint2D dist;
    public bool isGrappling;
    private float grappleTime = 0;

    void Update()
    {
        checkGrapple();
    }

    void FixedUpdate()
    {
        

        /*if(WallSide() && !IsOnGround() && horMove == 1)
        {
            rb.velocity = new Vector2(-GetWallDirection() * speed * -.75f, wallJumpY);
        }*/


        if (!isGrappling)
        {

            float horMove = Input.GetAxisRaw("Horizontal");
            Vector2 vect = rb.velocity;
            rb.velocity = new Vector2(horMove * speed, vect.y);
            animator.SetFloat("Speed", Mathf.Abs(horMove));

            if (horMove > 0 && !facingRight)
            {
                FlipMC();
            }

            else if (horMove < 0 && facingRight)
            {
                FlipMC();
            }

            float vertMove = Input.GetAxis("Jump"); //Refers to the control menu

            if (IsOnGround() && isJumping == false)
            {
                if (vertMove > 0f)
                {
                    isJumping = true;
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.jump);
                }
            }

            if (jumpButtonPressTime > maxJumpTime) //If I hold down the jump button, I keep hopping
            {
                vertMove = 0f;
            }

            if (isJumping && (jumpButtonPressTime < maxJumpTime))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed); //It keeps the x vector the same but the y vector changes in regards to speed
            }

            if (vertMove >= 1f)
            {
                jumpButtonPressTime += Time.deltaTime;
            }

            else
            {
                isJumping = false;
                jumpButtonPressTime = 0;
            }
        }
    }
        
    void checkGrapple()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && grappleTime == 0)
        {
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lineRend.SetPosition(0, mousePos);
            lineRend.SetPosition(1, transform.position);
            dist.connectedAnchor = mousePos;
            dist.enabled = true;
            lineRend.enabled = true;
            isGrappling = true;
            grappleTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            dist.enabled = false;
            grappleTime = 0;
            isGrappling = false;
        }
        if (dist.enabled)
        {
            lineRend.SetPosition(1, transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        dist.enabled = false;
        isGrappling = false;
    }

    void Awake() //This appears to be the closest thing to an initializer
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        dist.enabled = false;
        isGrappling = false;

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f; //Gets dimensions of hitbox
    }

    void Start()
    {
        groundFilter.SetLayerMask(groundMask);
        groundFilter.useLayerMask = true;
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
        //bool groundCheck1 = Physics2D.Raycast(new Vector2(collider2d.bounds.center.x, collider2d.bounds.min.y - 0.01f), -Vector2.up, rayCastLength);
        //Debug.Log(groundCheck1);
        /*//Checks if you hit a wall to your right when jumping
        bool groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height),
            -Vector2.up, rayCastLength);

        //Checks if you hit a wall to your left when jumping
        bool groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height),
            -Vector2.up, rayCastLength);*/
        /*
        if (groundCheck1)
        {
            return true;
        }
        
        return false;
        */
        ContactPoint2D[] contacts = new ContactPoint2D[8];
        int numContacts = collider2d.GetContacts(contacts);
        for (int i = 0;i<numContacts;i++)
        {
            if (contacts[i].point.y < collider2d.bounds.min.y) { return true; }
        }
        return false;
    }

    void OnBecameInvisible()  //Seems to be a specific method name, can't change it
    {
        Debug.Log("MC died lololol");
        Debug.Log(rb.position.x);
        Debug.Log(rb.position.y);
        Destroy(gameObject);
    }

    /*public bool IsWallOnLeft()
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
    }*/

    /*public int GetWallDirection()
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
    }*/

}
