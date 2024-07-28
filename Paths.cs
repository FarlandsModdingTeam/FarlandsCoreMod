using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FarlandsCoreMod
{
    public static class Paths
    {
        public static string Dialogue => Path.Combine(Plugin, "FarlandsDialogueMod");
        public static string Texture => Path.Combine(Plugin, "FarlandsTextureMod");
        public static string Plugin => BepInEx.Paths.PluginPath;
        public static string Config => BepInEx.Paths.ConfigPath;
    }
}
