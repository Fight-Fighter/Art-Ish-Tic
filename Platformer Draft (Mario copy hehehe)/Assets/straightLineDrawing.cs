using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineDrawing : MonoBehaviour
{
    public LineRenderer lineRend;
    public EdgeCollider2D edgeCol;
    public Transform player;

    List<Vector2> points;

    public void UpdateLine(Vector2 playerPos)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(new Vector2(player.position[0] + 0.5f, player.position[1]));
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
