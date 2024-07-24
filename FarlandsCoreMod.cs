using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
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
    [BepInPlugin("top.magincian.fcm", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class FarlandsCoreMod : BaseUnityPlugin
    {
        private static ConfigEntry<bool> debug_skipIntro;
        public static bool Debug_skipIntro => debug_skipIntro.Value;

        private static ConfigEntry<bool> debug_quitEarlyAccessScreen;
        public static bool Debug_quitEarlyAccessScreen => debug_quitEarlyAccessScreen.Value;

        public static FarlandsCoreMod instance;

        public string SHORT_NAME => "FCM";

        public static class Resources
        {
            private static Dictionary<string, UnityEngine.Object> m_resources = new();

            public static void Add(string name, UnityEngine.Object res) => m_resources.Add(name, res);
            public static UnityEngine.Object Get(string name) => m_resources[name];

            public static void Add(FarlandsMod mod, string name, UnityEngine.Object res) => Add($"{mod.SHORT_NAME}:{name}", res);
            public static UnityEngine.Object Get(FarlandsMod mod, string name) => Get($"{mod.SHORT_NAME}:{name}");

            public static void AddBase(string name, UnityEngine.Object res) => Add($"F:{name}", res);
            public static UnityEngine.Object GetBase(string name) => Get($"F:{name}");

            public static void AddCore(string name, UnityEngine.Object res) => Add($"{instance.SHORT_NAME}:{name}", res);
            public static UnityEngine.Object GetCore(string name) => Get($"{instance.SHORT_NAME}:{name}");

        }

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
            Resources.AddCore("bad", TextureLoader.LoadMod("FarlandsCoreMod.Resources.fcm-bad.png"));
            
            foreach (var item in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(TMP_FontAsset)))
            {
                var font = item as TMP_FontAsset;
                Resources.AddBase(font.name, font);
            }
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
