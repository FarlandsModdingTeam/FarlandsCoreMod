using FarlandsCoreMod.Core;

namespace FarlandsCoreMod;

public abstract class FarlandsMod : BepInEx.BaseUnityPlugin
{
    public FCMPlugin FCM;

    public void Awake()
    {
        FCM = FCMPlugin.GetFCM();
    }
}
