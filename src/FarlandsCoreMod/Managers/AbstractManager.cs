using UnityEngine;
using BepInEx;
using BepInEx.Logging;

namespace FarlandsCoreMod.Managers;

public abstract class AbstractManager : MonoBehaviour
{
    public FCMPlugin FCM;

    public ManualLogSource Logger;

    public abstract void OnLoad();

    protected virtual void OnDestroy()
    {
        if (Logger != null)
        {
            BepInEx.Logging.Logger.Sources.Remove(Logger);
            Logger.Dispose();
        }
    }
}
