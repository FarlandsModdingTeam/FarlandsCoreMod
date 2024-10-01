using FarlandsCoreMod.FarlandsLua;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.LightProbeProxyVolume;

namespace FarlandsCoreMod.FarlandsConsole.Functions
{
    public class LuaGameObjectComponent : MonoBehaviour
    {
        // ------------------- Declaraciones ------------------- //
        // private Script LUA = new();

        [SerializeField] private DynValue updateFunction;
        [SerializeField] private DynValue startFunction;
        [SerializeField] private DynValue _result;
        private DynValue _mod;
        public DynValue StartFunction { get => startFunction; set => startFunction = value; }
        public DynValue UpdateFunction { get => updateFunction; set => updateFunction = value; }
        public DynValue Result { get => _result; set => _result = value; }

        // Start
        void Start()
        {
            StartFunction = Result.Table.Get("Start");

            //LuaGameObject.FromGameObject(this.gameObject);
            if (StartFunction != null && StartFunction.Type == DataType.Function)
            {
                RefreshMod();
                StartFunction.Function.Call(Result);
            }
        }

        // Update
        void Update()
        {
            UpdateFunction = Result.Table.Get("Update");

            if (UpdateFunction != null && UpdateFunction.Type == DataType.Function)
            {
                RefreshMod();
                UpdateFunction.Function.Call(Result);
            }
        }

        void RefreshMod() => LuaManager.MOD = Result.Table.Get("_mod_");
    }
}
