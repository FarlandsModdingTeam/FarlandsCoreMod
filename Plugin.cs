using BepInEx;

namespace FarlandsEmptyMod
{
    [BepInPlugin("IDENTIFICADOR", "NOMBRE", "VERSION")]
    public class YourPlugin: FarlandsCoreMod.FarlandsMod
    {
        public override string SHORT_NAME => "YourShortName";

        public override void OnStart()
        {
            
        }
    }
}
