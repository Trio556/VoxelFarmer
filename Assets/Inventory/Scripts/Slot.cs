using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public int id;
    private Inventory inv;

    private void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (inv.items[id].Id == -1)
        {
            inv.items[droppedItem.slotIndex] = new Item();
            inv.items[id] = droppedItem.item;
            droppedItem.slotIndex = id;
        }
        else if (droppedItem.slotIndex != id)
        {
            var item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slotIndex = droppedItem.slotIndex;
            item.transform.SetParent(inv.slots[droppedItem.slotIndex].transform);
            item.transform.position = inv.slots[droppedItem.slotIndex].transform.position;
            droppedItem.slotIndex = id;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            inv.items[droppedItem.slotIndex] = item.GetComponent<ItemData>().item;
            inv.items[id] = droppedItem.item;
        }
    }
}
