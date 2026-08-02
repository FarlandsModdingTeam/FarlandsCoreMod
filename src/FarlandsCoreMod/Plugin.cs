using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using FarlandsCoreMod.Managers;

namespace FarlandsCoreMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class FCMPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private List<AbstractManager> managers;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        managers = new();

        LoadManager<SaveManager>();
    }

    private void LoadManager<T>() where T : AbstractManager
    {
        Logger.LogDebug($"Creating Manager «{typeof(T).Name}»");
        var component = gameObject.AddComponent<T>();
        component.FCM = this;
        component.Logger = BepInEx.Logging.Logger.CreateLogSource($"FCM.{typeof(T).Name}");
        managers.Add(component);
        component.OnLoad();
    }

    public T GetManager<T>() where T : AbstractManager => (T)managers.FirstOrDefault(m => m is T);
}
