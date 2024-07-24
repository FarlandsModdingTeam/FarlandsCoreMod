using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Loaders
{
    public class TextureLoader
    {
        public static Texture2D LoadMod(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Debug.LogError($"Resource '{resourceName}' not found.");
                    return null;
                }

                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(data);
                return texture;
            }
        }
    }
}
