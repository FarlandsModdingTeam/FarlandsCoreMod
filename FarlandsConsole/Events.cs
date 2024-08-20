using FarlandsCoreMod.Attributes;
using HarmonyLib;
using I2.Loc;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.FarlandsConsole
{
    [Patcher]
    public static class Events
    {
        // dialogue.portrair.*
        [HarmonyPatch(typeof(Sequencer), "HandleSetPortraitInternally")]
        [HarmonyPostfix]
        public static void OnSetPortrait(string commandName, string[] args)
        {
            Manager.ExecuteEvent("dialogue", "portrait", "any");
            Manager.ExecuteEvent("dialogue", "portrait", args[1]);
        }

        // farlands.language.change
        [HarmonyPatch(typeof(LanguageSelectionScript), "ChangeLanguage")]
        [HarmonyPostfix]
        public static void OnChangeLanguage()
        {
            Manager.ExecuteEvent("language", "change", "any");
            Manager.ExecuteEvent("language", "change", LocalizationManager.CurrentLanguage);
        }

        [OnLoadScene]
        public static void OnChangeScene(Scene scene)
        {
            Manager.ExecuteEvent("scene", "change", "any");
            Manager.ExecuteEvent("scene", "change", scene.name);
        }
    }
}
