using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private float poisonTime = 0f;
    public int health = 1;
    public float speed = 2;
    public bool isBoss = false;
    private HealthBar bossHealthBar;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        if (isBoss)
        {
            GameObject[] taggedItems = GameObject.FindGameObjectsWithTag("BossHealthBar");
            if (taggedItems != null && taggedItems.Length > 0) { bossHealthBar = taggedItems[0].GetComponent<HealthBar>(); }
            bossHealthBar.SetMaxHealth(health);
        }
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
        if (isBoss)
        {
            bossHealthBar.SetHealth(health);
        }
        if (health <= 0)
        {
            //anim.SetBool("Dead", true);
            Destroy(gameObject, 0.5f);
            
            if (gameObject.tag == "ArtBlock")
            {
                ArtBlock.Dead = true;
            }
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if (p != null)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.rockSmash);
            p.TakeDamage(1);
        }
    }
}
