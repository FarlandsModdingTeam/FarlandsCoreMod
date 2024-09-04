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
        // private Script LUA = new();

        private DynValue updateFunction;
        private DynValue startFunction;
        private DynValue _result;


        public DynValue StartFunction { get => startFunction; set => startFunction = value; }
        public DynValue UpdateFunction { get => updateFunction; set => updateFunction = value; }
        public DynValue Result { get => _result; set => _result = value; }

        // Start
        void Start()
        {
            StartFunction = _result.Table.Get("Start");
            Debug.Log("StartFunction: " + StartFunction);

            //LuaGameObject.FromGameObject(this.gameObject);
            if (StartFunction != null && StartFunction.Type == DataType.Function)
            {
                StartFunction.Function.Call(Result);
            }
        }

        // Update
        void Update()
        {
            UpdateFunction = _result.Table.Get("Update");
            Debug.Log("UpdateFunction: " + UpdateFunction);

            if (UpdateFunction != null && UpdateFunction.Type == DataType.Function)
            {
                UpdateFunction.Function.Call(Result);
            }
        }

    }
}
