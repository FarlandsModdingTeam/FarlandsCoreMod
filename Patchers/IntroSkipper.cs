using UnityEngine.SceneManagement;
using HarmonyLib;
using JanduSoft.Intro;
using FarlandsCoreMod.Attributes;

namespace FarlandsCoreMod.Patchers
{
    [Patcher]
    public class IntroSkipper
    {
        [HarmonyPatch(typeof(IntroJanduSoft), "Awake")]
        [HarmonyPrefix]
        public static bool Skip()
        {
            if(!FarlandsCoreMod.Debug_skipIntro) return true;

            SceneManager.LoadScene("PreloadScene");
            return false;
        }

    }
}