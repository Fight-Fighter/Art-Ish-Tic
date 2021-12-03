using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public float aggroRange = 5f;
    public float patrolRange = 2f; //time before flip while patrolling

    private Rigidbody2D rb;
    private Enemy enemy;
    private GameObject player = null;
    private Vector2 direction = Vector2.left;
    private bool dead = false;
    private float lastFlip = 0f;
    private float lastPatrolFlip = 0f;
    private float collisionFollowCooldown = 0f;
    private float patrolTime = 0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players != null && players.Length > 0) { player = players[0]; }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastFlip += Time.deltaTime;
        if (dead) { return; }

        float actualSpeed = enemy.GetSpeed();

        if (player != null && player.transform != null && (player.transform.position - transform.position).magnitude < aggroRange)
        {
            Vector2 newVelocity = (player.transform.position - transform.position).normalized * actualSpeed;
            Vector2 newDirection = (new Vector2(rb.velocity.x, 0)).normalized;
            if (newDirection.x == direction.x)
            {
                rb.velocity = newVelocity;
                direction = newDirection;
            }
            lastPatrolFlip = 0;
            patrolTime = 0;
        }
        else
        {
            patrolTime += Time.deltaTime;
            lastPatrolFlip += Time.deltaTime;
            direction.y = Mathf.Cos(patrolTime * 5) / actualSpeed * 1.5f;
            rb.velocity = direction * actualSpeed;

        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        Debug.Log("Direct hit!");
        if (p != null)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.rockSmash);
            p.TakeDamage(1);
        }

    }
}
