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

    public void UpdateLine(Vector2 playerPos)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            Vector2 newPoint = new Vector2(player.position[0] + 0.5f, player.position[1]);
            Vector2 diff = playerPos - newPoint;
            float angle = Mathf.Atan2(diff[1], diff[0]);
            Debug.Log(angle);
            float newY = Mathf.Sin(angle) * lineLength;
            float newX = Mathf.Cos(angle) * lineLength;
            Vector2 nextPoint = new Vector2(newPoint[0] + newX, newPoint[1] + newY);
            SetPoint(newPoint);
            SetPoint(nextPoint);
            return;
        }
        //Check if mouse has moved enough for us to insert through point
        //If it has: Insert point at mouse position

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

    }

    void InstantKill(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player p = col.gameObject.GetComponent<Player>();
            p.TakeDamage(3);
        }

        else if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject, 0.2f);
        }
    }

    void Damage(Collision2D col)
    {
        
    }


}
