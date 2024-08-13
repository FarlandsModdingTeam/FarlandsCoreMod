using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using JetBrains.Annotations;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.FarlandsTextureMod
{
    [Patcher]
    public class Manager : IManager
    {
        static string zipFilePath;
        public void Init() => LoadAllTextures();
        
        //[HarmonyPatch(typeof(DialogueSystemController), "Awake")]
        //[HarmonyPostfix]
        public static void LoadDialoguesTextures()
        {
            LoadTextures("Dialogue", Source.Replace.DialogueTexture);
        }
        public static void LoadAllTextures()
        {
            if (!Directory.Exists(Paths.Texture))
                Directory.CreateDirectory(Paths.Texture);

            var src = Directory.GetFiles(Paths.Texture, "*.zip");

            foreach (var item in src)
                LoadTexturesFrom(item);
        }

        public static void LoadTexturesFrom(string zfp)
        {
            zipFilePath = zfp;
            LoadTextures("Inventory", Source.Replace.InventoryTexture);
            LoadTextures("Placeable", Source.Replace.PlaceableTexture);
            LoadTextures("Plant", Source.Replace.PlantTextue);
            LoadTextures("World", Source.Replace.WorldResourceTexture);
            LoadTextures("Other", Source.Replace.OtherTexture);
        }

        private static void LoadTextures(string subdir, Action<string, byte[]> load)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                var entriesInSubdirectory = archive.Entries
                .Where(entry => entry.FullName.StartsWith(subdir + "/", StringComparison.OrdinalIgnoreCase))
                .ToList();

                foreach (ZipArchiveEntry entry in entriesInSubdirectory)
                {
                    if (Path.GetExtension(entry.FullName) != ".png") continue;

                    Console.WriteLine("Nombre del archivo en el subdirectorio: " + entry.FullName);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (Stream zipStream = entry.Open())
                        {
                            zipStream.CopyTo(memoryStream);
                        }

                        load(Path.GetFileNameWithoutExtension(entry.FullName), memoryStream.ToArray());
                    }
                }
            }
        }

        
    }
}
