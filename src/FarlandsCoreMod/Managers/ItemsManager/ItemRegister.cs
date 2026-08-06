using System.Collections.Generic;
using Farlands.Inventory;
using FarlandsCoreMod.Core.Language;
using I2.Loc;

namespace FarlandsCoreMod.Core.Items;

public class ItemRegister
{
    public class Entry
    {
        public Item Item;
        public string[] Langs;

        public Entry(Item item, string[] langs)
        {
            Item = item;
            Langs = langs;
        }
    }

    private const int FARLANDS_OFFSET = 5_000;
    private int addedItems = 0;

    public Dictionary<string, Dictionary<int, Entry>> IdMap = new();

    public Entry Register(string guid, Entry entry)
    {
        if (!IdMap.ContainsKey(guid))
            IdMap.Add(guid, new());

        // TODO: Already loaded case

        if (entry.Item.LocalId < 0)
            entry.Item.LocalId = GetFirtFreeLocalId(guid);

        entry.Item.RealId = FARLANDS_OFFSET + addedItems;
        IdMap[guid].Add(entry.Item.LocalId, entry);
        return entry;
    }

    public int GetFirtFreeLocalId(string guid)
    {
        var keys = new List<int>(IdMap[guid].Keys);
        keys.Sort();

        var lastId = -1;
        foreach (var id in keys)
        {
            if (id - lastId > 1)
                return lastId + 1;

            lastId = id;
        }

        return lastId + 1;
    }

    public void InsertIntoDB(List<InventoryItem> list, LanguageManager lang)
    {
        foreach (var p in IdMap)
            foreach (var v in p.Value.Values)
            {
                list.Add(v.Item.ToInventoryItem());
                lang.Register(p.Key,
                    new TermData()
                    {
                        Term = $"Inventory/item_name_{v.Item.RealId}",
                        TermType = eTermType.Text,
                        Languages = v.Langs
                    });
            }
    }
}
