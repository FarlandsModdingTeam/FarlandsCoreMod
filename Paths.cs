using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FarlandsCoreMod
{
    public static class Paths
    {
        public static string Plugin => BepInEx.Paths.PluginPath;
        public static string Config => BepInEx.Paths.ConfigPath;
        public static string Exe => Application.dataPath; //TODO revisar
    }
}
