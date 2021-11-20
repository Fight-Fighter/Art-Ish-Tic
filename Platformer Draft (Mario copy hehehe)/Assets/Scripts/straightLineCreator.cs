using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject instantKillPrefab;
    public GameObject damagePrefab;
    public GameObject poisonPrefab;
    private GameObject lineGO;
    straightLineDrawing activeLine;

    void Update()
    {

        if (MoveTime.freeSelected || MoveTime.grappleSelected)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (MoveTime.damageSelected)
            {
                Debug.Log("selection works");
                lineGO = Instantiate(damagePrefab);
            }
            else if (MoveTime.poisonSelected)
            {
                lineGO = Instantiate(poisonPrefab);
            }
            else if (MoveTime.instantKillSelected)
            {
                
                lineGO = Instantiate(instantKillPrefab);
            }
            else
            {
                lineGO = Instantiate(linePrefab);
            }
            activeLine = lineGO.GetComponent<straightLineDrawing>();
            Vector2 playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(playerPos);
            activeLine = null;
        }

    }
}
