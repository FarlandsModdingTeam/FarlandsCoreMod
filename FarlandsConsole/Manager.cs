using BepInEx.Configuration;
using Farlands.Dev;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using Rewired;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;
using static FarlandsCoreMod.Attributes.Configuration;
using Input = UnityEngine.Input;

namespace FarlandsCoreMod.FarlandsConsole
{
    [Patcher]
    public class Manager : IManager
    {
        public static ConfigEntry<bool> EnableConsole;
        public static Dictionary<string, FarlandsEasyMod> EasyMods = new();
        public static Dictionary<string, List<Action>> OnEvents = new();
        public static void ExecuteEvent(string ev)
        {
            if (!OnEvents.ContainsKey(ev)) return;
            OnEvents[ev].ForEach(x => x.Invoke());
        }
        public void Init()
        {
            EnableConsole = FarlandsCoreMod.AddConfig("Debug", "EnableConsole", "", false);
            var dcontrol = GameObject.FindObjectOfType<DebugController>();
            Commands.commandList.AddRange(dcontrol.commandList);

            if (!Directory.Exists(Paths.Plugin))
                Directory.CreateDirectory(Paths.Plugin);

            var src = Directory.GetFiles(Paths.Plugin, "*.zip");

            foreach (var item in src)
            {
                var fem = FarlandsEasyMod.FromZip(item);
                fem.ExecuteMain();
                EasyMods.Add(item,fem);
            }
                
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

            if (Input.GetKeyDown(KeyCode.Return) && (bool)Get("showConsole"))
            {
                Private.InvokeMethod(__instance, "HandleInput");
                Set("input", "");
            }

            return false;
        }

        public static void Execute(string[] codes, FarlandsEasyMod fem)
        { 
            Commands.MOD = fem;

            foreach (string code in codes) Execute(code);
        }

        private static string currentEvent = null;
        public static void Execute(string code)
        {
            code = code.Trim();

            Regex regex = new Regex(@"(?<=^|\s)([^\s']+|'[^']+')");

            MatchCollection matches = regex.Matches(code);
            List<string> r = new List<string>();
            foreach (Match match in matches)
            {
                // Limpiamos las comillas si es necesario
                r.Add(match.Value.Trim('\''));
            }

            // Convertimos la lista a un array
            string[] array = r.ToArray();
            r = null;
            Action Invoke = null;
            List<Action> actionList = new();

            
            for (int i = 0; i < Commands.commandList.Count; i++)
            {
                DebugCommandBase debugCommandBase = Commands.commandList[i] as DebugCommandBase;

                if (!code.StartsWith(debugCommandBase.commandId))
                {
                    continue;
                }

                var end = false;
                end = debugCommandBase.commandId == "end";

                if (Commands.commandList[i] is DebugCommand)
                {
                    Invoke = (Commands.commandList[i] as DebugCommand).Invoke;
                }
                else if (Commands.commandList[i] is DebugCommand<int>)
                {
                    Invoke = () => (Commands.commandList[i] as DebugCommand<int>).Invoke(int.Parse(array[1]));
                }
                else if (Commands.commandList[i] is DebugCommand<int, int>)
                {
                    int result = int.Parse(array[1]);
                    int result2 = int.Parse(array[2]);
                    if (int.TryParse(array[1], out result) && int.TryParse(array[2], out result2))
                    {
                        Invoke = () => (Commands.commandList[i] as DebugCommand<int, int>).Invoke(result, result2);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid parameter format for DebugCommand<int, int>.");
                    }
                }
                else if (Commands.commandList[i] is DebugCommand<string, string>)
                {
                    Invoke = () => (Commands.commandList[i] as DebugCommand<string, string>).Invoke(array[1], array[2]);
                }

                if (currentEvent == null || end) Invoke();
                else if(Invoke != null) actionList.Add(Invoke);

                break;
            }

            if(currentEvent != null && actionList.Count > 0)
            OnEvents[currentEvent].Add(() => actionList.ForEach(x => x.Invoke()));
        }

        public static class Commands
        {
            public static FarlandsEasyMod MOD;

            public static DebugCommand<string, string> TEXTURE_OVERRIDE = new("texture_override", "", "",
                (string origin, string path) => 
                {
                    Source.Replace.OtherTexture(origin, MOD[path]);
                });

            public static DebugCommand<string, string> PORTRAIT_OVERRIDE = new("portrait_override", "", "",
                (string origin, string path) =>
                {
                    var key = $"dialogue.portrait.{origin}";
                    if (!OnEvents.ContainsKey(key)) OnEvents.Add(key, new());
                    OnEvents[key].Add(() => TEXTURE_OVERRIDE.Invoke(origin, path));
                });

            public static DebugCommand<string, string> WHEN = new("when", "", "",
                (string ev, string mode) =>
                {
                    if (mode == "then")
                    { 
                        currentEvent = ev;
                        OnEvents.Add(ev, []);
                    }
                });

            public static DebugCommand END = new("end", "", "",
                () =>
                {
                    currentEvent = null;
                });

            public static List<object> commandList = [TEXTURE_OVERRIDE, WHEN, END, PORTRAIT_OVERRIDE];

        }
    }
}
