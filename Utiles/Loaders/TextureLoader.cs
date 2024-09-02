using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Loaders
{
    public static class TextureLoader
    {
        public static Texture2D FromRaw(byte[] raw)
        {
            var res = new Texture2D(1, 1);
            res.LoadImage(raw);
            res.filterMode = FilterMode.Point;

            return res;
        }
    }
}
