using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Patchers;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod
{
    [BepInPlugin("top.magincian.fcm", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class FarlandsCoreMod : BaseUnityPlugin
    {
        private static ConfigEntry<bool> debug_skipIntro;
        private static ConfigEntry<bool> debug_wishQuit;
        private static ConfigEntry<bool> debug_fakeDebugBuild;
        public static bool Debug_skipIntro => debug_skipIntro.Value;
        public static bool Debug_wishQuit => debug_wishQuit.Value;
        public static bool Debug_fakeDebugBuild => debug_fakeDebugBuild.Value;

        public static FarlandsCoreMod instance;

        public const string SHORT_NAME = "FCM";
        private void Awake()
        {
            
            debug_skipIntro = Config.Bind("Debug", "SkipIntro", false, "If true the intro will be skipped");
            debug_wishQuit = Config.Bind("Debug", "WishQuit", true, "If true, you will be redirected to steam page");
            debug_fakeDebugBuild = Config.Bind("Debug", "FakeDebugBuild", false, "If true, the debug build is active");
            // Plugin startup logic
            Logger.LogInfo($"Plugin {this.Info.Metadata.GUID} is loaded!");
            instance = this;

            Patcher.LoadAll();
            OnLoadScene.onLoadScene();

            StartCoroutine(allLoaded());
        }

        private IEnumerator allLoaded()
        { 
            yield return new WaitForEndOfFrame();
            OnAllModsLoaded();
        }

        private void OnAllModsLoaded()
        {
            
            Logger.LogMessage("************************");
            Logger.LogMessage($"FCM v{PluginInfo.PLUGIN_VERSION}:");
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
