using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snail : MonoBehaviour
{

    public float speed = 2f;
    Vector2 direction = Vector2.right;

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }


    void OnTriggerEnter2D(Collider2D col) //Only bounces off of objects that are triggerable
    {
        transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);

        direction = new Vector2(-1 * direction.x, direction.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "MC")
        {
            if(col.contacts[0].point.y > transform.position.y)
            {
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
