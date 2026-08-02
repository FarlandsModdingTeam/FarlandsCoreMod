using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using FarlandsCoreMod.Managers;
using HarmonyLib;

namespace FarlandsCoreMod;

[BepInPlugin(FCMInfo.PLUGIN_GUID, FCMInfo.PLUGIN_NAME, FCMInfo.PLUGIN_VERSION)]
public class FCMPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private List<AbstractManager> managers;

    private Harmony harmony;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {FCMInfo.PLUGIN_GUID} is loaded!");
        managers = new();

        harmony = new Harmony(FCMInfo.PLUGIN_GUID);
        harmony.PatchAll();
        Logger.LogInfo("All Harmony patches applied successfully");

        LoadManager<SaveManager>();
    }

    private void LoadManager<T>() where T : AbstractManager
    {
        var managerName = typeof(T).Name;
        Logger.LogDebug($"Creating Manager «{managerName}»");
        var component = gameObject.AddComponent<T>();
        component.FCM = this;
        component.Logger = BepInEx.Logging.Logger.CreateLogSource($"FCM.{managerName}");
        managers.Add(component);
        component.OnLoad();
    }

    public T GetManager<T>() where T : AbstractManager => (T)managers.FirstOrDefault(m => m is T);
}
