﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bee : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
    public float aggroRange = 5f;
    public float patrolRange = 2f; //time before flip while patrolling

    private Collider2D collider2d;
    private Rigidbody2D rb;
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
        collider2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastFlip += Time.deltaTime;
        collisionFollowCooldown -= Time.deltaTime;
        if (dead) { return;  }
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players != null && players.Length > 0) { player = players[0]; }
            
        }
        if (player != null && player.transform != null && collisionFollowCooldown < 0f && (player.transform.position - transform.position).magnitude < aggroRange)
        {
            Vector2 newVelocity = (player.transform.position - transform.position).normalized * speed;
            Vector2 newDirection = (new Vector2(rb.velocity.x, 0)).normalized;
            if (newDirection.x == direction.x || Flip(false))
            {
                rb.velocity = newVelocity;
                direction = newDirection;
            }
            lastPatrolFlip = 0;
            patrolTime = 0;
        } else
        {
            patrolTime += Time.deltaTime;
            lastPatrolFlip += Time.deltaTime;
            direction.y = Mathf.Cos(patrolTime * 5) / speed * 1.5f;
            if (lastPatrolFlip > patrolRange) {
                Flip(false);
            }
            rb.velocity = direction * speed;

        }

        if (rb.velocity.x * transform.localScale.x > 0)
        {
            Flip(true);
            direction = new Vector2(Mathf.Sign(rb.velocity.x), rb.velocity.y);
        }
    }

    private bool Flip(bool cooldownOverride)
    {
        if (lastFlip < 1f && !cooldownOverride) { return false; }
        lastFlip = 0f;
        lastPatrolFlip = 0f;
        transform.localScale = new Vector2(-1 * transform.localScale.x,
            transform.localScale.y);
        direction = new Vector2(direction.x * -1, direction.y);
        return true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        collisionFollowCooldown = 0.4f;
        if (col.contactCount > 1) { Debug.DrawLine(col.contacts[0].point, col.contacts[1].point, Color.red, 1f); }
        Debug.DrawLine(col.contacts[0].point, col.contacts[0].point + col.contacts[0].normal, Color.red, 10000f);
        Debug.Log(col.contacts[0].normal);
        if (col.gameObject.name == "MC")
        {
            if (col.contacts[0].normal.y < 0)
            {
                dead = true;
                GetComponent<Collider2D>().enabled = false; //Removes collider so snall can fall off screen
                Destroy(gameObject, 3);
                increaseTextUIScore();
            }
            else
            {
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rockSmash);
                Destroy(col.gameObject, .5f);
            }
        }
        else if (col.contacts[0].normal.x > 0 && direction.x < 0)
        {
            Flip(true);
        }
        else if (col.contacts[0].normal.x < 0 && direction.x > 0)
        {
            Flip(true);
        }
    }

    private void increaseTextUIScore()
    {

        // Find the Score UI component
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();

        // Get the string stored in it and convert to an int
        int score = int.Parse(textUIComp.text);

        // Increment the score
        score += 10;

        // Convert the score to a string and update the UI
        textUIComp.text = score.ToString();
    }
}
