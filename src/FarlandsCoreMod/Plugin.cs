using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using Farlands.Inventory;

using FarlandsCoreMod.Core.Items;
using FarlandsCoreMod.Core.Save;
using FarlandsCoreMod.Core.Language;

using HarmonyLib;
using UnityEngine;
using FarlandsCoreMod.Core.Debug;

namespace FarlandsCoreMod.Core;

[BepInPlugin(FCMInfo.PLUGIN_GUID, FCMInfo.PLUGIN_NAME, FCMInfo.PLUGIN_VERSION)]
public class FCMPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private List<AbstractManager> managers;

    private Harmony harmony;

    public SaveManager Save;
    public ItemsManager Item;
    public LanguageManager Language;
    public DebugManager Debug;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {FCMInfo.PLUGIN_GUID} is loaded!");
        managers = new();

        harmony = new Harmony(FCMInfo.PLUGIN_GUID);
        harmony.PatchAll();
        Logger.LogInfo("All Harmony patches applied successfully");

        #region Managers

        LoadManager(out Debug);

        LoadManager(out Save);
        LoadManager(out Item);
        LoadManager(out Language);

        #endregion

        var testItem = Item.RegisterItem(FCMInfo.PLUGIN_GUID,
            new Item("Test", InventoryItem.ItemType.Seed, 10, 100, true, true, 1),
            new string[] { "prueba", "test" }
        );
    }

    private void Start()
    {
        managers.ForEach(m => m.Start());
    }

    private void LoadManager<T>(out T manager) where T : AbstractManager
    {
        var managerName = typeof(T).Name;
        Logger.LogDebug($"Creating Manager «{managerName}»");

        manager = gameObject.AddComponent<T>();
        manager.FCM = this;
        manager.Logger = BepInEx.Logging.Logger.CreateLogSource($"FCM.{managerName}");
        managers.Add(manager);
        manager.OnLoad();
    }

    public T GetManager<T>() where T : AbstractManager => (T)managers.FirstOrDefault(m => m is T);

    public static FCMPlugin GetFCM() => GameObject.FindAnyObjectByType<FCMPlugin>();
}
