using BepInEx.Configuration;
using JanduSoft;
using UnityEngine;

namespace FarlandsCoreMod.Managers;

public class SaveManager : AbstractManager
{

    public SaveManagerConfig Config;

    public override string ConfigSection => "SaveManager";

    public override void OnLoad()
    {
        Config = new();
        Config.Load(this);

        if (Config.AlternativeSaveFile)
        {
            Logger.LogInfo("Alternative Save File activated");
            PCSave.savegamefile = "/gamedata.fcm.dat";
        }
    }
}

public class SaveManagerConfig : ManagerConfig
{
    private ConfigEntry<bool> _alternativeSaveFile;
    public bool AlternativeSaveFile => _alternativeSaveFile.Value;

    public override void Load(AbstractManager manager)
    {
        _alternativeSaveFile = manager.AddConfig("AlternativeSaveFile", true, "Change the save file to protect the main one");
    }
}
