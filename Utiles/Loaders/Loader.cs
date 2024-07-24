using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Loaders
{
    public static class Loader
    {
        //public static AssetBundle LoadMod(string resourceName)
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    using (var stream = assembly.GetManifestResourceStream(resourceName))
        //    {
        //        if (stream == null)
        //        {
        //            Debug.LogError($"Resource '{resourceName}' not found.");
        //            return null;
        //        }

        //        byte[] data = new byte[stream.Length];
        //        stream.Read(data, 0, (int)stream.Length);

        //        return AssetBundle.GetAllLoadedAssetBundles_Native();
        //    }
        //}
    }
}
