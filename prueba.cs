using FarlandsCoreMod.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod
{
    [SceneOverride("MainScene", typeof(Prueba))]
    public static class Prueba
    {
        [SceneOverride.GameObject("Prueba")]
        public static void objetoPrueba(GameObject gameObject)
        {   
            
        }
    }
}
