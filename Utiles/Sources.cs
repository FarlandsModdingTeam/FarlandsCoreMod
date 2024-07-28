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

        public static void Init()
        {
            DB = GameObject.FindObjectOfType<ScriptableObjectsDB>();
        }


        public static class Replace
        {
            public static void PlaceableTexture(int id, byte[] raw)
            { 
                var placeable = GetPlaceable(id);
                placeable.worldSprite.texture.LoadImage(raw);
            }
        }
    }
}
