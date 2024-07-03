using FarlandsCoreMod.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod
{
    [SceneOverride("MainMenu", typeof(Prueba))]
    public static class Prueba
    {
        [SceneOverride.GameObject("Canvas")]
        public static GameObject canvas;

        [SceneOverride.NewGameObject("Prueba")]
        public static void objetoPrueba(GameObject gameObject)
        {   
            gameObject.transform.SetParent(canvas.transform);
            var spriter = gameObject.AddComponent<Image>();
            spriter.sprite = Sprite.Create(new Rect(1,1,1,1),Vector2.one/2f,1,Texture2D.whiteTexture);
        }
    }
}
