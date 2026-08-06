using HarmonyLib;
using Farlands;
using UnityEngine.UI;
using UnityEngine;

namespace FarlandsCoreMod.Core.Version;

[HarmonyPatch(typeof(VersionText))]
public static class VersionTextPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPrefix]
    public static bool StartPrefix(VersionText __instance)
    {
        var versionText = __instance.GetComponent<Text>();
        var title0 = "Farlands";
        var title1 = "FCM";

        var version0 = Application.version;
        var version1 = FCMInfo.PLUGIN_VERSION;

        var space0 = 3;
        var space1 = title0.Length + space0 + version0.Length - title1.Length - version1.Length;

        versionText.text = $"{title0}{new string(' ', space0)}{version0}\n{title1}{new string(' ', space1)}{version1}";
        return false;
    }
}
