using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Coins : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.getCoin);

        increaseTextUIScore();

        Destroy(gameObject);
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

}
