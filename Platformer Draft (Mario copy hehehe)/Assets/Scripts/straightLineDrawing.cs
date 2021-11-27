using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineDrawing : MonoBehaviour
{
    public LineRenderer lineRend;
    public EdgeCollider2D edgeCol;
    private Transform player;
    public float lineLength;
    void Awake()
    {
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players != null && players.Length > 0) { player = players[0].transform; }
        }
    }
    List<Vector2> points;

    public void UpdateLine(Vector2 mousePos)
    {
        if (mousePos == null) { Debug.Log("Null Reference mousePos"); return; }
        if (points == null)
        {
            if ((mousePos.x - player.position.x) * player.transform.localScale.x < 0)
            {
                player.transform.localScale = new Vector3(-1 * player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }
            points = new List<Vector2>();
            Vector2 firstPoint = new Vector2(player.position[0] + 1f * Mathf.Sign(player.transform.localScale.x), player.position[1] + 0.15f);
            Vector2 diff = mousePos - firstPoint;
            float angle = Mathf.Atan2(diff[1], diff[0]);
            Debug.Log(angle);
            Vector2 diffVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Mathf.Min(lineLength, diff.magnitude);
            Vector2 nextPoint = firstPoint + diffVector;
            Vector2 unitVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (nextPoint - firstPoint).magnitude / 20;
            Debug.Log(diff.magnitude);
            Debug.Log((nextPoint - firstPoint).magnitude);
            Vector2 currPoint = firstPoint;


            StartCoroutine(lineDraw(currPoint, unitVector));

            return;
        }
        //Check if mouse has moved enough for us to insert through point
        //If it has: Insert point at mouse position

    }

    IEnumerator lineDraw(Vector2 currPoint, Vector2 unitVector)
    {
        int i = 0;
        Player playerComponent = player.GetComponent<Player>();
        while (i < 20 && Input.GetMouseButton(0) && playerComponent.HasPaint())
        {
            SetPoint(currPoint);
            currPoint = currPoint + unitVector;
            i += 1;
            playerComponent.UsePaint(10);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);

        lineRend.positionCount = points.Count; //All points
        lineRend.SetPosition(points.Count - 1, point);

        if (points.Count > 1)
        {
            edgeCol.points = points.ToArray();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (gameObject.tag == "Poison")
        {
            Poison(col);
        }
        else if (gameObject.tag == "InstantKill")
        {
            InstantKill(col);
        }
        else if (gameObject.tag == "Damage")
        {
            Damage(col);
        }

    }

    void Poison(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if (p != null)
        {
            p.Poison();
        }
        Enemy e = col.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.Poison();
        }
    }

    void InstantKill(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if (p != null)
        {
            p.TakeDamage(p.health);
        }
        Enemy e = col.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(e.health);
        }
    }

    void Damage(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if (p != null)
        {
            p.TakeDamage(1);
        }
        Enemy e = col.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(1);
        }
    }


}
