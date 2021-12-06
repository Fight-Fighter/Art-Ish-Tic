using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EaselWeasel : MonoBehaviour
{
	/*
	public float speed = 2;
	public float jumpSpeed = 5f;
	public float jumpInterval = 5f;
	private Vector2 direction = Vector2.right;
	*/
	private Rigidbody2D rb;
	private Animator anim;
	
	private GameObject player;
	private bool dead = false;
	
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		if (player == null)
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			if (players != null && players.Length > 0) { player = players[0]; }

		}
	}

	private float lastAttack = 0f;
	private float attackCooldown = 5f;

    private void Update()
    {
		if (player != null)
		{
			Vector2 playerDirection = (player.transform.position - transform.position).normalized;
			if (transform.localScale.x * playerDirection.x > 0) { Flip(); }
		}
	}
    void FixedUpdate()
	{
		lastAttack += Time.deltaTime;
		if (lastAttack >= attackCooldown)
        {
			lastAttack = 0f;
			anim.SetTrigger("Attack");
        }
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		// If you hit SnailStart & SnailEnd flip direction
		Flip();

	}

	void Flip()
	{
		//lastTurn = 0f;
		transform.localScale = new Vector2(-1 * transform.localScale.x,
			transform.localScale.y);

		//direction = new Vector2(-1 * direction.x, direction.y);
	}

}
