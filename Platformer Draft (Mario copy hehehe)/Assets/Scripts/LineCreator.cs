using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    public Player player;
    public GameObject grappleLine;
    private GameObject lineGO;
    LineDrawing activeLine;

    // Update is called once per frame
    void Update()
    {

        if (!UI_Inventory.IsSelected(Item.ItemType.FreeformPaint) && !UI_Inventory.IsSelected(Item.ItemType.GrapplePaint))
        {
            return;
        }
        bool hasPaint = player.HasPaint();
        if (Input.GetKeyDown(KeyCode.Mouse0) && hasPaint)
        {
            if (UI_Inventory.IsSelected(Item.ItemType.FreeformPaint))
            {
                lineGO = Instantiate(linePrefab);
            }
            else
            {
                lineGO = Instantiate(grappleLine);
            }
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
                Vector2 linePos = players[0].transform.position - (players[0].transform.localScale * 0.25f) - (players[0].transform.up * 0.15f);
                playerPos = new Vector2(linePos.x, linePos.y);
                if (MoveTime.isGrappling)
                {
                    activeLine.UpdateLine(playerPos);
                }
            }
            
        }

    }
}
