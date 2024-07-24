using UnityEngine.SceneManagement;
using HarmonyLib;
using JanduSoft.Intro;
using FarlandsCoreMod.Attributes;

namespace FarlandsCoreMod.Patchers
{
    
    public class EAScreenQuit
    {
        [OnLoadScene("EarlyAccess")]
        public static void OnLoadScene(Scene scene)
        {
            if(FarlandsCoreMod.Debug_quitEarlyAccessScreen)
                SceneManager.LoadScene("MainMenu");
        }

    }
}