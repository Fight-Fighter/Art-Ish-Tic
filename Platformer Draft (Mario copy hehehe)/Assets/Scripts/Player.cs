﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 3;
    public UIHealthBar healthBar;
    private Animator anim;
    // Update is called once per frame
    void Awake()
    {
        healthBar.SetHearts(3);
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            Destroy(gameObject, 0.5f);
        }
        healthBar.SetHearts(health);
    }
}