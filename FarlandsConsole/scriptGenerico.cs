using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsConsole
{
    internal class scriptGenerico : MonoBehaviour
    {
        // Start
        void Start()
        {
            //LuaGameObject.FromGameObject(this.gameObject);
            LuaGameObject.Start();
        }

        // Update
        void Update()
        {
            LuaGameObject.Update();
        }

    }
}
