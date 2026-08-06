using Farlands.Inventory;
using UnityEngine;

namespace FarlandsCoreMod.Core.Items;

public class Item
{
    public string Name;
    public InventoryItem.ItemType Type;
    public int Price;
    public int SellPrice;
    public bool CanBeStacked;
    public bool CanBeDestroyed;
    public float MatterPercent;

    public int LocalId;
    public int RealId;

    public Item(
            string name,
            InventoryItem.ItemType type,
            int price,
            int sellPrice,
            bool canBeStacked,
            bool canBeDestroyed,
            float matterPercent)
    {
        Name = name;
        Type = type;
        Price = price;
        SellPrice = sellPrice;
        CanBeStacked = canBeStacked;
        CanBeDestroyed = canBeDestroyed;
        MatterPercent = matterPercent;
    }

    public InventoryItem ToInventoryItem()
    {
        var item = ScriptableObject.CreateInstance<InventoryItem>();
        item.itemID = RealId;
        item.itemName = Name;
        item.itemType = Type;
        item.itemPrice = Price;
        item.itemSellPrice = SellPrice;
        item.canBeStacked = CanBeStacked;
        item.canBeDestroyed = CanBeDestroyed;
        item.matterPercent = MatterPercent;
        return item;
    }
}

