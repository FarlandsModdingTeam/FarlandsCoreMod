using BepInEx.Logging;
using Farlands;
using Farlands.UI;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.Patchers
{
    [Patcher]
    public class MainMenuButtonPatcher
    {
        [HarmonyPatch(typeof(MainmenuButton), "ExitGame")]
        [HarmonyPrefix]
        static bool ExitGame(MainmenuButton __instance)
        {
            if (FarlandsCoreMod.Debug_wishQuit)
                __instance.OpenWIshlist();

            Application.Quit();
            return false;
        }
    }
}