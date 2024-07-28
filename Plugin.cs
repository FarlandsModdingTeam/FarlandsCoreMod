using BepInEx;
using FarlandsCoreMod;

namespace FarlandsEmptyMod
{
    [BepInPlugin("IDENTIFICADOR", "NOMBRE", "VERSION")]
    public class YourPlugin: FarlandsMod
    {
        public override string SHORT_NAME => "YourShortName";

        // Se ejecuta cuando carga el mod
        public override void OnStart()
        {
            
        }

        // Se ejecuta cuando carga el juego
        public override void OnFirstFrame()
        {
            
        }
    }
}
