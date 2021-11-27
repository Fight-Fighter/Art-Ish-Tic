using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    LineDrawing activeLine;

    // Update is called once per frame
    void Update()
    {

        if (!UI_Inventory.IsSelected(Item.ItemType.FreeformPaint) || !UI_Inventory.IsSelected(Item.ItemType.GrapplePaint))
        {
        
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject lineGO = Instantiate(linePrefab);
            activeLine = lineGO.GetComponent<LineDrawing>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }

        if(activeLine != null)
        {
            Vector2 playerPos;
            if (UI_Inventory.IsSelected(Item.ItemType.GrapplePaint)) {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                playerPos = players[0].transform.position;
            }
            else {
                Debug.Log("id d0feigr0grzdokfg");
                playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            activeLine.UpdateLine(playerPos);
        }

    }
}
