using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Item _item;
    private string data;
    private GameObject toolTip;

    private void Start()
    {
        toolTip = GameObject.Find("Tooltip");
        toolTip.SetActive(false);
    }

    private void Update()
    {
        if (toolTip.activeSelf)
        {
            toolTip.transform.position = Input.mousePosition;
        }
    }

    public void Activate(Item item)
    {
        _item = item;
        ConstructDataString();
        toolTip.SetActive(true);
        
    }

    public void Deactivate()
    {
        toolTip.SetActive(false);
    }

    public void ConstructDataString()
    {
        data = "<color=#0473f0><b>" +  _item.Title + "</b></color>\n" + _item.Description + "";
        toolTip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
}

