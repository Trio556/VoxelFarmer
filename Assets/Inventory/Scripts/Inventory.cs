using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int slotAmount;
    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase db;

    public GameObject inventorySlot;
    public GameObject inventoryItem;
    
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        db = GetComponent<ItemDatabase>();

        slotAmount = 36;
        inventoryPanel = GameObject.Find("InventoryPanel");
        slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
    }
    
    public void AddItem(int id)
    {
        var itemToAdd = db.FetchItemById(id);

        if (itemToAdd.Stackable && CheckIfExists(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == itemToAdd.Id)
                {
                    var data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == -1)
                {
                    items[i] = itemToAdd;
                    var itemObject = Instantiate(inventoryItem);
                    itemObject.GetComponent<ItemData>().item = itemToAdd;
                    itemObject.GetComponent<ItemData>().slotIndex = i;
                    itemObject.transform.SetParent(slots[i].transform);
                    itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObject.transform.position = Vector2.zero;
                    itemObject.name = itemToAdd.Title;
                    slots[i].transform.GetChild(0).GetComponent<ItemData>().amount++;

                    break;
                }
            }
        }
    }

    bool CheckIfExists(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == item.Id)
            {
                return true;
            }
        }

        return false;
    }
}
