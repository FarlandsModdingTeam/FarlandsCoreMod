using BepInEx;

namespace FarlandsEmptyMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class YourPlugin: FarlandsCoreMod.FarlandsMod
    {
        public override string SHORT_NAME => "YourShortName";

        public override void OnStart()
        {
            
        }
    }
}
