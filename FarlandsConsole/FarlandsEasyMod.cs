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

    /// <summary>
    ///     FarlandsEasyMod es el mod en si 
    /// </summary>
    public class FarlandsEasyMod
    {
        // ----------------------- DECLARACIONES ----------------------- //
        /// <summary>
        ///     Informacion del mod en lua
        /// </summary>
        public DynValue Mod;

        /// <summary>
        ///     a inventar
        /// </summary>
        public string Tag;

        /// <summary>
        ///     Diccionario que contiene la ruta y el contenido de los archivos del mod
        /// </summary>
        public Dictionary<string, byte[]> PathValue = new();
        

        /// <summary>
        ///     carga un archivo zip y lo guarda en el diccionario
        /// </summary>
        /// <param name="zipPath"></param>
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

        /// <summary>
        ///     
        /// </summary>
        /// <param name="zipPath"></param>
        /// <returns>fem</returns>
        public static FarlandsEasyMod FromZip(string zipPath)
        { 
            var fem = new FarlandsEasyMod();
            fem.LoadZip(zipPath);
            return fem;
        }

        /// <summary>
        ///     Getters y Setters de PathValue
        /// </summary>
        /// <param name="path"></param>
        public byte[] this[string path]
        {
            get => PathValue[path];
            set => PathValue[path] = value;
        }

        /// <summary>
        ///    Ejecuta el archivo main.lua
        ///    y guarda la informacion del mod
        /// </summary>
        public void ExecuteMain()
        {
            Manager.Execute(this["main.lua"], this);
            Mod = Manager.MOD;
            Tag = Mod.Table.Get("tag").String;
        }
    }
}
