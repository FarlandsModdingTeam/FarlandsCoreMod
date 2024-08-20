using BepInEx.Configuration;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using I2.Loc;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FarlandsCoreMod.FarlandsDialogueMod
{
    [Patcher]
    public class Manager: IManager
    {
        public static bool isSourcesLoaded = false;
        public static bool isDialoguesLoaded = false;

        private static List<SourceJSON> sources = null;
        private static List<SourceJSON> addedSources = new();
        public static ConfigEntry<bool> Config_exportDialogues;

        public static void AddSource(SourceJSON source) => addedSources.Add(source);

        public void Init()
        {
            Config_exportDialogues = FarlandsCoreMod.AddConfig("FarlandsDialogueMod", "ExportDialogues", "If true, a export file will be created and will save all the dialogues", false);
        }
        public static void AddSourceFromBytes(byte[] raw)
        {
            var json = Encoding.UTF8.GetString(raw);
            AddSource(SourceJSON.FromJson(json));
        }
        private static void GetSources()
        {
            var src = Directory.GetFiles(Paths.Dialogue, "*.source.json", SearchOption.TopDirectoryOnly);
            if (src.Count() < 1) 
            { 
                sources = addedSources;
                return;
            }

            sources = src.Select(SourceJSON.FromFile).ToList();
            sources.AddRange(addedSources);
        }

        public static void export()
        {
            if (Config_exportDialogues.Value)
            {
                var source = SourceJSON.FromFull(LocalizationManager.Sources.First(), DialogueManager.instance.masterDatabase);

                File.WriteAllText(
                    Path.Combine(Paths.Dialogue, "export.json"),
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

            isSourcesLoaded = true;

            if (sources == null)
                GetSources();

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

            isDialoguesLoaded = true;

            if (sources == null)
                GetSources();

            if (sources.Count() < 1) return;

            var data = sources.Select(LoadOneData).ToList();
        }

        private static LanguageSourceData LoadOneSource(SourceJSON source) => source.LoadInMain();
        private static DialogueDatabase LoadOneData(SourceJSON source) => source.LoadInData();
    }
}
