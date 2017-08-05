using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int amount;
    public int slotIndex;

    private Inventory inv;
    private Tooltip tooltip;
    private Vector2 offset;

    private void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        this.transform.position = eventData.position - offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;

        this.transform.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inv.slots[slotIndex].transform);
        this.transform.position = inv.slots[slotIndex].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }
}
