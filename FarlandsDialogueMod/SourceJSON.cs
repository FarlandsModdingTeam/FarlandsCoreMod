using I2.Loc;
using Language.Lua;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using FarlandsCoreMod.Utiles;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;
using UnityEngine.UI;
using Unity.Profiling.LowLevel.Unsafe;
using Rewired.Demos;
using System.IO;

namespace FarlandsCoreMod.FarlandsDialogueMod
{
    public class SourceJSON
    {
        public string Name = "NONAME";
        public string Code = "NaN";
        public string farlandsVersion = "NaN";
        public string fdmVersion = Properties.Resources.Version;
        public TranslationJSON translations = new();
        public CreateJSON create = new();

        public SourceJSON() 
        {
            TextAsset textAsset = UnityEngine.Resources.Load<TextAsset>("Version");
            string farlandsVersion = "";
            if (textAsset != null)
            {
                farlandsVersion = textAsset.text;
            }
        }
        public class TJson<T>
        { 
            public Dictionary<string, T> General = new();
            public Dictionary<string, TInventory> Inventory = new();
            public TDialogue Dialogues = new();

            public class TInventory
            {
                public T name;
                public T description;
            }

            public class TDialogue
            {
                public Dictionary<string, T> Actors = new();
                public Dictionary<string, T> Variables = new();
                public Dictionary<string, T> Conversations = new();
            }
        }
        public class TranslationJSON : TJson<string>;
        public class CreateJSON : TJson<TermJSON>;

        public class TermJSON
        {
            public int ActorID = -1;
            public int ConversantID = -1;
            public string Description = "";
            public string Title = "";
            public float canvasZoom = 1;
            /* Articy Id */
            public EntryJson Entry;
            public List<EntryJson> Entries;
            public int ID = -1;
            public string nodeColor;
            
            /*
            entryGroups
            fields
            overrideSettings
            id
             */

            public class EntryJson
            {
                public int ActorID = -1;
                public string AnimationFiles = "";
                public string AudioFiles = "";
                public int ConversantID = -1;
                public string DialogueText = "";
                public string MenuText = "";
                public string Sequence = "";
                public string Title = "";

                public class RectJson
                { 
                    public float x=0;
                    public float y=0;
                    public float width=0;
                    public float height=0;

                    public Rect ToRect() => new Rect(x, y, width, height);
                    public static RectJson From(Rect r) => new RectJson()
                    {
                        x= r.x,
                        y= r.y,
                        width= r.width,
                        height= r.height
                    };
                }
                public RectJson canvasRect = new();
                public ConditionPriority conditionPriority;
                public string conditionsString = "";
                public int ConversationID = -1;
                public bool delaySimStatus = false;
                public string falseConditionAction = "";
                public int ID = -1;
                public bool isGroup = false;
                public bool isRoot = false;
                public string nodeColor = "";
                public List<LinkJson> outgoingLinks;
                public string userScript = "";

                public eTermType termType;
                public List<string> Languages;
                public List<byte> Flags;

                public static EntryJson From(DialogueEntry entry)
                {
                    var res = new EntryJson();
                    res.ActorID = entry.ActorID;
                    res.AnimationFiles = entry.AnimationFiles;
                    res.AudioFiles = entry.AudioFiles;
                    res.ConversantID = entry.ConversantID;
                    res.DialogueText = entry.DialogueText;
                    res.MenuText = entry.MenuText;
                    res.Sequence = entry.Sequence;
                    res.Title = entry.Title;
                    res.canvasRect = RectJson.From(entry.canvasRect);
                    res.conditionPriority = entry.conditionPriority;
                    res.conditionsString = entry.conditionsString;
                    res.ConversationID = entry.conversationID;
                    res.delaySimStatus = entry.delaySimStatus;
                    res.falseConditionAction = entry.falseConditionAction;
                    res.ID = entry.id;
                    res.isGroup = entry.isGroup;
                    res.isRoot = entry.isRoot;
                    res.nodeColor = entry.nodeColor;
                    res.userScript = entry.userScript;
                    res.outgoingLinks = entry.outgoingLinks.Select(LinkJson.From).ToList();

                    return res;
                }
                public void ModifyFrom(DialogueEntry entry)
                {
                    ActorID = entry.ActorID;
                    AnimationFiles = entry.AnimationFiles;
                    AudioFiles = entry.AudioFiles;
                    ConversantID = entry.ConversantID;
                    DialogueText = entry.DialogueText;
                    MenuText = entry.MenuText;
                    Sequence = entry.Sequence;
                    Title = entry.Title;
                    canvasRect = RectJson.From(entry.canvasRect);
                    conditionPriority = entry.conditionPriority;
                    conditionsString = entry.conditionsString;
                    ConversationID = entry.conversationID;
                    delaySimStatus = entry.delaySimStatus;
                    falseConditionAction = entry.falseConditionAction;
                    ID = entry.id;
                    isGroup = entry.isGroup;
                    isRoot = entry.isRoot;
                    nodeColor = entry.nodeColor;
                    userScript = entry.userScript;
                    outgoingLinks = entry.outgoingLinks.Select(LinkJson.From).ToList();

                }
                public DialogueEntry ToDialogueEntry()
                {
                    var dialogue = new DialogueEntry();
                    dialogue.fields = new();
                    
                    dialogue.ActorID = ActorID;
                    dialogue.AnimationFiles = AnimationFiles;
                    dialogue.AudioFiles = AudioFiles;
                    dialogue.ConversantID = ConversantID;
                    dialogue.DialogueText = DialogueText;
                    dialogue.MenuText = MenuText;
                    dialogue.Sequence = Sequence;
                    dialogue.Title = Title;
                    dialogue.canvasRect = canvasRect.ToRect();
                    dialogue.conditionPriority = conditionPriority;
                    dialogue.conditionsString = conditionsString;
                    dialogue.conversationID = ConversationID;
                    dialogue.delaySimStatus = delaySimStatus;
                    dialogue.falseConditionAction = falseConditionAction;
                    dialogue.id = ID;
                    dialogue.isGroup = isGroup;
                    dialogue.isRoot = isRoot;
                    dialogue.nodeColor = nodeColor;
                    dialogue.userScript = userScript;
                    dialogue.outgoingLinks = this.outgoingLinks.Select(LinkJson.ToLink).ToList();

                    return dialogue;
                }

                public static DialogueEntry ToDialogueEntry(EntryJson entry) => entry.ToDialogueEntry();

                public class LinkJson
                {
                    public int destinationConversationID;
                    public int destinationDialogueID;
                    public bool isConnector;
                    public int originConversationID;
                    public int originDialogueID;
                    public ConditionPriority priority;

                    public static LinkJson From(Link link) => new()
                    {
                        destinationConversationID = link.destinationConversationID,
                        destinationDialogueID = link.destinationDialogueID,
                        isConnector = link.isConnector,
                        originConversationID = link.originConversationID,
                        originDialogueID = link.originDialogueID,
                        priority = link.priority,
                    };

                    public Link ToLink() => new()
                    { 
                        destinationConversationID = destinationConversationID,
                        destinationDialogueID = destinationDialogueID,
                        isConnector = isConnector,
                        originConversationID = originConversationID,
                        originDialogueID = originDialogueID,
                        priority = priority
                    };

                    public static Link ToLink(LinkJson link) => link.ToLink();
                } 
            }

            public static TermJSON From(TermData data)
            { 
                var res = new TermJSON();
                res.Entry = new()
                { 
                    termType = data.TermType,
                    Languages = new(data.Languages),
                    Flags = new(data.Flags),
                };

                return res;
            }
            public void LoadFrom(PixelCrushers.DialogueSystem.Conversation data)
            {
                Title = data.Title;
                ID = data.id;
                ActorID = data.ActorID;
                ConversantID = data.ConversantID;

                for (int i = 0; i < data.dialogueEntries.Count; i++)
                {
                    if (i < Entries.Count) Entries[i].ModifyFrom(data.dialogueEntries[i]);
                    else Entries.Add(EntryJson.From(data.dialogueEntries[i]));
                }
            }

            public static TermJSON From(PixelCrushers.DialogueSystem.Conversation data)
            {
                var res = new TermJSON
                {
                    Entries = new()
                };

                res.LoadFrom(data);
                return res;
            }

            public Conversation ToConversation()
            {
                var conversation = new Conversation();
                conversation.fields = new();
                conversation.Title = this.Title;
                conversation.id = this.ID;
                conversation.ActorID = this.ActorID;
                conversation.ConversantID = this.ConversantID;
                conversation.dialogueEntries = Entries.Select(EntryJson.ToDialogueEntry).ToList();

                return conversation;
            }

            public static Conversation ToConversation(TermJSON term) => term.ToConversation();
        }
        public static SourceJSON FromBytes(byte[] raw) => SourceJSON.FromJson(Encoding.UTF8.GetString(raw));

        private static string splitInFirst(string str,string separator, out string rest)
        { 
            var i = str.IndexOf(separator);
            rest = str.Substring(i+1);
            return str.Substring(0,i);
        }
        public static void FromSource(LanguageSourceData source, SourceJSON res)
        {
            foreach (var term in source.mDictionary)
            {
                Debug.Log(term.Key);
                var VALUE = TermJSON.From(term.Value);

                string op = splitInFirst(term.Key, "/", out var key);

                if (key == "") continue;

                if (op == "General") res.create.General.Add(key, VALUE);
                else if (op == "Inventory")
                {
                    string[] stroke = key.Split("_");
                    string obj = stroke[2];
                    string field = stroke[1];

                    if (!res.create.Inventory.ContainsKey(obj))
                        res.create.Inventory.Add(obj, new());

                    if (field == "name")
                        res.create.Inventory[obj].name = VALUE;
                    else if (field == "description")
                        res.create.Inventory[obj].description = VALUE;
                }
                else if (op == "Dialogue System")
                {
                    string section = splitInFirst(key, "/", out key);

                    if (section == "Actor")
                    {
                        //TODO
                    }
                    else if (section == "Variable")
                    {
                        //TODO
                    }
                    else if (section == "Conversation")
                    {
                        Debug.Log("DIALOG");

                        section = splitInFirst(key, "/", out key);
                        section = section.Replace(".", "/");

                        if (!res.create.Dialogues.Conversations.ContainsKey(section))
                            res.create.Dialogues.Conversations.Add(section, new() { Entries = new() });

                        var conversation = res.create.Dialogues.Conversations[section];
                        section = splitInFirst(key, "/", out key);
                        section = splitInFirst(key, "/", out key);

                        var i = int.Parse(section);

                        for (int j = conversation.Entries.Count; j <= i; j++)
                            conversation.Entries.Add(new());

                        conversation.Entries[i] = VALUE.Entry;
                    }

                }
            }
        }

        public static void FromDialogue(DialogueDatabase database, SourceJSON res)
        {
            foreach (var conversation in CreateCache(database.conversations))
            {
                if (res.create.Dialogues.Conversations.ContainsKey(conversation.Key))
                    res.create.Dialogues.Conversations[conversation.Key].LoadFrom(conversation.Value);
                else
                    res.create.Dialogues.Conversations.Add(conversation.Key, TermJSON.From(conversation.Value));
            }
        }
        public static SourceJSON FromFull(LanguageSourceData source, DialogueDatabase database)
        {
            var res = new SourceJSON();
            try
            {
                FromSource(source, res);
                FromDialogue(database, res);
            }
            catch
            {
                Debug.LogError("Error loadding custom dialogue");
            }
            return res;
        }
        private static Dictionary<string, T> CreateCache<T>(List<T> assets) where T : Asset
        {
            bool flag = typeof(T) == typeof(PixelCrushers.DialogueSystem.Conversation);
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            if (Application.isPlaying)
            {
                for (int i = 0; i < assets.Count; i++)
                {
                    T t = assets[i];
                    string key = flag ? (t as PixelCrushers.DialogueSystem.Conversation).Title : t.Name;
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, t);
                    }
                }
            }
            return dictionary;
        }
        public static SourceJSON FromJson(string json) =>
            Newtonsoft.Json.JsonConvert.DeserializeObject<SourceJSON>(json);

        public static SourceJSON FromFile(string path) =>
            FromJson(File.ReadAllText(path));

        private void AddContainingTerm(string key, string value, LanguageSourceData data)
        {
            if (data.ContainsTerm(key))
            {
                var term = data.GetTermData(key);
                term.Languages[data.GetLanguages(false).IndexOf(Name)] = value;
            }
        }
        private void AddNewTerm(string key, TermJSON.EntryJson entry, LanguageSourceData data)
        {
            var term = data.AddTerm(key, entry.termType);
            if(entry.Languages != null) term.Languages = entry.Languages.ToArray();
            if(entry.Flags != null) term.Flags = entry.Flags.ToArray();
        }

        private void AddNewTerm(string key, TermJSON value, LanguageSourceData data)
        {
            if (!data.ContainsTerm(key))
            {
                if (value.Entries == null)
                {
                    AddNewTerm($"{key}", value.Entry, data);
                    return;
                }

                int i = 0;
                foreach (var entry in value.Entries)
                {
                    AddNewTerm($"{key}/Entry/{i}", entry, data);
                    i++;
                }
            }
        }

        public LanguageSourceData LoadInMain() => LoadIn(LocalizationManager.Sources.First());
        internal DialogueDatabase LoadInData() => LoadIn(DialogueManager.instance.masterDatabase);
        public LanguageSourceData LoadIn(LanguageSourceData data)
        {
            Debug.Log("RECARGANDO DATA");
            data.AddLanguage(Name, Code);

            if (translations != null)
            {
                if (translations.General != null)
                    foreach (var pair in translations.General)
                        AddContainingTerm($"General/{pair.Key}", pair.Value, data);

                if (translations.Inventory != null)
                    foreach (var pair in translations.Inventory)
                    {
                        var translation = pair.Value;
                        if(translation.name != null)
                        AddContainingTerm($"Inventory/item_name_{pair.Key}",
                            translation.name, data);

                        if (translation.description != null)
                            AddContainingTerm($"Inventory/item_description_{pair.Key}",
                            translation.description, data);
                    }

                if (translations.Dialogues != null)
                {
                    if (translations.Dialogues.Actors != null)
                        foreach (var pair in translations.Dialogues.Actors)
                        {
                            AddContainingTerm($"Dialogue System/Actor/{pair.Key}/Name", pair.Value, data);
                        }

                    if (translations.Dialogues.Variables != null)
                        foreach (var pair in translations.Dialogues.Variables)
                        {
                            AddContainingTerm($"Dialogue System/Variable/{pair.Key}/Initial Value", pair.Value, data);
                        }

                    if (translations.Dialogues.Conversations != null)
                        foreach (var pair in translations.Dialogues.Conversations)
                        {
                            string section = splitInFirst(pair.Key, "/", out string key);
                            AddContainingTerm($"Dialogue System/Conversation/{section}/Entry/{key}/Dialogue Text",
                                        pair.Value, data);
                        }
                }
            }

            if (create != null)
            {
                if (create.General != null)
                    foreach (var pair in create.General)
                        AddNewTerm($"General/{pair.Key}", pair.Value, data);

                if (create.Inventory != null)
                    foreach (var pair in create.Inventory)
                    {
                        var translation = pair.Value;
                        AddNewTerm($"Inventory/item_name_{pair.Key}",
                            translation.name, data);
                        AddNewTerm($"Inventory/item_description_{pair.Key}",
                            translation.description, data);
                    }

                if (create.Dialogues != null)
                {
                    if (create.Dialogues.Actors != null)
                        foreach (var pair in create.Dialogues.Actors)
                        {
                            AddNewTerm($"Dialogue System/Actor/{pair.Key}/Name", pair.Value, data);
                        }

                    if (create.Dialogues.Variables != null)
                        foreach (var pair in create.Dialogues.Variables)
                        {
                            AddNewTerm($"Dialogue System/Variable/{pair.Key}/Initial Value", pair.Value, data);
                        }

                    if (create.Dialogues.Conversations != null)
                        foreach (var pair in create.Dialogues.Conversations)
                        {
                            if (pair.Value != null)
                                AddNewTerm($"Dialogue System/Conversation/{pair.Key.Replace("/", ".")}",
                                        pair.Value, data);
                        }
                }
            }

            return data;
        }
        public DialogueDatabase LoadIn(DialogueDatabase database)
        {
            if (this.create.Dialogues != null && this.create.Dialogues.Conversations != null)
                this.create.Dialogues.Conversations.Values.Select(TermJSON.ToConversation).
                    ToList().ForEach(database.AddConversation);

            return database;
        }

        public string ToJson() => Newtonsoft.Json.JsonConvert.SerializeObject(this);

        
    }
}
