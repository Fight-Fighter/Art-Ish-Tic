using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    public Inventory()
    {
        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.ItemType.DamagePaint, amount = 1000f });
        AddItem(new Item { itemType = Item.ItemType.NormalPaint, amount = 1000f });
        AddItem(new Item { itemType = Item.ItemType.FreeformPaint, amount = 1000f });
        AddItem(new Item { itemType = Item.ItemType.KillPaint, amount = 1000f });
        AddItem(new Item { itemType = Item.ItemType.PoisonPaint, amount = 1000f });
        AddItem(new Item { itemType = Item.ItemType.GrapplePaint, amount = 1000f });
        Debug.Log("Inventory Initialized");
        Debug.Log(itemList.Count);
    }
    public bool AddItem(Item item)
    {
        if (item.IsStackable())
        {
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    OnItemListChanged?.Invoke(this, EventArgs.Empty);
                    return true;
                }
            }
            itemList.Add(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            itemList.Add(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
       
    }

    public void RemoveItem(Item item)
    {
        bool alreadyInInventory = false;
        Item toDelete = null;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                inventoryItem.amount -= item.amount;
                toDelete = inventoryItem;
                alreadyInInventory = true;
                break;
            }
        }
        if (alreadyInInventory && toDelete.amount <= 0)
        {
            //itemList.Remove(toDelete);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool HasItem(Item.ItemType itemType)
    {
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == itemType)
            {
                return inventoryItem.amount > 0;
            }
        }
        return false;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public List<Item> GetPaintList()
    {
        return itemList.FindAll(item => item.IsPaint());
    }
}
