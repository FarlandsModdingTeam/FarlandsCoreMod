using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
using Farlands.PlaceableObjectsSystem;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Patchers;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod
{
    [BepInPlugin("top.magincian.fcm", "FarlandsCoreMod", "0.0.4")]
    public class FarlandsCoreMod : BaseUnityPlugin
    {
        private static ConfigEntry<bool> debug_skipIntro;
        public static bool Debug_skipIntro => debug_skipIntro.Value;

        private static ConfigEntry<bool> debug_quitEarlyAccessScreen;
        public static bool Debug_quitEarlyAccessScreen => debug_quitEarlyAccessScreen.Value;

        public static FarlandsCoreMod instance;

        public string SHORT_NAME => "FCM";

        

        private void Awake()
        {
            
            debug_skipIntro = Config.Bind("Debug", "SkipIntro", false, "If true the intro will be skipped");
            debug_quitEarlyAccessScreen = Config.Bind("Debug", "QuitEarlyAccessScreen", false, "If true the Early Access Screen will be removed");
            // Plugin startup logic
            Logger.LogInfo($"Plugin {this.Info.Metadata.GUID} is loaded!");
            instance = this;


            Patcher.LoadAll();
            
            OnLoadScene.onLoadScene();

            StartCoroutine(allLoaded());
        }

        private void LoadFCMResources()
        {
            //Utiles.Resources.AddCore("bad", TextureLoader.LoadMod("FarlandsCoreMod.Resources.fcm-bad.png"));

            //foreach (var item in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(TMP_FontAsset)))
            //{
            //    var font = item as TMP_FontAsset;
            //    Utiles.Resources.AddBase(font.name, font);
            //}
        }

        private IEnumerator allLoaded()
        { 
            yield return new WaitForEndOfFrame();
            OnAllModsLoaded();
            LoadFCMResources();
        }

        private void OnAllModsLoaded()
        {
            
            Logger.LogMessage("************************");
            var target = "top.magincian.fcm";

            UnityChainloader.Instance.Plugins.Values
                .Where(p => p.Dependencies.Any(d => d.DependencyGUID == target))
                .ToList()
                .ForEach(p =>
                {
                    Logger.LogMessage($"{p.Metadata.GUID}: {p.Metadata.Version}");
                });

            Logger.LogMessage("************************");
        }
    }
}
