using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    straightLineDrawing activeLine;
    private bool normal;

    void Start()
    {
        normal = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            normal = true;
        }

        else if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.R))
        {
            normal = false;
        }

        if (!normal)
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
