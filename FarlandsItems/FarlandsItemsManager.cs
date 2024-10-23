using BepInEx.Configuration;
using Farlands.DataBase;
using Farlands.Inventory;
using Farlands.PlaceableObjectsSystem;
using Farlands.PlantSystem;
using Farlands.WorldResources;
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
    public class FarlandsItemsManager : IManager
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


        #region Inventory Items
        public static InventoryItem GetInventory(int id) => DB.GetInventoryItem(id);
        public static InventoryItem GetInventory(string name)
        {
            foreach (var i in DB.inventoryItems)
            {
                if (i.itemIcon.texture.name == name) return i;
            }

            return null;
        }
        public static int AddInventoryItem(InventoryItem item)
        {
            var id = GetNewItemID();

            item.itemID = id;

            DB.inventoryItems.Add(item);

            return id;
        }
        #endregion

        #region Placeables
        public static PlaceableScriptableObject GetPlaceable(int id) => DB.getPlaceablesData(id);
        public static PlaceableScriptableObject GetPlaceable(string name)
        {
            foreach (var i in DB.placeables)
            {
                if (i.worldSprite.texture.name == name) return i;
            }

            return null;
        }
        #endregion

        #region Plants
        public static PlantScriptableObject GetPlant(int id) => DB.getPlantData(id);
        // TODO cambiar a que vea el nombre de la textura
        public static PlantScriptableObject GetPlant(string name)
        {
            foreach (var i in DB.plants)
            {
                if (i.name == name) return i;
            }

            return null;
        }
        public static int AddPlant(PlantScriptableObject item)
        {
            var id = GetNewPlantId();

            item.ID = id;

            DB.plants.Add(item);

            return id;
        }

        #endregion

        #region WoldResources
        public static WorldResource GetWorldResource(int id) => DB.getWorldResourceData(id);
        public static WorldResource GetWorldResource(string name)
        {
            foreach (var i in DB.worldResources)
            {
                if (i.resourceSprite.texture.name == name) return i;
            }

            return null;
        }
        #endregion

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

            FarlandsEvents.EventsManager.ExecuteEvent("inventory.start");
        }
    }
}
