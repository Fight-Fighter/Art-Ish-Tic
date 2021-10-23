using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PrizeBlock : MonoBehaviour
{

	public AnimationCurve anim;

	public int coinsInBlock = 5;

	private Collider2D collider2d;

	void Awake()
	{
		collider2d = GetComponent<Collider2D>();
	}

	void OnCollisionEnter2D(Collision2D col)
	{

		// Check if the collision hit the bottom of the block
		if (col.gameObject.name == "MC" && col.contacts[0].point.y <= collider2d.bounds.min.y && (col.contactCount == 1 || col.contacts[1].point.y <= collider2d.bounds.min.y))
		{

			// Calls RunAnimation which will be paused 
			// and resumed over time
			StartCoroutine(RunAnimation());
			if (coinsInBlock > 0)
			{

				// Play coin sound
				SoundManager.Instance.PlayOneShot(SoundManager.Instance.getCoin);

				// Increase the Score Text component
				increaseTextUIScore();

				coinsInBlock--;

			}
		}

		// If block contains coins
	}

	IEnumerator RunAnimation()
	{

		// Get starting position of PrizeBlock
		Vector2 startPos = transform.position;

		// Cycle through all the keys in the animation curve
		for (float x = 0; x < anim.keys[anim.length - 1].time; x += Time.deltaTime)
		{

			// Change the block position to what is defined
			// on the AnimationCurve
			transform.position = new Vector2(startPos.x,
				startPos.y + anim.Evaluate(x));

			// Continue looping at next update
			yield return null;
		}
	}

	// Increases the score the the text UI name passed
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
}
