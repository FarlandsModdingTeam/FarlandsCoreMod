using FarlandsCoreMod.Utiles;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace FarlandsCoreMod.FarlandsTextureMod
{
    public static class FarlandsTextureMod
    {
        static string zipFilePath;

        public static void LoadAllTextures()
        {
            if (!Directory.Exists(Paths.Texture)) return;

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
        }

        private static void LoadTextures(string subdir, Action<string, byte[]> load)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                var entriesInSubdirectory = archive.Entries
                .Where(entry => entry.FullName.StartsWith(subdir+"/", StringComparison.OrdinalIgnoreCase))
                .ToList();

                foreach (ZipArchiveEntry entry in entriesInSubdirectory)
                {
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
