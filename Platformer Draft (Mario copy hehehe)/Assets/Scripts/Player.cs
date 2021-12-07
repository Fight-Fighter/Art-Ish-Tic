using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 3;
    public float speed = 5f;
    public UIHealthBar healthBar;
    private float poisonTime = 0f;
    private Animator anim;
    public static Player Instance;
    public bool dead = false;
    // Update is called once per frame

    [SerializeField] private UI_Inventory uiInventory;
    public Inventory inventory;

    void Awake()
    {
        healthBar.SetHearts(3);
        anim = GetComponent<Animator>();
        Instance = this;

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }

    private void Update()
    {
        poisonTime -= Time.deltaTime;
        if (transform.position.y < -30) { TakeDamage(health); }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            if (inventory.AddItem(itemWorld.GetItem()))
            {
                itemWorld.DestroySelf();
            }
        }
    }
        public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            Destroy(gameObject, 0.5f);
            dead = true;
        }
        healthBar.SetHearts(Mathf.Max(health, 0));
    }

    void OnBecameInvisible()
    {
        if (dead)
        {
            SceneManager.LoadScene(11);
        }
    }

    public void Poison()
    {
        poisonTime = 5f;
    }

    public void Poison(float time)
    {
        poisonTime = time;
    }
    public float GetSpeed()
    {
        if (poisonTime > 0)
        {
            return speed / 2;
        }
        return speed;
    }

    public bool HasPaint()
    {
        return inventory.HasItem((Item.ItemType) UI_Inventory.selection);
    }

    public bool HasPaint(Item.ItemType itype)
    {
        return inventory.HasItem(itype);
    }

    public void UsePaint(float amt)
    {
        inventory.RemoveItem(new Item { itemType = (Item.ItemType) UI_Inventory.selection, amount = amt});
    }

    public void UsePaint(Item.ItemType t, float amt)
    {
        inventory.RemoveItem(new Item { itemType = t, amount = amt });
    }
}
