using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject instantKillPrefab;
    public GameObject damagePrefab;
    public GameObject poisonPrefab;
    straightLineDrawing activeLine;

    void Update()
    {

        if (!MoveTime.normalSelected)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject lineGO = Instantiate(linePrefab);
            activeLine = lineGO.GetComponent<straightLineDrawing>();
            Vector2 playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(playerPos);
            activeLine = null;
        }

    }
}
