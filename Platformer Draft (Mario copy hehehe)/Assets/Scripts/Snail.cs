using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snail : MonoBehaviour
{

	public float speed = 2;
	public float jumpSpeed = 5f;
	public float jumpInterval = 5f;
	private Collider2D collider2d;
	private Rigidbody2D rb;
	// Snail Direction
	private Vector2 direction = Vector2.right;

	private bool isJumping = false;
	private bool shouldApproachEdge = false;
	private bool isJumpingOverGap = false;
	private float timeToStop = 0f;
	private float jumpingTimeElapsed = 0f;
	private bool dead = false;
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
	}

	private float lastTurn = 0f;
	private float lastJump = 0f;
	private float maxJumpTime = 3f;
	void FixedUpdate()
	{
		if (dead) { return; }
		lastTurn += Time.deltaTime;
		lastJump += Time.deltaTime;
		// Move the snail
		bool onGround = IsOnGround();
		if (onGround && !shouldApproachEdge) { timeToStop = 0; }
		if (onGround || jumpingTimeElapsed > maxJumpTime) { isJumping = false; isJumpingOverGap = false; jumpingTimeElapsed = 0; }
		else { jumpingTimeElapsed += Time.deltaTime; }

		if (jumpingTimeElapsed <= timeToStop || !isJumpingOverGap) { rb.velocity = new Vector2(direction.x * speed, rb.velocity.y); }
		else if (jumpingTimeElapsed > timeToStop) {
			rb.velocity = new Vector2(0, rb.velocity.y); 
		}
		if (onGround) { FollowPlayer(); }

		bool groundAhead = IsGroundAhead();
		if (!isJumping && !shouldApproachEdge && !groundAhead) {
			timeToStop = CanJumpOverGap();
			if (timeToStop > 0)
			{
				shouldApproachEdge = true;
			} else {
				Debug.Log("Flipping because no ground ahead");
				FlipSnail();
			}
		} 
		if (shouldApproachEdge && !IsGroundAheadOfCenter())
        {
			shouldApproachEdge = false;
			isJumpingOverGap = true;
			SnailJump(true);
		}

		if (!isJumpingOverGap && !groundAhead && !shouldApproachEdge && IsAbyssAhead()) { FlipSnail(); Debug.Log("Flipping because abyss ahead"); }

		if (onGround && !isJumping && !shouldApproachEdge) { SnailJump(false); }
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		// If you hit SnailStart & SnailEnd flip direction
		FlipSnail();

	}

	void FlipSnail()
	{
		lastTurn = 0f;
		transform.localScale = new Vector2(-1 * transform.localScale.x,
			transform.localScale.y);

		direction = new Vector2(-1 * direction.x, direction.y);
	}

	void SnailJump(bool cooldownOverride)
	{
		if (lastJump < jumpInterval && !cooldownOverride) { return; }
		lastJump = 0f;
		rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
		isJumping = true;
	}

	void FollowPlayer()
	{

		if (lastTurn < 1f) { return; }
		Debug.Log("Following player");
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players == null || players.Length == 0) { return; }
		GameObject player = players[0];
		if (player.transform.position.x <= transform.position.x && direction.x > 0) { FlipSnail(); }
		else if (player.transform.position.x >= transform.position.x && direction.x < 0) { FlipSnail(); }
	}

	void OnCollisionEnter2D(Collision2D col)
    {
		if (col.contactCount > 1) { Debug.DrawLine(col.contacts[0].point, col.contacts[1].point, Color.red, 1f); }
        if(col.gameObject.name == "MC")
        {
			Debug.Log(col.contacts[0].point);
			Debug.Log(col.contacts[1].point);
			if (col.contacts[0].point.y >= collider2d.bounds.max.y)
            {
				dead = true;
                GetComponent<Animator>().SetTrigger("Dead");
                GetComponent<Collider2D>().enabled = false; //Removes collider so snall can fall off screen
                direction = new Vector2(direction.x, -1);
                DestroyObject(gameObject, 3);
                increaseTextUIScore();
            } else
            {
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rockSmash);
                DestroyObject(col.gameObject, .5f);
            }
        }
		else if (col.contacts[0].point.x <= collider2d.bounds.min.x && direction == Vector2.left && (col.contactCount == 1 || col.contacts[1].point.x <= collider2d.bounds.min.x))
		{
			Debug.Log("Collided on the left");
			FlipSnail();
		}
		else if (col.contacts[0].point.x >= collider2d.bounds.max.x && direction == Vector2.right && (col.contactCount == 1 || col.contacts[1].point.x >= collider2d.bounds.max.x))
		{
			Debug.Log("Collided on the right");
			FlipSnail();
		}
	}

    void increaseTextUIScore()
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
    public float groundRayCastLength = 0.01f;

    public bool IsOnGround()
    {

        // Check if contacting the ground straight down
        bool groundCheck1 = Physics2D.Raycast(new Vector2(
                                collider2d.bounds.center.x,
                                collider2d.bounds.min.y-0.01f),
                                -Vector2.up, groundRayCastLength);
        return groundCheck1;

    }

	public float groundCheckAheadTime = 0.01f;
	public bool IsGroundAhead()
    {
		if (direction.x > 0)
		{
			return Physics2D.Raycast(new Vector2(collider2d.bounds.max.x + rb.velocity.x * groundCheckAheadTime, collider2d.bounds.min.y - 0.01f), -Vector2.up, groundRayCastLength);
		} else
        {
			return Physics2D.Raycast(new Vector2(collider2d.bounds.min.x + rb.velocity.x * groundCheckAheadTime, collider2d.bounds.min.y - 0.01f), -Vector2.up, groundRayCastLength);
		}
    }
	public bool IsAbyssAhead()
	{
		if (direction.x > 0)
		{
			return !Physics2D.Raycast(new Vector2(collider2d.bounds.max.x + rb.velocity.x * groundCheckAheadTime, collider2d.bounds.min.y - 0.01f), -Vector2.up, 10);
		}
		else
		{
			return !Physics2D.Raycast(new Vector2(collider2d.bounds.min.x + rb.velocity.x * groundCheckAheadTime, collider2d.bounds.min.y - 0.01f), -Vector2.up, 10);
		}
	}
	public bool IsGroundAheadOfCenter()
	{
		return Physics2D.Raycast(new Vector2(collider2d.bounds.center.x + rb.velocity.x * groundCheckAheadTime, collider2d.bounds.min.y - 0.01f), -Vector2.up, groundRayCastLength);
	}

	public float CanJumpOverGapWithRayCast()
    {
		float timeStep = -1 * jumpSpeed / Physics2D.gravity.y / 2;
		float maxPositions = 5;
		float maxTime = timeStep * maxPositions;
        float maxLanding = 20f;
		//check collisions for the top of the collider instead of the center, since it is a jump
        Vector2 topPos = new Vector2(transform.position.x, collider2d.bounds.max.y);
		//Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 currVelocity = new Vector2(direction.x * speed, jumpSpeed);
		float lastLandingTime = 0;
		for (float currtime = 0; currtime < maxTime; currtime += timeStep) {
			Vector2 finalVelocity = currVelocity + timeStep * Physics2D.gravity;
			Vector2 distanceTraveled = currVelocity * timeStep + timeStep * timeStep * Physics2D.gravity / 2;
            RaycastHit2D rch = Physics2D.Raycast(topPos, distanceTraveled, Convert.ToSingle(Math.Sqrt(distanceTraveled.magnitude)));
			Debug.DrawLine(topPos, topPos + distanceTraveled, Color.white, 1f);
			if (rch.collider != null && rch.collider != collider2d)
            {
				if (distanceTraveled.y > 0) { return 0; }
            }
			topPos = distanceTraveled + topPos;
            currVelocity = finalVelocity;
			//technically landing should check from the bottom instead of the top but I got lazy
            Debug.DrawLine(topPos, topPos + maxLanding * -Vector2.up, Color.red, 1f);
			bool isLanding = Physics2D.Raycast(topPos, -Vector2.up, maxLanding);

            if (isLanding) { lastLandingTime = currtime + timeStep; }

		}
		return lastLandingTime;
    }

	public float CanJumpOverGap()
	{
		float timeStep = -1 * jumpSpeed / Physics2D.gravity.y / 2;
		float maxPositions = 5;
		float maxTime = timeStep * maxPositions;
		float maxLanding = 20f;
        //check collisions for the top of the collider instead of the center, since it is a jump
        //Vector2 topPos = new Vector2(transform.position.x, collider2d.bounds.max.y);
        Vector2 currPos = collider2d.bounds.center + new Vector3(collider2d.bounds.extents.x, 0, 0);
		Vector2 topRight = currPos + new Vector2(collider2d.bounds.extents.x, collider2d.bounds.extents.y);
        Vector2 topLeft = currPos + new Vector2(-collider2d.bounds.extents.x, collider2d.bounds.extents.y);
		Vector2 bottomRight = currPos + new Vector2(collider2d.bounds.extents.x, -collider2d.bounds.extents.y);
		Vector2 bottomLeft = currPos + new Vector2(-collider2d.bounds.extents.x, -collider2d.bounds.extents.y);

		Vector2 currVelocity = new Vector2(direction.x * speed, jumpSpeed);
		float lastLandingTime = 0;
		for (float currtime = 0; currtime < maxTime; currtime += timeStep)
		{
			Vector2 finalVelocity = currVelocity + timeStep * Physics2D.gravity;
			Vector2 distanceTraveled = currVelocity * timeStep + timeStep * timeStep * Physics2D.gravity / 2;
			RaycastHit2D rch = Physics2D.BoxCast(currPos+new Vector2(0.01f, 0.01f), collider2d.bounds.extents, 0, distanceTraveled, Convert.ToSingle(Math.Sqrt(distanceTraveled.magnitude)));
			Debug.DrawLine(currPos, currPos + distanceTraveled, Color.white, 1f);
			Debug.DrawLine(topLeft, topLeft + distanceTraveled, Color.white, 1f);
			Debug.DrawLine(topRight, topRight + distanceTraveled, Color.white, 1f);
			Debug.DrawLine(bottomLeft, bottomLeft + distanceTraveled, Color.white, 1f);
			Debug.DrawLine(bottomRight, bottomRight + distanceTraveled, Color.white, 1f);
			if (rch.collider != null && rch.collider != collider2d)
			{
				Debug.Log(rch.collider);
				if (distanceTraveled.y > 0) { return 0; }
			}
			currPos = distanceTraveled + currPos;
			topRight = currPos + new Vector2(collider2d.bounds.extents.x, collider2d.bounds.extents.y);
			topLeft = currPos + new Vector2(-collider2d.bounds.extents.x, collider2d.bounds.extents.y);
			bottomRight = currPos + new Vector2(collider2d.bounds.extents.x, -collider2d.bounds.extents.y);
			bottomLeft = currPos + new Vector2(-collider2d.bounds.extents.x, -collider2d.bounds.extents.y);
			currVelocity = finalVelocity;

			bool isLanding = Physics2D.Raycast(currPos + new Vector2(0, -collider2d.bounds.extents.y), -Vector2.up, maxLanding);
			Debug.DrawLine(currPos + new Vector2(0, -collider2d.bounds.extents.y), currPos + new Vector2(0, -collider2d.bounds.extents.y) + maxLanding * -Vector2.up, Color.red, 1f);
			//technically landing should check from the bottom instead of the top but I got lazy
			/*
			bool isLanding = Physics2D.BoxCast(currPos, collider2d.bounds.size, 0, -Vector2.up, maxLanding);
			Debug.DrawLine(currPos, currPos + maxLanding * -Vector2.up, Color.red, 1f);
			Debug.DrawLine(topLeft, topLeft + maxLanding * -Vector2.up, Color.red, 1f);
			Debug.DrawLine(topRight, topRight + maxLanding * -Vector2.up, Color.red, 1f);
			Debug.DrawLine(bottomLeft, bottomLeft + maxLanding * -Vector2.up, Color.red, 1f);
			Debug.DrawLine(bottomRight, bottomRight + maxLanding * -Vector2.up, Color.red, 1f);
			*/


			if (isLanding) { lastLandingTime = currtime + timeStep; }
		}
		return lastLandingTime;
	}
}
