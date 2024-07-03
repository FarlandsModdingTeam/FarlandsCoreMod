using FarlandsCoreMod.Attributes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace FarlandsCoreMod.Patchers
{
    [Patcher]
    public class EnableDebug
    {
        [HarmonyPatch(typeof(Debug), "isDebugBuild", MethodType.Getter)]
        [HarmonyPrefix]
        public static bool debug(ref bool __result)
        {
            if (!FarlandsCoreMod.Debug_fakeDebugBuild) return true;
            DialogueDebug.level = DialogueDebug.DebugLevel.Info;

            __result = true;
            return false;
        }
    }
}
