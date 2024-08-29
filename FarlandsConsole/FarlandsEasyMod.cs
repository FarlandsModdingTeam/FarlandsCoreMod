using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using PixelCrushers;
using System.Diagnostics;
using MoonSharp.Interpreter;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine.UIElements;

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
        // P_PCero
        public Dictionary<string, byte[]> PathValue = new();
        public ConfigFile ConfigFile;
      
        //TODO que se puedan leer carpetas

        /// <summary>
        ///     carga un archivo zip y lo guarda en el diccionario
        /// </summary>
        /// <param name="zipPath"></param>
        // CIUDADO
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

      
        //TODO comprobar
        public void LoadFolder(string path, string acumPath = "")
        {
            foreach (var file in Directory.EnumerateFiles(path))
                PathValue[Path.Combine(acumPath, file)] = File.ReadAllBytes(file);

            foreach (var dir in Directory.EnumerateDirectories(path))
                LoadFolder(Path.Combine(path, dir), Path.Combine(path, acumPath));

        }

      
        /// <summary>
        ///     
        /// </summary>
        /// <param name="zipPath"></param>
        /// <returns>fem</returns> 
        // CIUDADO
        public static FarlandsEasyMod FromZip(string zipPath)
        { 
            var fem = new FarlandsEasyMod();
            fem.LoadZip(zipPath);
            return fem;
        }

        // CIUDADO
        public static void LoadAndAddZip(string zipPath)
        {
            var fem = FromZip(zipPath);
            Manager.CURRENT_MOD = fem;
            fem.ExecuteMain();
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

        public string[] GetFilesInFolder(string t, string folder) =>
            PathValue.Where(x => x.Key.StartsWith(folder) && !x.Key.EndsWith("/")).Select(x=> t + "/" + x.Key).ToArray();

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
