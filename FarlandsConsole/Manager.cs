using BepInEx.Configuration;
using Farlands.Dev;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using I2.Loc;
using MoonSharp.Interpreter;using PixelCrushers.DialogueSystem;
using Rewired;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        /*
         * name: MOD
         * especie de getter y setter para la variable global _mod_
         */
        public static DynValue MOD
        {
            get => LUA.Globals.Get("_mod_");
            set => LUA.Globals.Set("_mod_", value);
        }

        public int Index => 1;

        /*
         * name: ExecuteEvent
         * ejecuta un evento en todos los mods cargados
         * 
         */
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

        // Método para inicializar el Manager
        public void Init()
        {
            EnableConsole = FarlandsCoreMod.AddConfig("Debug", "EnableConsole", "", false);

            AuxiliarFunctions();

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
        public static void AuxiliarFunctions()
        {
            LUA.Globals["MOD"] = (string tag) =>
            {
                var code =
@$"
{tag} = {{}}
{tag}.tag = '{tag}'
{tag}.scenes = {{}}
{tag}.event = {{}}
{tag}.event.scene = {{}}
{tag}.event.scene.change = {{}}
{tag}.event.language = {{}}
{tag}.event.language.change = {{}}
{tag}.event.dialogue = {{}}
{tag}.event.dialogue.portrait = {{}}

_mod_ = {tag}";

                Execute(code, null);
                CURRENT_MOD.ConfigFile = new(Path.Combine(Paths.Config, $"{tag}.cfg"), true);
                EasyMods.Add(tag, CURRENT_MOD);
            };
            
            LUA.Globals["config"] = (string section, string key, DynValue def, string description) =>
            {
                var code =
@$"
_mod_.config = _mod_.config or {{}}
_mod_.config.{section} = _mod_.config.{section} or {{}}
";
                Execute(code, null);
                if (def.Type == DataType.Boolean)
                {
                    var entry = CURRENT_MOD.ConfigFile.Bind(section, key, def.Boolean, description);

                    LUA.Globals.Get("_mod_")
                        .Table.Get("config")
                        .Table.Get(section)
                        .Table.Set(key, DynValue.NewCallback((ctx, args) => DynValue.NewBoolean(entry.Value)));
                }
                
            };

            LUA.Globals["load_scene"] = (string scene) =>
            {
                SceneManager.LoadScene(scene);
            };


            //TODO comprobar que funcione
            LUA.Globals["toggle_ui"] = () =>
            {
                var canvas = SceneManager.GetActiveScene().GetRootGameObjects().First(x=>x.name == "Canvas");
                canvas.SetActive(!canvas.activeSelf);
            };
            
            LUA.Globals["texture_override"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count == 0) throw new Exception("Invalid args for TextureOverride");
                else if (args.Count == 1)
                {
                    var path = args[0].String;
                    Source.Replace.OtherTexture(Path.GetFileNameWithoutExtension(path), GetFromMod(path));
                }
                else if (args.Count == 2)
                {
                    var origin = args[0].String;
                    var path = args[1].String;
                    Source.Replace.OtherTexture(origin, GetFromMod(path));
                }

                return DynValue.Void;
            });

            LUA.Globals["texture_override_in"] = DynValue.NewCallback((ctx, args) =>
            {
                var path = args[0].String;
                Debug.Log(string.Join('\n', GetFilesInMod(path)));

                foreach (var item in GetFilesInMod(path))
                    Source.Replace.OtherTexture(Path.GetFileNameWithoutExtension(item), GetFromMod(item));

                return DynValue.Void;
            });

            // No funciona
            LUA.Globals["sprite_override"] = (string origin, int[] position, string path) =>
            {
                var vec = new Vector2Int(position[0], position[1]);
                Source.Replace.ReplaceSprite(origin, vec, GetFromMod(path));
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

            LUA.Globals["add_language"] = (string path) =>
            {
                FarlandsDialogueMod.Manager.AddSourceFromBytes(GetFromMod(path));
            };
            LUA.Globals["get_language"] = () =>
            {
                return LocalizationManager.CurrentLanguage;
            };

            LUA.Globals["find_object"] = (string name) =>
            {
                var go = GameObject.Find(name);
                return LuaGameObject.FromGameObject(go);
            };

            LUA.Globals["create_object"] = (string name) =>
            {
                var go = new GameObject(name);
                return LuaGameObject.FromGameObject(go);
            };

            LUA.Globals["create_scene"] = (string name) =>
            {
                var scene = SceneManager.CreateScene(name);
                //TODO agergar creación del objeto de la escena para lua
            };

            LUA.Globals["show"] = (string txt) =>
            {
                Debug.Log(txt);
            };
        }

        // Método para actualizar la consola de depuración
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

        // Método para dibujar la interfaz de usuario de la consola de depuración
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

        // Método para ejecutar código en LUA
        public static DynValue Execute(byte[] codes, FarlandsEasyMod fem) =>
            Execute(Encoding.UTF8.GetString(codes), fem);

        public static DynValue Execute(string codes, FarlandsEasyMod fem)
        {
            if (fem != null && fem.Tag != null)
            {
                CURRENT_MOD = fem;  
                MOD = DynValue.NewString(fem.Tag);
            }


            Debug.Log(codes);
            return LUA.DoString(codes);
            // foreach (string code in codes) Execute(code);
        }

        private static string currentEvent = null;

        [HarmonyPatch(typeof(DebugController), "HandleInput")]
        [HarmonyPrefix]
        public static bool ExecuteInput(DebugController __instance)
        {
            Execute(Private.GetFieldValue<string>(__instance, "input"), null);
            return false;
        }
    }
}
