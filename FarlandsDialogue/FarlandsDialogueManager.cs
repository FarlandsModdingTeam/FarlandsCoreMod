using BepInEx.Configuration;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using I2.Loc;
using I2.Loc.SimpleJSON;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsDialogue
{
    [Patcher]
    public class FarlandsDialogueManager: IManager
    {
        public static bool isSourcesLoaded = false;
        public static bool isDialoguesLoaded = false;

        private static List<SourceJSON> sources = null;
        private static List<SourceJSON> addedSources = new();
        public static ConfigEntry<bool> Config_exportDialogues;

        public int Index => 0;

        public static void AddSourcePreStart(SourceJSON source) => addedSources.Add(source);
        public static void AddSourcePreStartFromBytes(byte[] raw) => AddSourcePreStart(SourceJSON.FromBytes(raw));

        public static void AddSource(SourceJSON source) => source.LoadInMain();
        public static void AddSourceFromBytes(byte[] raw) => AddSource(SourceJSON.FromBytes(raw));

        private static void GetSources() => sources = [.. addedSources];

        public static void LoadSourceFromBytes(byte[] raw)
        {
            var source = SourceJSON.FromBytes(raw);
            FarlandsCoreMod.instance.StartCoroutine(LoadSource(source));
            FarlandsCoreMod.instance.StartCoroutine(LoadData(source));
        }

        public static IEnumerator LoadSource(SourceJSON source)
        {
            yield return new WaitUntil(() => isSourcesLoaded);
            source.LoadInMain();
        }

        public static IEnumerator LoadData(SourceJSON source)
        {
            yield return new WaitUntil(() => isDialoguesLoaded);
            source.LoadInData();
        }

        public void Init()
        {
            Config_exportDialogues = FarlandsCoreMod.AddConfig("FarlandsDialogueMod", "ExportDialogues", "If true, a export file will be created and will save all the dialogues", false);
        }

        public static void AddInventoryTranslation(int id, List<string> names, List<string> descriptions)
        {
            var translation = new SourceJSON();
            
            translation.create.Inventory.Add(id.ToString(), new()
            {
                name = new() { Entry = new() { Languages = names } },
                description = new() { Entry = new() { Languages = descriptions } },
            });

            addedSources.Add(translation);
        }

        public static void export()
        {
            if (Config_exportDialogues.Value)
            {
                var source = SourceJSON.FromFull(LocalizationManager.Sources.First(), DialogueManager.instance.masterDatabase);

                File.WriteAllText(
                    Path.Combine(Paths.Plugin, "dialogue_export.json"),
                    Newtonsoft.Json.JsonConvert.SerializeObject(source)
                );
            }
        }

        [HarmonyPatch(typeof(LocalizationManager), "RegisterSceneSources")]
        [HarmonyPostfix]
        public static void SourcePatch()
        {
            if (isSourcesLoaded) return;
            if (isDialoguesLoaded) export();

            if (sources == null)
                GetSources();

            isSourcesLoaded = true;

            if (sources.Count() < 1) return;

            var mainSource = sources.Select(LoadOneSource).ToList().First();


            mainSource.UpdateDictionary(true);

        }


        // TODO: posible mejora en el siguiente código
        [HarmonyPatch(typeof(DialogueSystemController), "Awake")]
        [HarmonyPostfix]
        public static void DialoguePatch()
        {
            if (isDialoguesLoaded) return;
            if (isSourcesLoaded) export();

            if (sources == null)
                GetSources();

            isDialoguesLoaded = true;

            if (sources.Count() < 1) return;

            var data = sources.Select(LoadOneData).ToList();
        }

        private static LanguageSourceData LoadOneSource(SourceJSON source) => source.LoadInMain();
        private static DialogueDatabase LoadOneData(SourceJSON source) => source.LoadInData();
    }
}
