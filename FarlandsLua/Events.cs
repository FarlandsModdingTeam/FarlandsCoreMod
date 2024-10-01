using FarlandsCoreMod.Attributes;
using HarmonyLib;
using I2.Loc;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.FarlandsLua
{
    [Patcher]
    public static class Events
    {
        // dialogue.portrair.*
        [HarmonyPatch(typeof(Sequencer), "HandleSetPortraitInternally")]
        [HarmonyPostfix]
        public static void OnSetPortrait(string commandName, string[] args)
        {
            LuaManager.ExecuteEvent("dialogue", "portrait", "any");
            LuaManager.ExecuteEvent("dialogue", "portrait", args[1]);
        }

        // farlands.language.change
        [HarmonyPatch(typeof(LanguageSelectionScript), "ChangeLanguage")]
        [HarmonyPostfix]
        public static void OnChangeLanguage()
        {
            LuaManager.ExecuteEvent("language", "change", "any");
            LuaManager.ExecuteEvent("language", "change", LocalizationManager.CurrentLanguage);
        }

        [OnLoadScene]
        public static void OnChangeScene(Scene scene)
        {
            LuaManager.ExecuteEvent("scene", "change", "any");
            LuaManager.ExecuteEvent("scene", "change", scene.name);
        }
    }
}
