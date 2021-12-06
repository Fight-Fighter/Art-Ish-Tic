using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtBlock : MonoBehaviour
{
    public float aggroRange = 5f;
    public float postJumpRestingTime = .5f;
    private ArtBlockState state;
    public GameObject player { get; private set; }
    public static bool Dead = false;
    public static float poisonTime;
    private Enemy enemy;
    // Start is called before the first frame update
    void Awake()
    {
        state = new IdleState(this);
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players != null && players.Length > 0) { player = players[0]; }
        }
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ArtBlockState next = state.Update();
        if (next != null)
        {
            state = next;
            //Debug.Log("Entering state " + next.GetType().ToString());
        }
        poisonTime -= Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        if (Dead)
        {
            SceneManager.LoadScene(3);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col);
        Player p = col.gameObject.GetComponent<Player>();

        if (p != null)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.rockSmash);
            p.TakeDamage(1);
        }

        straightLineDrawing line = col.gameObject.GetComponent<straightLineDrawing>();
        if (line != null)
        {
            line.BossTriggerEntered(gameObject);
            
        }

    }
    /*
    public void LineContact(string tag)
    {
        if (tag == "Poison")
        {
            Poison();
        }
        else if (tag == "InstantKill")
        {
            TakeDamage(3);
        }
        else if (tag == "Damage")
        {
            TakeDamage(1);
        }
    }

    public void Poison()
    {
        poisonTime = 5f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //anim.SetBool("Dead", true);
            Destroy(gameObject, 0.5f);
            if (gameObject.tag == "ArtBlock")
            {
                Dead = true;
            }
        }
    }
    */
}

abstract class ArtBlockState 
{
    protected ArtBlock artblock;
    
    public ArtBlockState(ArtBlock artblock)
    {
        this.artblock = artblock;
    }
    public abstract ArtBlockState Update();
}

class IdleState : ArtBlockState
{
    public IdleState(ArtBlock artBlock) : base(artBlock)
    {

    }
    public override ArtBlockState Update()
    {
        float distance = (artblock.player.transform.position - artblock.transform.position).magnitude;
        if (distance < artblock.aggroRange) return new JumpingState(artblock);
        return null;
    }
}

class JumpingState : ArtBlockState
{
    private Vector3 targetPos;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 5;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 20;


    Vector3 startPos;


    /// 
    /// This is a 2D version of Quaternion.LookAt; it returns a quaternion
    /// that makes the local +X axis point in the given forward direction.
    /// 
    /// forward direction
    /// Quaternion that rotates +X to align with forward
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    public JumpingState(ArtBlock artBlock) : base(artBlock)
    {
        startPos = artblock.transform.position;
        if (artblock.player != null)
        {
            targetPos = artblock.player.transform.position;
        }
    }
    public override ArtBlockState Update()
    {

        // Compute the next position, with arc added in
        float x0 = startPos.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        if (Mathf.Abs(dist) < 10) return new RestingState(artblock);
        float nextX;
        if (ArtBlock.poisonTime > 0) {
            nextX = Mathf.MoveTowards(artblock.transform.position.x, x1, speed * Time.deltaTime / 1.5f);
        }
        else
        {
            nextX = Mathf.MoveTowards(artblock.transform.position.x, x1, speed * Time.deltaTime);
        }
        float baseY = Mathf.Lerp(startPos.y, targetPos.y, Mathf.Min((nextX - x0) / dist, 1));
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / Mathf.Min(-0.25f * dist * dist, 1);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, artblock.transform.position.z);

        
        artblock.transform.position = nextPos;

        // Do something when we reach the target
        if (nextPos == targetPos) return new RestingState(artblock);
        return null;
    }
}

class RestingState : ArtBlockState
{
    private float timeResting = 0.0f;
    public RestingState(ArtBlock artBlock) : base(artBlock)
    {

    }
    public override ArtBlockState Update()
    {
        if (timeResting > artblock.postJumpRestingTime) return new JumpingState(artblock);
        timeResting += Time.deltaTime;
        return null;
    }

}

