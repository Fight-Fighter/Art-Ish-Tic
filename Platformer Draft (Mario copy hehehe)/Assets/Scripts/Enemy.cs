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

    [Header("Obstacle Avoidance")]
    public ContactFilter2D contactFilter;
    public Collider2D avoidanceRange;
    public bool avoidDamageOnly = false;

    private HealthBar bossHealthBar;
    private Animator anim;
    private Collider2D collider2d;
    void Awake()
    {
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
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

    public Vector2 GetObstacleOffset()
    {

        List<Collider2D> colliders = new List<Collider2D>();
        int numContacts = avoidanceRange.OverlapCollider(contactFilter, colliders);
        Vector2 totalOffset = new Vector2(0, 0);
        for (int i = 0; i < colliders.Count; i++)
        {
            Collider2D other = colliders[i];
            if (other.Equals(collider2d))
            {
                continue;
            }
            if (other.gameObject.GetComponent<Player>() != null)
            {
                continue;
            }

            straightLineDrawing sline = other.gameObject.GetComponent<straightLineDrawing>();
            
            if (avoidDamageOnly && (sline == null || !(sline.paintType == Item.ItemType.PoisonPaint || sline.paintType == Item.ItemType.DamagePaint || sline.paintType == Item.ItemType.KillPaint)))
            {
                continue;
            }
            ColliderDistance2D dist = collider2d.Distance(other);
            if (dist.distance <= 0)
            {
                continue;
            }
            Debug.Log(other);
            Debug.Log(dist.normal * dist.distance);
            totalOffset += 1 / dist.distance * dist.normal;
        }
        
        return totalOffset;
    }

    public Vector2 GetOffsetVelocity(Vector2 direction)
    {
        Debug.Log((direction * speed + GetObstacleOffset()).normalized * speed);
        return (direction * speed + GetObstacleOffset()).normalized * speed;
    }
}
