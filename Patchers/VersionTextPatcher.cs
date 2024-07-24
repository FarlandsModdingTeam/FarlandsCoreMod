using BepInEx.Logging;
using Farlands;
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
    public class VersionTextPatcher
    {
        [HarmonyPatch(typeof(VersionText), "Start")]
        [HarmonyPostfix]
        static void SetPluginVersion(VersionText __instance)
        {
            if (__instance == null)
            {
                Debug.Log("NULL ERROR");
                return;
            } else Debug.Log("not error");

            var vText = __instance.GetComponent<Text>();

            vText.text = vText.text.Trim();
            vText.text += $"\n{FarlandsCoreMod.instance.SHORT_NAME}: {PluginInfo.PLUGIN_VERSION}";

            __instance.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 10);

            Debug.Log($"{FarlandsCoreMod.instance.SHORT_NAME}: {PluginInfo.PLUGIN_VERSION}");
        }
    }
}