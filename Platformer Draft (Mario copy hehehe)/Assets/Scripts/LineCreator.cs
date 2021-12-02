using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    public Player player;
    LineDrawing activeLine;

    // Update is called once per frame
    void Update()
    {

        if (!UI_Inventory.IsSelected(Item.ItemType.FreeformPaint)/* && !UI_Inventory.IsSelected(Item.ItemType.GrapplePaint)*/)
        {
            return;
        }
        bool hasPaint = player.HasPaint();
        if (Input.GetKeyDown(KeyCode.Mouse0) && hasPaint)
        {
            GameObject lineGO = Instantiate(linePrefab);
            activeLine = lineGO.GetComponent<LineDrawing>();
        }

        if(activeLine != null && hasPaint)
        {
            player.UsePaint(10); //change this line to change amount of paint used
            Vector2 playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(playerPos);
        }

        if (Input.GetMouseButtonUp(0) || !hasPaint)
        {
            activeLine = null;
        }

        if(activeLine != null)
        {
            Vector2 playerPos;
            if (UI_Inventory.IsSelected(Item.ItemType.FreeformPaint))
            {
                playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                activeLine.UpdateLine(playerPos);
            }
            else
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                playerPos = new Vector2(players[0].transform.position[0] - 1, players[0].transform.position[1] - 1);
                if (MoveTime.isGrappling)
                {
                    activeLine.UpdateLine(playerPos);
                }
            }
            
        }

    }
}
