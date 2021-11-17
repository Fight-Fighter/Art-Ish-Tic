﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    LineDrawing activeLine;
    private bool free = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            free = true;
        }

        else if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.N))
        {
            free = false;
        }

        if (!free)
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
            Vector2 playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(playerPos);
        }

    }
}
