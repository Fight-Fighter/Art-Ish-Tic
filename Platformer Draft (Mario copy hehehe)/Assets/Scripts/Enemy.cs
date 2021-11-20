using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private float poisonTime = 0f;
    public int health = 1;
    public float speed = 2;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        poisonTime -= Time.deltaTime;
    }

    public void Poison()
    {
        poisonTime = 5f;
    }

    public void Poison(float time)
    {
        poisonTime = time;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //anim.SetBool("Dead", true);
            Destroy(gameObject, 0.5f);
        }
    }

    public float GetSpeed()
    {
        if (poisonTime > 0)
        {
            return speed / 2;
        }
        return speed;
    }
}
