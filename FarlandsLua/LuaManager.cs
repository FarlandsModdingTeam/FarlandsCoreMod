using BepInEx.Configuration;
using CommandTerminal;
using Farlands;
using Farlands.DataBase;
using Farlands.Dev;
using Farlands.Inventory;
using Farlands.PlantSystem;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.FarlandsLua.Functions;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using HarmonyLib;
using I2.Loc;
using JanduSoft;
using Language.Lua;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace FarlandsCoreMod.FarlandsLua
{
    [Patcher]
    public class LuaManager : IManager
    {
        // ----------------------- DECLARACIONES ----------------------- //
        public static ConfigEntry<bool> UnityDebug;
        public static Dictionary<string, FarlandsEasyMod> EasyMods = new();
        public static Dictionary<string, List<Action>> OnEvents = new();
        public static FarlandsEasyMod CURRENT_MOD;
        public static Script LUA = new();
        public static GameObject _o; // public static, Odio mi vida

        public int Index => 1;

        /// <summary>
        /// name: MOD
        /// especie de getter y setter para la variable global _mod_
        /// </summary>
        public static DynValue MOD
        {
            get => LUA.Globals.Get("_mod_");
            set => LUA.Globals.Set("_mod_", value);
        }

        /// <summary>
        ///     name: ExecuteEvent
        ///     Ejecuta un evento en todos los mods cargados
        /// </summary>
        /// <param name="ev"></param>
        public static void ExecuteEvent(params string[] ev)
        {
            Debug.Log(string.Join('.', ev));

            try
            {
                foreach (var mod in EasyMods.Values)
                {
                    var dyn = mod.Mod.Table.Get("event");
                    foreach (var single in ev)
                    {
                        Debug.Log("mondngo");
                        dyn = dyn.Table.Get(single);
                        if (dyn.Type == DataType.Nil) break;
                    }

                    if (dyn.Type != DataType.Nil)
                    {
                        MOD = mod.Mod; // TODO revisar si funciona
                        LUA.Call(dyn);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
        }

        // Método para inicializar el Manager
        public void Init()
        {
            UnityDebug = FarlandsCoreMod.AddConfig("Debug", "UnityDebug", "If enable Unity logs will be visible in terminal", false);

            FarlandsLua.Functions.LuaFunctions.AddToLua();

            if (!Directory.Exists(Paths.Plugin))
                Directory.CreateDirectory(Paths.Plugin);

            var src = Directory.GetFiles(Paths.Plugin, "*.zip");

            src.ToList().ForEach(FarlandsEasyMod.LoadAndAddZip);
        }

        public static string[] GetFilesInMod(string path)
        {
            var i = path.IndexOf('/');
            var mod = path.Substring(0, i);
            if (mod == ".") mod = MOD.Table.Get("tag").String;
            return EasyMods[mod].GetFilesInFolder(mod, path.Substring(i + 1, path.Length - i - 1));
        }

        // Método para obtener datos de un mod
        public static byte[] GetFromMod(string path)
        {
            var i = path.IndexOf('/');
            var mod = path.Substring(0, i);
            if (mod == ".") mod = MOD.Table.Get("tag").String;
            return EasyMods[mod][path.Substring(i + 1, path.Length - i - 1)];
        }

        // Método para definir funciones auxiliares en LUA
      

        // Método para ejecutar código en LUA
        public static DynValue Execute(byte[] codes, FarlandsEasyMod fem) =>
            Execute(Encoding.UTF8.GetString(codes), fem);

        public static DynValue Execute(string codes, FarlandsEasyMod fem)
        {

            try
            {
                if (fem != null && fem.Tag != null)
                {
                    CURRENT_MOD = fem;
                    MOD = DynValue.NewString(fem.Tag);
                }

                return LUA.DoString(codes);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return DynValue.Nil;
            }

        }

        private static string currentEvent = null;

        [HarmonyPatch(typeof(DebugController), "HandleInput")]
        [HarmonyPrefix]
        public static bool ExecuteInput(DebugController __instance)
        {
            Execute(Private.GetFieldValue<string>(__instance, "input"), null);
            return false;
        }

        

        [HarmonyPatch(typeof(InventorySystem), "EquipItem", [typeof(int)])]
        [HarmonyPostfix]
        public static void EquipPatcher(InventorySystem __instance, int itemIndex)
        {
            if(__instance.equippedClass != null)
            __instance.equippedClass.SetActive(true);
        }

        [HarmonyPatch(typeof(SeedSelector), "EquipSeed", [typeof(int), typeof(PlayerController), typeof(InventorySystem)])]
        [HarmonyPrefix]
        public static bool EquipPatcher(InventorySystem __instance, int itemID, PlayerController playerRef, InventorySystem inventorySystem)
        {
            foreach (EquippableData seedInstance in Private.GetFieldValue<List<EquippableData>>(__instance, "seedInstances"))
            {
                if (itemID == seedInstance.itemID)
                {
                    GameObject gameObject = Object.Instantiate(seedInstance.instance);
                    gameObject.transform.parent = playerRef.transform;
                    gameObject.transform.position = playerRef.transform.TransformPoint(new Vector3(0f, -15f, 0f));
                    
                    if (seedInstance.instance.TryGetComponent<LuaGameObjectComponent>(out var sg))
                    { 
                       gameObject.GetComponent<LuaGameObjectComponent>().Result = sg.Result;
                    }
                    
                    inventorySystem.equippedClass = gameObject;
                }
            }

            return false;
        }

    }
}