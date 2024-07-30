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

        public static List<SourceJSON> sources = null;

        public static ConfigEntry<bool> Config_exportDialogues;

        public void Init()
        {
            Config_exportDialogues = FarlandsCoreMod.AddConfig("FarlandsCoreMod", "ExportDialogues", "If true, a export file will be created and will save all the dialogues", false);
        }

        private static void GetSources()
        {
            var src = Directory.GetFiles(Paths.Dialogue, "*.source.json", SearchOption.TopDirectoryOnly);
            if (src.Count() < 1) 
            { 
                sources = new();
                return;
            }

            sources = src.Select(SourceJSON.FromFile).ToList();
        }

        public static void export()
        {
            if (Config_exportDialogues.Value)
            {
                var source = SourceJSON.FromFull(LocalizationManager.Sources.First(), DialogueManager.instance.masterDatabase);

                File.WriteAllText(
                    Path.Combine(Paths.Dialogue, "/export.json"),
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
