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
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod
{
    [BepInPlugin("top.magincian.fcm", "FarlandsCoreMod", "0.0.6")]
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
            debug_skipIntro = AddConfig("Debug", "SkipIntro", 
                "If true the intro will be skipped", false);

            debug_quitEarlyAccessScreen = AddConfig("Debug", "QuitEarlyAccessScreen", 
                "If true the Early Access Screen will be removed", false);
            
            Logger.LogInfo($"Plugin {this.Info.Metadata.GUID} is loaded!");
            instance = this;


            Patcher.LoadAll();

            OnLoadScene.onLoadScene();

            StartCoroutine(allLoaded());
        }

        private void LoadManagers()
        {

            new FarlandsTextureMod.Manager().Init();
            new FarlandsDialogueMod.Manager().Init();
        }

        private static bool isLoaded = false;
        private IEnumerator allLoaded()
        { 
            yield return new WaitForEndOfFrame();
            Source.Init();
            OnAllModsLoaded();
            LoadManagers();

            isLoaded = true;
        }

        public static bool IsAllLoaded() => isLoaded;
        public static ConfigEntry<T> AddConfig<T>(string section, string key, string description, T defaultValue) =>
            instance.Config.Bind(section, key, defaultValue, description);
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
