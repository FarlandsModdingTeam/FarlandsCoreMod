using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace FarlandsCoreMod.Utiles.Loaders
{
    public static class SpriteLoader
    {
        public static Sprite FromTexture(Texture2D texture)
        {
            Debug.Log($"({texture.width},{texture.height})");

            var sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                1);

            return sprite;
        }

        public static Sprite FromRaw(byte[] raw) => FromTexture(TextureLoader.FromRaw(raw));
    }
}
