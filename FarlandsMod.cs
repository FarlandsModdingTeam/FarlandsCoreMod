using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
using FarlandsCoreMod.Attributes;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FarlandsCoreMod
{
    [BepInDependency("top.magincian.fcm", "~0.1.0")]
    public abstract class FarlandsMod : BaseUnityPlugin
    {
        public Assembly ASM => Assembly.GetAssembly(this.GetType());
        public string PLUGIN_PATH => Path.Combine(Paths.Plugin, this.Info.Metadata.Name);
        public string GetPath(string path) => Path.Combine(PLUGIN_PATH, path);

        public abstract string SHORT_NAME { get; }

        private void Awake()
        {
            FarlandsCoreMod.ModList.Add(this);
            ConfigureAll();

            Debug.Log("asm: " + ASM);
            Patcher.LoadAll(ASM);

            OnStart();
            StartCoroutine(allLoaded());
        }
        private IEnumerator allLoaded()
        {
            yield return new WaitUntil(FarlandsCoreMod.IsAllLoaded);
            OnFirstFrame();
            OnLoadScene.onLoadScene(ASM);
        }

        public abstract void OnStart();
        public virtual void OnFirstFrame() { }
        public void ConfigureAll()
        {
            SetPluginDirectory();

            GetType().GetFields().ToList()
                .ForEach(f => 
                {
                    var attributes = f.GetCustomAttributes<Configuration>();
                    if (attributes.Count() > 0)
                    { 
                        var at = attributes.First();

                        if (at is Configuration.Bool)
                        {
                            var c = f.GetCustomAttribute<Configuration.Bool>();

                            f.SetValue(this, Config.Bind(
                                c.section,
                                c.key,
                                c.defaultValue,
                                c.description
                                ));
                        }
                        else if (at is Configuration.Int)
                        {
                            var c = f.GetCustomAttribute<Configuration.Int>();

                            f.SetValue(this, Config.Bind(
                                c.section,
                                c.key,
                                c.defaultValue,
                                c.description
                                ));
                        }
                    }
                });
        }
        public void Configure<T>(ref ConfigEntry<T> configEntry, T defaultValue)
        {
            Debug.Log("name: " + GetFieldName(configEntry));
            var c = this.GetType().GetField(GetFieldName(configEntry), BindingFlags.Public | BindingFlags.Static).GetCustomAttribute<Configuration>();
            
            configEntry = Config.Bind(
                c.section,
                c.key,
                defaultValue,
                c.description
            );
        }

        private string GetFieldName<T>(ConfigEntry<T> configEntry)
        {
            // Obtener el campo al que hace referencia configEntry
            var fields = this.GetType().GetFields( BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(ConfigEntry<T>) && field.GetValue(this) == configEntry)
                {
                    return field.Name;
                }
            }
            throw new ArgumentException("Field not found for the provided configEntry");
        }

        public void SetPluginDirectory() 
        {
            Debug.Log($"getting {PLUGIN_PATH}");
            if (!Directory.Exists(PLUGIN_PATH))
                Directory.CreateDirectory(PLUGIN_PATH);
        }

        public void Write(string path, string text) => File.WriteAllText(
            GetPath(path), text);

        public string Read(string path) => File.ReadAllText(
            GetPath(path));

        public string[] GetFiles(string path) => 
            Directory.GetFiles(GetPath(path))
                .Select(x=> x.Replace(Path.Combine(Paths.Plugin, this.Info.Metadata.Name) + "/", "")).ToArray();
        public string[] GetFiles(string path, string pattern) => 
            Directory.GetFiles(GetPath(path),pattern)
                .Select(x => x.Replace(Path.Combine(Paths.Plugin, this.Info.Metadata.Name)+"/", "")).ToArray();
        public string[] GetFiles(string path, string pattern, SearchOption searchOption) =>
            Directory.GetFiles(GetPath(path), pattern, searchOption)
                .Select(x => x.Replace(Path.Combine(Paths.Plugin, this.Info.Metadata.Name) + "/", "")).ToArray();
    }
}