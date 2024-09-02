using Farlands.DataBase;
using Farlands.Inventory;
using JanduSoft;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class FarlandsItems
    {
        public static ScriptableObjectsDB DB => Singleton<ScriptableObjectsDB>.Instance;
        public static int GetLastInventoryItemId() => DB.inventoryItems.Count;

        public static int AddInventoryItem(InventoryItem item)
        {
            var id = GetLastInventoryItemId() + 1;

            item.itemID = id;

            DB.inventoryItems.Add(item);

            return id;
        }
    }
}
