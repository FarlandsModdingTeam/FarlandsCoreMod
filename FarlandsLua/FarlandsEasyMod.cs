﻿using System;
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

namespace FarlandsCoreMod.FarlandsLua
{

    /// <summary>
    ///     FarlandsEasyMod es el mod en si 
    /// </summary>
    public class FarlandsEasyMod
    {
        // ----------------------- DECLARACIONES ----------------------- //
        /// <summary>
        ///     Informacion del mod en DynValue -> LUA
        /// </summary>
        public DynValue Mod;


        /// <summary>
        ///     Texto identificador del mod, ejemplo: MOD("francoPea")
        /// </summary>
        public string Tag;


        /// <summary>
        ///     Diccionario que contiene la ruta y el contenido de los archivos del mod
        /// </summary>
        public Dictionary<string, byte[]> PathValue = new();


        /// <summary>
        ///    Configuracion del mod
        /// </summary>
        public ConfigFile ConfigFile;


        //TODO: que se puedan leer carpetas
        /// <summary>
        ///     carga un archivo zip y lo guarda en el diccionario
        ///     Abertencias: CIUDADO
        /// </summary>
        /// <param name="zipPath">La direccion del zip</param> 
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
        ///     Cargar mod desde carpeta en vez dedde zip
        ///     TODO: comprobar
        /// </summary>
        /// <param name="path">Direccin donde esta</param>
        /// <param name="acumPath">Direccion de lo que tengo ni idea</param>
        public void LoadFolder(string path, string acumPath = "")
        {
            foreach (var file in Directory.EnumerateFiles(path))
                PathValue[Path.Combine(acumPath, file)] = File.ReadAllBytes(file);

            foreach (var dir in Directory.EnumerateDirectories(path))
                LoadFolder(Path.Combine(path, dir), Path.Combine(path, acumPath));

        }


        /// <summary>
        ///     Carga el mod del zip en en un FarlandsEasyMod
        ///     Abvertencias: CIUDADO
        /// </summary>
        /// <param name="zipPath">Direccion donde esta el zip</param>
        /// <returns>Desvuelve un el mod en FarlandsEasyMod</returns>
        public static FarlandsEasyMod FromZip(string zipPath)
        { 
            var fem = new FarlandsEasyMod();
            fem.LoadZip(zipPath);
            return fem;
        }


        /// <summary>
        ///     Carga un archivo ZIP, crea una instancia de <see cref="FarlandsEasyMod"/> y ejecuta el archivo main.lua.
        ///     Abvertencias: CIUDADO
        /// </summary>
        /// <param name="zipPath">Direccion donde eta el zip</param>
        public static void LoadAndAddZip(string zipPath)
        {
            var fem = FromZip(zipPath);
            LuaManager.CURRENT_MOD = fem;
            fem.ExecuteMain();
        }


        /// <summary>
        ///     Getters y Setters de PathValue
        /// </summary>
        /// <param name="path">Es una direccion, ¿De que? Ni puta idea</param>
        public byte[] this[string path]
        {
            get => PathValue[path];
            set => PathValue[path] = value;
        }


        /// <summary>
        ///     Obtiene una lista de archivos en una carpeta específica dentro del diccionario PathValue.
        /// </summary>
        /// <param name="t">Un prefijo que se añadirá a cada ruta de archivo en el resultado.</param>
        /// <param name="folder">La carpeta dentro de PathValue de la cual se quieren obtener los archivos.</param>
        /// <returns>
        /// Un array de strings que contiene las rutas de los archivos en la carpeta especificada, con el prefijo t añadido a cada ruta.
        /// </returns>
        public string[] GetFilesInFolder(string t, string folder) =>
            PathValue.Where(x => x.Key.StartsWith(folder) && !x.Key.EndsWith("/")).Select(x=> t + "/" + x.Key).ToArray();


        /// <summary>
        ///    Ejecuta el archivo main.lua
        ///    y guarda la informacion del mod
        /// </summary>
        public void ExecuteMain()
        {
            LuaManager.Execute(this["main.lua"], this);
            Mod = LuaManager.MOD;
            Tag = Mod.Table.Get("tag").String;
        }
    }
}
