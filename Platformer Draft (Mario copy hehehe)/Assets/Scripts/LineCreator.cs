using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
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

        if (Input.GetMouseButtonUp(0))
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
