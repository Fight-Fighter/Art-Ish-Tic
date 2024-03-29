﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : MonoBehaviour
{

    private SpriteRenderer sr;  //Represents this object
    private Collider2D collider2d;

    public Sprite explodedBlock;

    public float spriteChange = .2f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.contacts[0].point.y <= collider2d.bounds.min.y && (col.contactCount == 1 || col.contacts[1].point.y <= collider2d.bounds.min.y))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.rockSmash);

            sr.sprite = explodedBlock;
            Destroy(gameObject, spriteChange);
        }
    }
}
