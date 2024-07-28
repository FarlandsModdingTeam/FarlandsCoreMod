using Farlands.DataBase;
using Farlands.PlaceableObjectsSystem;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class Source
    {
        
        public static Object GetObject(string id) => 
            Object.FindObjectFromInstanceID(int.Parse(id));

        public static ScriptableObjectsDB DB;

        public static PlaceableScriptableObject GetPlaceable(int id) => DB.getPlaceablesData(id);
        public static PlaceableScriptableObject GetPlaceable(string id) => DB.getPlaceablesData(int.Parse(id));

        public static PlaceableScriptableObject GetPlaceable(object id)
        {
            if(id is int) return GetPlaceable((int)id);
            else if(id is string) return GetPlaceable((string)id);
            throw new System.Exception("Invalid ID");
        }

        public static void Init()
        {
            DB = GameObject.FindObjectOfType<ScriptableObjectsDB>();
        }


        public static class Replace
        {
            public static void PlaceableTexture(object id, byte[] raw)
            { 
                var placeable = GetPlaceable(id);
                placeable.worldSprite.texture.LoadImage(raw);
            }
        }
    }
}
