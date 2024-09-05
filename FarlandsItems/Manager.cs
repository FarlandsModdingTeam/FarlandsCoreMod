using BepInEx.Configuration;
using Farlands.DataBase;
using Farlands.Inventory;
using Farlands.PlantSystem;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using JanduSoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsItems
{
    [Patcher]
    public class Manager : IManager
    {
        public int Index => 0;
        public static List<SeedData> seeds = new();
        public static ConfigEntry<int> FirstID;

        public static ScriptableObjectsDB DB => Singleton<ScriptableObjectsDB>.Instance;
        private static int GetNewItemID() => Math.Max(DB.inventoryItems.Select(x => x.itemID).Max() + 1, FirstID.Value);
        private static int GetNewPlantId() => Math.Max(DB.plants.Select(x => x.ID).Max() + 1, FirstID.Value);

        public void Init()
        {
            FirstID = FarlandsCoreMod.AddConfig("FarlandsItems", "FirstID", "The first id for mod objects", 2000);
        }

        public static int AddInventoryItem(InventoryItem item)
        {
            var id = GetNewItemID();

            item.itemID = id;

            DB.inventoryItems.Add(item);

            return id;
        }
        public static int AddPlant(PlantScriptableObject item)
        {
            var id = GetNewPlantId();

            item.ID = id;

            DB.plants.Add(item);

            return id;
        }

        public class SeedData
        { 
            public int ItemId;
            public List<int> PlantsId;
        }


        [HarmonyPatch(typeof(InventorySystem), "Start")]
        [HarmonyPostfix]
        public static void OnStart(InventorySystem __instance)
        {
            var ss = __instance.GetComponent<SeedSelector>();
            var originalSeeds = Private.GetFieldValue<List<EquippableData>>(ss, "seedInstances");

            foreach (var s in seeds)
            {
                var seeds = Private.GetFieldValue<List<EquippableData>>(GameObject.FindObjectOfType<SeedSelector>(), "seedInstances");
                var instance = GameObject.Instantiate(seeds.First().instance);
                instance.SetActive(false);
                var seedScript = instance.GetComponent<SeedScript>();
                seedScript.plantsID = s.PlantsId;
                seedScript.ColdplantsID = new();
                seedScript.HotplantsID = new();
                seedScript.isRandom = false;

                originalSeeds.Add(new EquippableData()
                {
                    itemID = s.ItemId,
                    instance = instance,
                });
            }

            FarlandsConsole.Manager.ExecuteEvent("inventory", "start");
        }
    }
}
