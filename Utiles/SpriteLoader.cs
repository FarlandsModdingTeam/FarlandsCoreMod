using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public class SpriteLoader
    {
        public static Sprite LoadMod(string resourceName)
        {
            var texture = TextureLoader.LoadMod(resourceName);
            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }
            
    }
}
