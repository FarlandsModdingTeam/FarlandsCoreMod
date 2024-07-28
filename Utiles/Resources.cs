using Farlands.PlaceableObjectsSystem;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class Resources
    {
        public static class Prefabs
        { 
            public static int FENCE_ID = 13490;
            public static Object GetPrefab(int ID) => Object.FindObjectFromInstanceID(ID);
        }
    }
}
