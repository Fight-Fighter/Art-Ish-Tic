using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Coins : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.name == "MC")
		{

			SoundManager.Instance.PlayOneShot(SoundManager.Instance.getCoin);

			//increaseTextUIScore();

			Destroy(gameObject);
		}
    }

	/*
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
	*/

}
