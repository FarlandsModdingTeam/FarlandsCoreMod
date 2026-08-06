using System;
using System.Collections.Generic;
using I2.Loc;

namespace FarlandsCoreMod.Core.Language;

public class LanguageRegister
{
    public LanguageSourceData Source;
    public LanguageRegister()
    {
        Source = new();
        Source.mLanguages.Add(new LanguageData()
        {
            Name = "Español",
            Code = "es",
        });
        Source.mLanguages.Add(new LanguageData()
        {
            Name = "English",
            Code = "en",
        });
    }

    public List<TermData> registeredLanguages = new();
    public void ForEach(Action<TermData> e) =>
      registeredLanguages.ForEach(e);


    public void Register(TermData data)
    {
        registeredLanguages.Add(data);
    }
}

public class LanguageManager : AbstractManager
{
    public override string ConfigSection => "LanguageManager";
    public static Dictionary<string, LanguageRegister> LanguageRegister;

    public override void OnLoad()
    {
        LanguageRegister = new();
    }

    public override void Start()
    {
        foreach (var pair in LanguageRegister)
        {
            Logger.LogDebug($"Loadding Languages");
            pair.Value.ForEach(pair.Value.Source.mTerms.Add);
            pair.Value.Source.Awake();
        }
    }

    public void Register(string guid, TermData data)
    {
        if (!LanguageRegister.ContainsKey(guid))
            LanguageRegister.Add(guid, new());

        LanguageRegister[guid].Register(data);
    }
}
