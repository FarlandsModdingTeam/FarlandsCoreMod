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
using UnityEngine.Windows;
using static FarlandsCoreMod.Attributes.Configuration;
using static PixelCrushers.DialogueSystem.UnityGUI.GUIProgressBar;
using Input = UnityEngine.Input;

namespace FarlandsCoreMod.FarlandsConsole
{
    [Patcher]
    public class Manager : IManager
    {
        public static ConfigEntry<bool> EnableConsole;
        public static Dictionary<string, FarlandsEasyMod> EasyMods = new();
        public static Dictionary<string, List<Action>> OnEvents = new();
        public static FarlandsEasyMod CURRENT_MOD;
        public static Script LUA = new();
        public static DynValue MOD 
        {
            get => LUA.Globals.Get("_mod_");
            set => LUA.Globals.Set("_mod_", value);
        }
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

        public static byte[] GetFromMod(string path)
        {
            var i = path.IndexOf('/');
            var mod = path.Substring(0, i);
            if (mod == ".") mod = MOD.Table.Get("tag").String;
            return EasyMods[mod][path.Substring(i+1, path.Length-i-1)];
        }
        public static void AuxiliarFunctions()
        {

            LUA.Globals["MOD"] = (string tag) =>
            {
                var code = Encoding.UTF8.GetString(Properties.Resources.init);
                Execute(code.Replace("%%",tag), null);
            };

            LUA.Globals["texture_override"] = (string origin, string path) =>
            {
                Debug.Log(origin);
                Source.Replace.OtherTexture(origin, GetFromMod(path));
            };

            LUA.Globals["portrait_override"] = (string origin, string path) => 
            {
                Debug.Log("PEPINO");
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

        [HarmonyPatch(typeof(DebugController), "Update")]
        [HarmonyPrefix]
        public static bool UpdateConsole(DebugController __instance)
        {
            if(EnableConsole == null || !EnableConsole.Value) return false;

            var Set= (string field, object val) => Private.SetFieldValue(__instance, field, val);
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
                    //ExecuteInput(__instance);
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

        public static DynValue Execute(byte[] codes, FarlandsEasyMod fem) =>
            Execute(Encoding.UTF8.GetString(codes), fem);

        public static DynValue Execute(string codes, FarlandsEasyMod fem)
        {
            if(fem != null && fem.Tag != null)
                MOD=DynValue.NewString(fem.Tag);


            Debug.Log(codes);
            return LUA.DoString(codes);
            // foreach (string code in codes) Execute(code);
        }

        private static string currentEvent = null;

        //[HarmonyPatch(typeof(DebugController), "HandleInput")]
        //[HarmonyPrefix]
        //public static bool ExecuteInput(DebugController __instance)
        //{
        //    Execute(Private.GetFieldValue<string>(__instance, "input"));
        //    return false;
        //}

        //public static void Execute(string code)
        //{
        //    code = code.Trim();

        //    Regex regex = new Regex(@"(?<=^|\s)([^\s']+|'[^']+')");

        //    MatchCollection matches = regex.Matches(code);
        //    List<string> r = new List<string>();
        //    foreach (Match match in matches)
        //    {
        //        // Limpiamos las comillas si es necesario
        //        r.Add(match.Value.Trim('\''));
        //    }

        //    // Convertimos la lista a un array
        //    string[] array = r.ToArray();
        //    r = null;
        //    Action Invoke = null;
        //    List<Action> actionList = new();

            
        //    for (int i = 0; i < Commands.commandList.Count; i++)
        //    {
        //        DebugCommandBase debugCommandBase = Commands.commandList[i] as DebugCommandBase;

        //        if (!code.StartsWith(debugCommandBase.commandId))
        //        {
        //            continue;
        //        }

        //        var end = false;
        //        end = debugCommandBase.commandId == "end";

        //        if (Commands.commandList[i] is DebugCommand)
        //        {
        //            Invoke = (Commands.commandList[i] as DebugCommand).Invoke;
        //        }
        //        else if (Commands.commandList[i] is DebugCommand<int>)
        //        {
        //            Invoke = () => (Commands.commandList[i] as DebugCommand<int>).Invoke(int.Parse(array[1]));
        //        }
        //        else if (Commands.commandList[i] is DebugCommand<int, int>)
        //        {
        //            int result = int.Parse(array[1]);
        //            int result2 = int.Parse(array[2]);
        //            if (int.TryParse(array[1], out result) && int.TryParse(array[2], out result2))
        //            {
        //                Invoke = () => (Commands.commandList[i] as DebugCommand<int, int>).Invoke(result, result2);
        //            }
        //            else
        //            {
        //                Debug.LogWarning("Invalid parameter format for DebugCommand<int, int>.");
        //            }
        //        }
        //        else if (Commands.commandList[i] is DebugCommand<string, string>)
        //        {
        //            Invoke = () => (Commands.commandList[i] as DebugCommand<string, string>).Invoke(array[1], array[2]);
        //        }

        //        if (currentEvent == null || end) Invoke();
        //        else if(Invoke != null) actionList.Add(Invoke);

        //        break;
        //    }

        //    if(currentEvent != null && actionList.Count > 0)
        //    OnEvents[currentEvent].Add(() => actionList.ForEach(x => x.Invoke()));
        //}

    }
}
