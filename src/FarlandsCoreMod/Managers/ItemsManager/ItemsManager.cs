using HarmonyLib;
using Farlands;
using System.Collections.Generic;
using FarlandsCoreMod.Core.Language;
using Farlands.Inventory;
using Farlands.Dev;

namespace FarlandsCoreMod.Core.Items;

public class ItemsManager : AbstractManager
{
    public LanguageManager Language;

    public override string ConfigSection => "ItemsManager";
    public ItemRegister Register;
    public bool loadedItems;

    public override void OnLoad()
    {
        Register = new();
        FCM.Debug.RegisterCommand(new DebugCommand("fcm:items", "Devuelve una lista de los items agregados", "fcm:items", () =>
        {
            FCM.Debug.Clear();

            FCM.Debug.Print("--- Registered Items ---\n");
            foreach (var p in Register.IdMap)
            {
                foreach (var v in p.Value)
                {
                    FCM.Debug.Print($"{p.Key}:{v.Key} => {v.Value.Item.RealId}");
                }
            }
        }));
    }

    public Item RegisterItem(string guid, Item item, string[] langs)
    {
        if (loadedItems)
            throw new System.Exception("Items has already been loaded");


        Register.Register(guid, new ItemRegister.Entry(item, langs));
        return item;
    }

    public void InsetIntoDB(List<InventoryItem> items, LanguageManager lang)
    {
        Register.InsertIntoDB(items, lang);
        loadedItems = true;
    }

}

[HarmonyPatch(typeof(FarlandsGameManager))]
public static class FarlandsGameManagerPatch
{
    [HarmonyPatch(nameof(FarlandsGameManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(FarlandsGameManager __instance)
    {
        var fcm = FCMPlugin.GetFCM();
        var items = fcm.Item;

        items.InsetIntoDB(__instance.scriptableObjectsDB.inventoryItems, fcm.Language);
    }
}
