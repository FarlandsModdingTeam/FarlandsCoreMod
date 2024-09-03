using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsConsole
{
    internal class scriptGenerico : MonoBehaviour
    {
        // ------------------- Declaraciones ------------------- //
        private Script LUA = new();

        private static DynValue updateFunction;
        private static DynValue startFunction;



        // Start
        void Start()
        {
            //LuaGameObject.FromGameObject(this.gameObject);
            if (updateFunction != null && updateFunction.Type == DataType.Function)
            {
                updateFunction.Function.Call();
            }
        }

        // Update
        void Update()
        {
            if (startFunction != null && startFunction.Type == DataType.Function)
            {
                startFunction.Function.Call();
            }
        }

    }
}
