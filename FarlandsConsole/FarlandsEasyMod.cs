using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using PixelCrushers;
using System.Diagnostics;
using MoonSharp.Interpreter;

namespace FarlandsCoreMod.FarlandsConsole
{
    public class FarlandsEasyMod
    {
        public DynValue Mod;
        public string Tag;
        public Dictionary<string, byte[]> PathValue = new();
        
        public void LoadZip(string zipPath)
        {
            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    // Recorrer cada entrada en el archivo ZIP
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // Abrir la entrada y leer su contenido en el MemoryStream
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.CopyTo(ms);
                            }

                            // Guardar el contenido en el diccionario
                            UnityEngine.Debug.Log(entry.FullName);
                            PathValue[entry.FullName] = ms.ToArray();
                        }
                    }
                }
            }
        }
        public static FarlandsEasyMod FromZip(string zipPath)
        { 
            var fem = new FarlandsEasyMod();
            fem.LoadZip(zipPath);
            return fem;
        }
        public byte[] this[string path]
        {
            get => PathValue[path];
            set => PathValue[path] = value;
        }

        public void ExecuteMain()
        {
            Manager.Execute(this["main.lua"], this);
            Mod = Manager.MOD;
            Tag = Mod.Table.Get("tag").String;
        }
    }
}
