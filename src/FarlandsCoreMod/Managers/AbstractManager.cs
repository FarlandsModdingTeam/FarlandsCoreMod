using UnityEngine;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace FarlandsCoreMod.Managers;

public abstract class AbstractManager : MonoBehaviour
{
    public FCMPlugin FCM;
    public ManualLogSource Logger;
    public abstract void OnLoad();

    public abstract string ConfigSection { get; }

    public ConfigEntry<T> AddConfig<T>(string key, T value, string description) => FCM.Config.Bind(ConfigSection, key, value, description);

    protected virtual void OnDestroy()
    {
        if (Logger != null)
        {
            BepInEx.Logging.Logger.Sources.Remove(Logger);
            Logger.Dispose();
        }
    }
}

public abstract class ManagerConfig
{
    public abstract void Load(AbstractManager manager);
}
