using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 3;
    public float speed = 5f;
    public UIHealthBar healthBar;
    private float poisonTime = 0f;
    private Animator anim;
    public static Player Instance;
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
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //anim.SetBool("Dead", true);
            Destroy(gameObject, 0.5f);
        }
        healthBar.SetHearts(Mathf.Max(health, 0));
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
}
