using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
using CommandTerminal;
using Farlands.Dev;
using Farlands.PlaceableObjectsSystem;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Patchers;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using HarmonyLib;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

namespace FarlandsCoreMod
{
    [BepInPlugin("top.magincian.fcm", "FarlandsCoreMod", "0.1.3")]
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
            instance = this;

            this.gameObject.AddComponent<Terminal>();

            debug_skipIntro = AddConfig("Debug", "SkipIntro", 
                "If true the intro will be skipped", false);

            debug_quitEarlyAccessScreen = AddConfig("Debug", "QuitEarlyAccessScreen", 
                "If true the Early Access Screen will be removed", false);

            Logger.LogInfo($"Plugin {this.Info.Metadata.GUID} is loaded!");
            
            Patcher.LoadAll();

            OnLoadScene.onLoadScene();

            StartCoroutine(allLoaded());
        }

        private void LoadManagers()
        {
            var managers = Assembly.GetAssembly(this.GetType())
                .GetTypes().Where(x => typeof(IManager).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => (Activator.CreateInstance(x) as IManager))
                .ToList();

            IComparer<IManager> comparer = Comparer<IManager>.Create((x,y)=>x.Index.CompareTo(y.Index));
            managers.Sort(comparer);
            managers.ForEach(m => m.Init());
        }

        private static bool isLoaded = false;
        private IEnumerator allLoaded()
        {
            yield return new WaitForEndOfFrame();

            Source.Init();

            LoadManagers();
            OnAllModsLoaded();

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
