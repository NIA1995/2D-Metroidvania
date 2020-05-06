using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeEnum
{
    Equipment,
    Consumption,
    Etc,
}


public class ItemData
{
    public string ItemName;
    public int ItemValue;
    public int ItemPrice;
    public string ItemDesc;
    public ItemTypeEnum ItemType;
    public Sprite ItemImage;

    public ItemData(string Name, int Value, int Price, string Description, ItemTypeEnum Type, Sprite Image)
    {
        ItemName = Name;
        ItemValue = Value;
        ItemPrice = Price;
        ItemDesc = Description;
        ItemType = Type;
        ItemImage = Image;
    }

    public ItemData()
    {

    }
}
