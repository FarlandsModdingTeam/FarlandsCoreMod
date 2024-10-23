using Farlands.Events;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.FarlandsLua;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using I2.Loc;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.FarlandsEvents
{
    [Patcher]
    public class EventsManager : IManagerASM
    {
        public static Dictionary<string, List<Action>> onEvents = new();
        private static IEnumerable<Assembly> asms;
        public int Index => 0;

        public void SetASM(IEnumerable<Assembly> asm)
        {
            asms = asm;
        }
        public static void ExecuteEvent(string ev)
        {
            UnityEngine.Debug.Log("event: " + string.Join('.', ev));

            LuaManager.ExecuteEvent(ev);

            if (onEvents.ContainsKey(ev))
                onEvents[ev].ForEach(x => x());
        }
        public void Init()
        {
            asms.ToList().ForEach(OnEvent.LoadAll);
        }

        // dialogue.portrair.*
        [HarmonyPatch(typeof(Sequencer), "HandleSetPortraitInternally")]
        [HarmonyPostfix]
        public static void OnSetPortrait(string commandName, string[] args)
        {
            ExecuteEvent("dialogue.portrait.any");
            ExecuteEvent($"dialogue.portrait.{args[1]}");
        }

        // farlands.language.change
        [HarmonyPatch(typeof(LanguageSelectionScript), "ChangeLanguage")]
        [HarmonyPostfix]
        public static void OnChangeLanguage()
        {
            ExecuteEvent("language.change.any");
            ExecuteEvent($"language.change.{LocalizationManager.CurrentLanguage}");
        }

        [OnLoadScene]
        public static void OnChangeScene(Scene scene)
        {
            ExecuteEvent("scene.change.any");
            ExecuteEvent($"scene.change.{scene.name}");
        }

       
    }
}
