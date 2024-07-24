using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Loaders
{
    public static class SpriteLoader
    {
        public static Sprite FromTexture(Texture2D texture) => 
            Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));

        public static Sprite LoadMod(string resourceName) => 
            FromTexture(TextureLoader.LoadMod(resourceName));


    }
}
