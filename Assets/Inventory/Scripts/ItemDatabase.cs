using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    List<Item> database = new List<Item>();
    JsonData itemData;

    private void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
        Debug.Log(FetchItemById(1).Slug);
    }

    public Item FetchItemById(int id)
    {
        //For performance using FirstOrDefault instead of SingleOrDefault though there should only ever be one item with that id
        var item = database.FirstOrDefault(d => d.Id == id) ?? new Item();
        return item;
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            var item = new Item(
                (int)itemData[i]["id"],
                itemData[i]["title"].ToString(),
                (int)itemData[i]["value"],
                (int)itemData[i]["stats"]["power"],
                (int)itemData[i]["stats"]["defense"],
                (int)itemData[i]["stats"]["vitality"],
                itemData[i]["description"].ToString(),
                (bool)itemData[i]["stackable"],
                (int)itemData[i]["rarity"],
                itemData[i]["slug"].ToString()
                );

            database.Add(item);
        }
    }
}


public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defense { get; set; }
    public int Vitality { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }

    public Item()
    {
        this.Id = -1;
    }

    public Item(int id, string title, int value, int power, int defense, int vitality, string description, bool stackable, int rarity, string slug)
    {
        this.Id = id;
        this.Title = title;
        this.Value = value;
        this.Power = power;
        this.Defense = defense;
        this.Vitality = vitality;
        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Slug = slug;
    }
}