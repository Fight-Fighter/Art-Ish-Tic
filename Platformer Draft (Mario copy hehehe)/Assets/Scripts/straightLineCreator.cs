﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class straightLineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject instantKillPrefab;
    public GameObject damagePrefab;
    public GameObject poisonPrefab;
    private Player player;
    private GameObject lineGO;
    straightLineDrawing activeLine;

    private void Awake()
    {
        UI_Inventory.Instance.OnSelectionChanged += Event_SetNull;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null || players.Length == 0) { return; }
        player = players[0].GetComponent<Player>();
    }

    void Update()
    {
        if (PauseMenu.isPaused) { return; }
        if (UI_Inventory.IsSelected(Item.ItemType.FreeformPaint) || UI_Inventory.IsSelected(Item.ItemType.GrapplePaint))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.HasPaint())
        {
            if (UI_Inventory.IsSelected(Item.ItemType.DamagePaint))
            {
                lineGO = Instantiate(damagePrefab);
            }
            else if (UI_Inventory.IsSelected(Item.ItemType.PoisonPaint))
            {
                lineGO = Instantiate(poisonPrefab);
            }
            else if (UI_Inventory.IsSelected(Item.ItemType.KillPaint))
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

    void Event_SetNull(object Sender, System.EventArgs e)
    {
        Debug.Log("event received");
        activeLine = null;
    }
}
