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
    }
}
