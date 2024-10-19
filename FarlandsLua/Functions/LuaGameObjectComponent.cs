using FarlandsCoreMod.FarlandsLua;
using FarlandsCoreMod.FarlandsLua.Functions;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.LightProbeProxyVolume;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    /// <summary>
    ///     Clase de C# que la gracia es permitir ejecutar LUA en un GameObject como si fuera nativo
    /// </summary>
    public class LuaGameObjectComponent : MonoBehaviour
    {
        // ------------------- Declaraciones ------------------- //
        // private Script LUA = new();

        [SerializeField] private DynValue updateFunction;
        [SerializeField] private DynValue startFunction;
        [SerializeField] private DynValue _result;
        private DynValue _mod;

        /// <summary>
        ///   Metodo donde guarda el codigo LUA que se ejecutara con Start de Unity 
        /// </summary>
        public DynValue StartFunction { get => startFunction; set => startFunction = value; }

        /// <summary>
        ///  Metodo donde guarda el codigo LUA que se ejecutara con Update de Unity
        /// </summary>
        public DynValue UpdateFunction { get => updateFunction; set => updateFunction = value; }

        /// <summary>
        ///     Resultado de la ejecucion del script LUA : SUPONGO
        /// </summary>
        public DynValue Result { get => _result; set => _result = value; }

        /// <summary>
        ///     Metodo Start de Unity, 
        /// </summary>
        void Start()
        {
            StartFunction = Result.Table.Get("Start");

            //Result.Table.Set("gameObject", LuaFactory.FromGameObject(this.gameObject)); //UserData.Create(this.gameObject));

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
