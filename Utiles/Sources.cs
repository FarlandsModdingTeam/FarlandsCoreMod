using Farlands.DataBase;
using Farlands.Inventory;
using Farlands.PlaceableObjectsSystem;
using Farlands.PlantSystem;
using Farlands.WorldResources;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class Source
    {
        public static Object GetObject(string id) => 
            Object.FindObjectFromInstanceID(int.Parse(id));

        public static ScriptableObjectsDB DB;

        public static InventoryItem GetInventory(int id) => DB.GetInventoryItem(id);
        public static InventoryItem GetInventory(string name)
        {
            foreach (var i in DB.inventoryItems)
            { 
                if(i.itemIcon.texture.name == name) return i;
            }

            return null;
        }

        public static PlaceableScriptableObject GetPlaceable(int id) => DB.getPlaceablesData(id);
        public static PlaceableScriptableObject GetPlaceable(string name)
        {
            foreach (var i in DB.placeables)
            {
                if (i.worldSprite.texture.name == name) return i;
            }

            return null;
        }

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

        public static WorldResource GetWorldResource(int id) => DB.getWorldResourceData(id);
        public static WorldResource GetWorldResource(string name)
        {
            foreach (var i in DB.worldResources)
            {
                if (i.resourceSprite.texture.name == name) return i;
            }

            return null;
        }

        public static Texture2D GetTexture(string texture)
        {
            foreach (var i in UnityEngine.Resources.FindObjectsOfTypeAll<Texture2D>())
            {
                if (i.name == texture) return i;
            }

            return null;
        }

        public static void Init()
        {
            DB = GameObject.FindObjectOfType<ScriptableObjectsDB>();
        }

        public static class Replace
        {

            public static void OtherTexture(string id, byte[] raw) => GetTexture(id).LoadImage(raw);
            public static void PlaceableTexture(string id, byte[] raw)
            { 
                var placeable = GetPlaceable(id);
                placeable.worldSprite.texture.LoadImage(raw);
            }

            public static void InventoryTexture(string id, byte[] raw)
            { 
                var inventory = GetInventory(id);
                inventory.itemIcon.texture.LoadImage(raw);
            }

            public static void WorldResourceTexture(string id, byte[] raw)
            {
                var wr = GetWorldResource(id);
                wr.resourceSprite.texture.LoadImage(raw);
            }

            public static void PlantTextue(string id, byte[] raw)
            {
                var plant = GetPlant(id);
                plant.seedSprite.texture.LoadImage(raw);
            }
        }
    }
}
