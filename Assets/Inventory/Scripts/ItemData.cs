using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public int amount;

    private Transform originalParent;
    private Vector2 offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        originalParent = this.transform.parent;

        this.transform.SetParent(this.transform.parent.parent);
        this.transform.position = eventData.position - offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;

        this.transform.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(originalParent);
    }
}
