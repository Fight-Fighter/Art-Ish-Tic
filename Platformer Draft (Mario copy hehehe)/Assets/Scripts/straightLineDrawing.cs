using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineDrawing : MonoBehaviour
{
    public LineRenderer lineRend;
    public EdgeCollider2D edgeCol;
    private Transform player;

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
            SetPoint(newPoint);
            SetPoint(playerPos);
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
}
