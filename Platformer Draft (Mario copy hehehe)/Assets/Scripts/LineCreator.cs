using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    private Player player;
    public GameObject grappleLine;
    private GameObject lineGO;
    LineDrawing activeLine;

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null || players.Length == 0) { return; }
        player = players[0].GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused) { return; }
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

        if (Input.GetMouseButtonUp(0) || !hasPaint)
        {
            activeLine = null;
        }

        if(activeLine != null && hasPaint)
        {
            Vector2 playerPos;
            if (UI_Inventory.IsSelected(Item.ItemType.FreeformPaint))
            {
                player.UsePaint(10);
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
