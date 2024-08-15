using BepInEx.Configuration;
using Farlands.Dev;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using MoonSharp.Interpreter;
using Rewired;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static FarlandsCoreMod.Attributes.Configuration;
using static PixelCrushers.DialogueSystem.UnityGUI.GUIProgressBar;
using Input = UnityEngine.Input;

namespace FarlandsCoreMod.FarlandsConsole
{
    [Patcher]
    public class Manager : IManager
    {
        // ----------------------- DECLARACIONES ----------------------- //
        public static ConfigEntry<bool> EnableConsole;
        public static Dictionary<string, FarlandsEasyMod> EasyMods = new();
        public static Dictionary<string, List<Action>> OnEvents = new();
        public static FarlandsEasyMod CURRENT_MOD;
        public static Script LUA = new();


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

        /// <summary>
        ///     Método para inicializar el Manager
        /// </summary>
        public void Init()
        {
            EnableConsole = FarlandsCoreMod.AddConfig("Debug", "EnableConsole", "", false);

            AuxiliarFunctions();

            if (!Directory.Exists(Paths.Plugin))
                Directory.CreateDirectory(Paths.Plugin);

            var src = Directory.GetFiles(Paths.Plugin, "*.zip");

            foreach (var item in src)
            {
                var fem = FarlandsEasyMod.FromZip(item);
                fem.ExecuteMain();
                EasyMods.Add(fem.Tag, fem);
            }

        }

        /// <summary>
        ///     Método para obtener datos de un mod
        /// </summary>
        /// <param name="path"></param>
        /// <returns>EasyMods[mod][path.Substring(i + 1, path.Length - i - 1)];</returns>
        public static byte[] GetFromMod(string path)
        {
            var i = path.IndexOf('/');
            var mod = path.Substring(0, i);
            if (mod == ".") mod = MOD.Table.Get("tag").String;
            return EasyMods[mod][path.Substring(i + 1, path.Length - i - 1)];
        }

        // Método para definir funciones auxiliares en LUA
        public static void AuxiliarFunctions()
        {

            LUA.Globals["MOD"] = (string tag) =>
            {
                var code =
@$"
{tag} = {{}}
{tag}.tag = '{tag}'
{tag}.event = {{}}
{tag}.event.dialogue = {{}}
{tag}.event.dialogue.portrait = {{}}

_mod_ = {tag}";

                Execute(code, null);
            };

            LUA.Globals["load_scene"] = (string scene) =>
            {
                SceneManager.LoadScene(scene);
            };

            LUA.Globals["texture_override"] = (string origin, string path) =>
            {
                Source.Replace.OtherTexture(origin, GetFromMod(path));
            };

            LUA.Globals["portrait_override"] = (string origin, string path) =>
            {
                string code =
@$"
    function _mod_.event.dialogue.portrait:{origin}()
        texture_override('{origin}', '{path}')
    end
    ";
                LUA.DoString(code);
            };

            LUA.Globals["show"] = (string txt) =>
            {
                Debug.Log(txt);
            };
        }

        /// <summary>
        ///     Método para actualizar la consola de depuración
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns>false</returns>
        [HarmonyPatch(typeof(DebugController), "Update")]
        [HarmonyPrefix]
        public static bool UpdateConsole(DebugController __instance)
        {
            if (EnableConsole == null || !EnableConsole.Value) return false;

            var Set = (string field, object val) => Private.SetFieldValue(__instance, field, val);
            var Get = (string field) => Private.GetFieldValue(__instance, field);
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                Set("showConsole", !(bool)Get("showConsole"));

                if ((bool)Get("showConsole"))
                {
                    Set("consoleActive", true);

                    __instance.player.controllers.maps.SetAllMapsEnabled(state: false);
                    ReInput.players.GetSystemPlayer().controllers.maps.SetAllMapsEnabled(state: false);
                    Set("inputFocused", true);
                }
                else
                {
                    Set("consoleActive", false);
                    __instance.player.controllers.maps.SetAllMapsEnabled(state: true);
                    ReInput.players.GetSystemPlayer().controllers.maps.SetAllMapsEnabled(state: true);
                    Set("inputFocused", false);
                }
            }

            return false;
        }

        /// <summary>
        ///     Método para dibujar la interfaz de usuario de la consola de depuración
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns>false</returns>
        [HarmonyPatch(typeof(DebugController), "OnGUI")]
        [HarmonyPrefix]
        public static bool OnGui(DebugController __instance)
        {
            if (EnableConsole == null || !EnableConsole.Value) return false;

            var Set = (string field, object val) => Private.SetFieldValue(__instance, field, val);
            var Get = (string field) => Private.GetFieldValue(__instance, field);

            if (!(bool)Get("showConsole"))
                return false;

            float num = 0f;
            GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
            gUIStyle.fontSize = 28;
            GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
            gUIStyle2.fontSize = 24;
            //if (showHelp)
            //{
            //    GUI.Box(new Rect(0f, num, Screen.width, 200f), "");
            //    Rect viewRect = new Rect(0f, 0f, Screen.width - 30, 20 * commandList.Count);
            //    Vector2 zero = Vector2.zero;
            //    zero = GUI.BeginScrollView(new Rect(0f, num + 5f, Screen.width, 190f), zero, viewRect);
            //    for (int i = 0; i < commandList.Count; i++)
            //    {
            //        DebugCommandBase debugCommandBase = commandList[i] as DebugCommandBase;
            //        string text = debugCommandBase.commandFormat + " - " + debugCommandBase.commandDescription;
            //        GUI.Label(new Rect(5f, 20 * i, viewRect.width - 100f, 120f), text, gUIStyle2);
            //    }

            //    GUI.EndScrollView();
            //    num += 200f;
            //}

            GUI.Box(new Rect(0f, num, Screen.width, 52f), "");
            GUI.backgroundColor = Color.black;
            if ((bool)Get("inputFocused"))
            {
                GUI.SetNextControlName("InputTextField");
                GUI.FocusControl("InputTextField");
                Set("input", GUI.TextField(new Rect(10f, num + 5f, (float)Screen.width - 20f, 100f), (string)Get("input"), gUIStyle));
                if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
                {
                    GUI.FocusControl(null);
                    ExecuteInput(__instance);
                    Set("input", "");

                    Set("showConsole", false);
                    Set("consoleActive", false);
                    __instance.player.controllers.maps.SetAllMapsEnabled(state: true);
                    ReInput.players.GetSystemPlayer().controllers.maps.SetAllMapsEnabled(state: true);
                    Set("inputFocused", false);
                }
            }

            return false;
        }

        /// <summary>
        ///     Método para ejecutar código en LUA
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="fem"></param>
        public static DynValue Execute(byte[] codes, FarlandsEasyMod fem) =>
            Execute(Encoding.UTF8.GetString(codes), fem);

        /// <summary>
        ///    Método para ejecutar código generico en LUA
        ///    comprueba si el mod es nulo
        ///    
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="fem"></param>
        /// <returns>LUA.DoString(codes)</returns>
        public static DynValue Execute(string codes, FarlandsEasyMod fem)
        {
            if (fem != null && fem.Tag != null)
                MOD = DynValue.NewString(fem.Tag);


            Debug.Log(codes);
            return LUA.DoString(codes);
            // foreach (string code in codes) Execute(code);
        }

        private static string currentEvent = null;

        /// <summary>
        ///    Método para ejecutar un evento en LUA
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns>false</returns>
        [HarmonyPatch(typeof(DebugController), "HandleInput")]
        [HarmonyPrefix]
        public static bool ExecuteInput(DebugController __instance)
        {
            Execute(Private.GetFieldValue<string>(__instance, "input"), null);
            return false;
        }
    }
}
