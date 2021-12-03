using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{

    private Inventory inventory;
    private RectTransform[] paintLocations;
    private Transform paintContainer;
    private Transform paintTemplate;
    public static int selection = 0;
    public Player player;
    public Gauge gauge;

    private void Awake()
    {
        Transform paintLocationsTransform = transform.Find("PaintLocations");
        paintLocations = new RectTransform[6];
        int i = 0;
        foreach (Transform child in paintLocationsTransform)
        {
            paintLocations[i] = (RectTransform) child;
            i++;
        }
        paintContainer = transform.Find("PaintContainer");
        paintTemplate = transform.Find("PaintTemplate");
        
    }

    private void Update()
    {
        if (PauseMenu.isPaused) { return; }
        int oldselection = selection;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selection = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selection = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selection = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selection = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selection = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selection = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selection = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selection = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selection = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            selection -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            selection += 1;
        }

        //paint selection
        List<Item> itemList = inventory.GetPaintList();
        if (itemList.Count != 0)
        {
            selection = (selection + itemList.Count) % 6;
            if (oldselection != selection)
            {
                RefreshInventoryItems();
            }
        }

    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        foreach (Transform child in paintContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Item item in inventory.GetItemList())
        {
            
            RectTransform itemSlotRectTransform = Instantiate(paintTemplate, paintContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            int locNumber = (int) item.itemType;
            int pos = (6 + locNumber - selection) % 6;
            itemSlotRectTransform.anchoredPosition = paintLocations[pos].anchoredPosition;
            if (pos == 0)
            {
                gauge.SetMaxValue(item.maxAmount);
                gauge.SetValue(item.amount);
            }
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            //Image border = itemSlotRectTransform.Find("border").GetComponent<Image>();
            image.sprite = item.GetSprite();
            Image amount = itemSlotRectTransform.Find("amount").GetComponent<Image>();
            amount.fillAmount = Mathf.Clamp(1 - item.amount / item.maxAmount, 0, 1);
            Image color = itemSlotRectTransform.Find("color").GetComponent<Image>();
            color.color = item.GetColor();
            Image amount2 = itemSlotRectTransform.Find("amount (1)").GetComponent<Image>();
            amount2.sprite = item.GetSprite();
            amount2.fillAmount = Mathf.Clamp(1 - item.amount / item.maxAmount, 0, 1);
            /*
            TextMeshProUGUI itemAmountText = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                itemAmountText.SetText(item.amount.ToString());
            }
            else
            {
                itemAmountText.SetText("");
            }
            */
        }
    }
    public static bool IsSelected(Item.ItemType paintType)
    {
        return selection == (int)paintType;
    }
}
