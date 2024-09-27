using CommandTerminal;
using Farlands;
using Farlands.DataBase;
using Farlands.Dev;
using Farlands.Inventory;
using Farlands.PlantSystem;
using FarlandsCoreMod.FarlandsLua;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using I2.Loc;
using JanduSoft;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace FarlandsCoreMod.FarlandsConsole.Functions
{
    public static class LuaFunctions
    {
        public static void AddToLua()
        {
            //TODO agergar forma para ampliar la cámara

            LuaManager.LUA.Globals["MOD"] = (string tag) =>
            {
                var code =
@$"
{tag} = {{}}
{tag}.tag = '{tag}'
{tag}.scenes = {{}}
{tag}.event = {{}}
{tag}.event.scene = {{}}
{tag}.event.scene.change = {{}}
{tag}.event.language = {{}}
{tag}.event.language.change = {{}}
{tag}.event.dialogue = {{}}
{tag}.event.dialogue.portrait = {{}}
{tag}.event.inventory = {{}}

_mod_ = {tag}";

                LuaManager.Execute(code, null);
                LuaManager.CURRENT_MOD.ConfigFile = new(Path.Combine(Paths.Config, $"{tag}.cfg"), true);
                LuaManager.EasyMods.Add(tag, LuaManager.CURRENT_MOD);
            };

            LuaManager.LUA.Globals["config"] = (string section, string key, DynValue def, string description) =>
            {
                var code =
@$"
_mod_.config = _mod_.config or {{}}
_mod_.config.{section} = _mod_.config.{section} or {{}}
";
                LuaManager.Execute(code, null);
                if (def.Type == DataType.Boolean)
                {
                    var entry = LuaManager.CURRENT_MOD.ConfigFile.Bind(section, key, def.Boolean, description);

                    LuaManager.LUA.Globals.Get("_mod_")
                        .Table.Get("config")
                        .Table.Get(section)
                        .Table.Set(key, DynValue.NewCallback((ctx, args) => DynValue.NewBoolean(entry.Value)));
                }

            };

            LuaManager.LUA.Globals["get_input"] = (string action) =>
            {
                var player = GameObject.FindObjectOfType<PlayerController>();
                return player.player.GetButtonDown(action) && player.inputEnabled;
            };

            // ----------------------- COMANDO DE COMANDOS ----------------------- //
            LuaManager.LUA.Globals["execute_command"] = (string _comando) =>
            {
                if (LuaManager._o == null)
                {
                    LuaManager._o = new GameObject("FalsoDebugController");
                    LuaManager._o.AddComponent<Farlands.Dev.DebugController>();
                }
                List<object> _lista = LuaManager._o.GetComponent<Farlands.Dev.DebugController>().commandList;


                string[] args = _comando.Split(' ', StringSplitOptions.None);
                for (int i = 0; i < _lista.Count; i++)
                {
                    DebugCommandBase debugCommandBase = _lista[i] as DebugCommandBase;
                    if (args.Contains(debugCommandBase.commandId))
                    {
                        if (_lista[i] is DebugCommand)
                        {
                            (_lista[i] as DebugCommand).Invoke();
                        }
                        else if (_lista[i] is DebugCommand<int>)
                        {
                            (_lista[i] as DebugCommand<int>).Invoke(int.Parse(args[1]));
                        }
                        else if (_lista[i] is DebugCommand<int, int>)
                        {
                            int value = int.Parse(args[1]);
                            int value2 = int.Parse(args[2]);
                            if (int.TryParse(args[1], out value) && int.TryParse(args[2], out value2))
                            {
                                (_lista[i] as DebugCommand<int, int>).Invoke(value, value2);
                            }
                            else
                            {
                                Debug.LogWarning("Invalid parameter format for DebugCommand<int, int>.");
                            }
                        }
                    }
                }

                //Destroy(_o);
            };

            // ----------------------- FUNCIONES DE ESCENA ----------------------- //
            LuaManager.LUA.Globals["load_scene"] = (DynValue scene) =>
            {
                if (scene.Type == DataType.String)
                    SceneManager.LoadScene(scene.String);
                else if (scene.Type == DataType.Number)
                    SceneManager.LoadScene(Convert.ToInt32(scene.Number));
            };

            LuaManager.LUA.Globals["print_scene"] = () =>
            {
                Scene currentScene = SceneManager.GetActiveScene();
                Terminal.Log($"({currentScene.buildIndex}) {currentScene.name}");
            };

            LuaManager.LUA.Globals["texture_override"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count == 0) throw new Exception("Invalid args for TextureOverride");
                else if (args.Count == 1)
                {
                    var path = args[0].String;
                    TexturesModifier.ReplaceTexture(Path.GetFileNameWithoutExtension(path), LuaManager.GetFromMod(path));
                }
                else if (args.Count == 2)
                {
                    var origin = args[0].String;
                    var path = args[1].String;
                    TexturesModifier.ReplaceTexture(origin, LuaManager.GetFromMod(path));
                }
                return DynValue.Void;
            });

            LuaManager.LUA.Globals["texture_override_in"] = DynValue.NewCallback((ctx, args) =>
            {
                var path = args[0].String;
                Debug.Log(string.Join('\n', LuaManager.GetFilesInMod(path)));

                foreach (var item in LuaManager.GetFilesInMod(path))
                    TexturesModifier.ReplaceTexture(Path.GetFileNameWithoutExtension(item), LuaManager.GetFromMod(item));

                return DynValue.Void;
            });

            // No funciona
            LuaManager.LUA.Globals["sprite_override"] = (string origin, int[] position, string path) =>
            {
                var vec = new Vector2Int(position[0], position[1]);
                TexturesModifier.ReplaceSprite(origin, vec, LuaManager.GetFromMod(path));
            };

            LuaManager.LUA.Globals["portrait_override"] = (string origin, string path) =>
            {
                string code =
@$"
    function _mod_.event.dialogue.portrait:{origin}()
        texture_override('{origin}', '{path}')
    end
    ";
                LuaManager.LUA.DoString(code);
            };

            /// <summary>
            /// 
            /// </summary>
            LuaManager.LUA.Globals["add_language"] = (string path) =>
            {
                FarlandsDialogue.FarlandsDialogueManager.AddSourcePreStartFromBytes(LuaManager.GetFromMod(path));
            };
            LuaManager.LUA.Globals["get_language"] = () =>
            {
                return LocalizationManager.CurrentLanguage;
            };


            LuaManager.LUA.Globals["print"] = (string txt) =>
            {
                if (!LuaManager.UnityDebug.Value) Debug.Log(txt);
                Terminal.Log(txt);

            };

            // ----------------------- BUSCAR OBJETOS ----------------------- //
            LuaManager.LUA.Globals["get_object"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Nil;
                var nameObjects = args.GetArray().Select(x => x.String);

                var scene = SceneManager.GetActiveScene();
                GameObject previous = null;
                var notFound = false;
                foreach (var go in nameObjects)
                {
                    if (previous == null && !scene.GetRootGameObjects().Any(x => x.name == go))
                    {
                        notFound = true;
                        break;
                    }
                    if (previous == null) previous = scene.GetRootGameObjects().First(x => x.name == go);
                    else
                    {
                        for (var i = 0; i < previous.transform.childCount; i++)
                        {
                            var next = previous.transform.GetChild(i).gameObject;
                            if (next.name == go)
                            {
                                previous = next;
                                break;
                            }
                        }
                    }
                }
                if (notFound) return DynValue.Nil;
                return LuaFactory.FromGameObject(previous);
            });

            LuaManager.LUA.Globals["find_object"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Nil;

                if (args.Count == 1)
                {
                    var go = GetAllGameObjectsInScene(SceneManager.GetActiveScene()).First(x => x.name == args[0].String);
                    return LuaFactory.FromGameObject(go);
                }

                var gameObject = GetAllGameObjectsInScene(SceneManager.GetSceneByName(args[1].String)).First(x => x.name == args[0].String);
                return LuaFactory.FromGameObject(gameObject);
            });


            LuaManager.LUA.Globals["add_item"] = (DynValue _id, int _cantidad = 1) =>
            {
                if (_id.Type == DataType.Number)
                    UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(Convert.ToInt32(_id.Number), _cantidad);
                else
                {
                    if (_id.String.StartsWith("0x"))
                    {
                        UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(
                            Convert.ToInt32(_id.String.Replace("0x", ""), 16), _cantidad);
                    }
                    else
                    {
                        foreach (var item in FarlandsItems.FarlandsItemsManager.DB.inventoryItems)
                        {
                            if (item.itemName.ToUpper() == _id.String.ToUpper())
                            {
                                UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(item.itemID, _cantidad);
                                break;
                            }
                        }
                    }
                }
            };

            LuaManager.LUA.Globals["add_credits"] = (int _cantidad) =>
            {
                Singleton<FarlandsGameManager>.Instance.persistentDataScript.credits += _cantidad;
                UnityEngine.Object.FindObjectOfType<HUDMoneyScript>().UpdateCredits();
            };

            // ----------------------- CREAR OBJETOS ----------------------- //
            LuaManager.LUA.Globals["create_object"] = (string name) =>
            {
                var go = new GameObject(name);
                go.AddComponent<LuaGameObjectComponent>();
                return LuaFactory.FromGameObject(go);
            };

            LuaManager.LUA.Globals["create_inventory_item"] = (DynValue args) =>
            {
                // string name, string itemType, string spritePath, int buyPrice, int sellPrice, bool canBeStacked, bool canBeDestroyed, float matterPercent

                string name = args.Table.Get("name").String;
                string itemType = args.Table.Get("type").String.ToUpper();
                string spritePath = args.Table.Get("sprite").String;
                int buyPrice = Convert.ToInt32(args.Table.Get("buy_price").Number);
                int sellPrice = Convert.ToInt32(args.Table.Get("sell_price").Number);
                bool canBeStacked = args.Table.Get("stackeable").Boolean;
                bool canBeDestroyed = args.Table.Get("destroyable").Boolean;
                float matterPercent = Convert.ToSingle(args.Table.Get("matter_percent").Number);

                var sprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(spritePath));

                var type = InventoryItem.ItemType.Resource;

                if (itemType == "RESOURCE") type = InventoryItem.ItemType.Resource;
                else if (itemType == "TOOL") type = InventoryItem.ItemType.Tool;
                else if (itemType == "SEED") type = InventoryItem.ItemType.Seed;
                else if (itemType == "CRAFTING") type = InventoryItem.ItemType.Crafting;
                else if (itemType == "FISH") type = InventoryItem.ItemType.Fish;
                else if (itemType == "INSECT") type = InventoryItem.ItemType.Insect;
                else if (itemType == "PLACEABLE") type = InventoryItem.ItemType.Placeable;
                else type = InventoryItem.ItemType.TreeSeed;

                return FarlandsItems.FarlandsItemsManager.AddInventoryItem(new()
                {
                    itemName = name,
                    itemType = type,
                    itemIcon = sprite,
                    canBeDestroyed = canBeDestroyed,
                    canBeStacked = canBeStacked,
                    itemPrice = buyPrice,
                    itemSellPrice = sellPrice,
                    matterPercent = matterPercent
                });
            };


            //TODO agregar múltiple
            LuaManager.LUA.Globals["create_plant"] = (string name, int daysForDeath, int daysForStage, int growSeason,
                 DynValue resources, string seedSprite, string s1Sprite,
                 string s2Sprite, string s3Sprite, string s4Sprite, string s5Sprite) =>
            {

                var plant = new PlantScriptableObject();
                plant.plantName = name;
                plant.daysForDeath = daysForDeath;
                plant.daysForStage = daysForStage;
                plant.growSeason = growSeason;

                var _resources = new List<DBResourceProbability>();


                if (resources.Type == DataType.Number)
                {
                    var res = new DBResourceProbability();
                    res.itemID = Convert.ToInt32(resources.Number);
                    res.item = FarlandsItems.FarlandsItemsManager.DB.GetInventoryItem(res.itemID);
                    res.probabilityList = [new() { amountToSpawn = 1, probability = 100 }];
                    _resources.Add(res);
                }
                else if (resources.Type == DataType.Table)
                {
                    foreach (var t in resources.Table.Values)
                    {
                        DBResourceProbability res = null;

                        if (_resources.Any(x => x.itemID == t.Table.Get("item").Number))
                            res = _resources.First(x => x.itemID == t.Table.Get("item").Number);

                        if (res == null)
                        {
                            res = new DBResourceProbability();
                            res.itemID = Convert.ToInt32(t.Table.Get("item").Number);
                            res.item = FarlandsItems.FarlandsItemsManager.DB.GetInventoryItem(res.itemID);
                            res.probabilityList = [new()
                            {
                                amountToSpawn = Convert.ToInt32(t.Table.Get("amount").Number),
                                probability = Convert.ToInt32(t.Table.Get("prob").Number)
                            }];
                            _resources.Add(res);
                        }
                        else
                        {
                            var rpl = res.probabilityList.ToList();
                            rpl.Add(new()
                            {
                                amountToSpawn = Convert.ToInt32(t.Table.Get("amount").Number),
                                probability = Convert.ToInt32(t.Table.Get("prob").Number)
                            });

                            res.probabilityList = rpl.ToArray();
                        }


                    }
                }

                plant.resourcesList = _resources.ToArray();
                plant.multiple = true;

                plant.seedSprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(seedSprite));
                plant.stage1Sprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(s1Sprite));
                plant.stage2Sprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(s2Sprite));
                plant.stage3Sprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(s3Sprite));
                plant.stage4Sprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(s4Sprite));
                plant.stage5Sprite = SpriteLoader.FromRaw(LuaManager.GetFromMod(s5Sprite));

                return FarlandsItems.FarlandsItemsManager.AddPlant(plant);
            };

            LuaManager.LUA.Globals["create_seed"] = (int inventoryId, List<int> plantsId) =>
            {
                FarlandsItems.FarlandsItemsManager.seeds.Add(new FarlandsItems.FarlandsItemsManager.SeedData()
                {
                    ItemId = inventoryId,
                    PlantsId = plantsId
                });
            };

            LuaManager.LUA.Globals["translate_inventory_item"] = (int id, List<string> name, List<string> description) =>
            {
                FarlandsDialogue.FarlandsDialogueManager.AddInventoryTranslation(id, name, description);
            };

            LuaManager.LUA.Globals["create_scene"] = (string name) =>
            {
                var scene = SceneManager.CreateScene(name);
                //TODO agergar creación del objeto de la escena para LuaManager.LUA
            };

            LuaManager.LUA.Globals["add_command"] = (string name, DynValue LuaFunc, string help) =>
            {
                Action<CommandArg[]> action = (CommandArg[] args) =>
                {
                    var arguments = new List<DynValue>();

                    foreach (var a in args)
                    {
                        if (float.TryParse(a.String, out var floatValue))
                        {
                            Debug.Log("float:" + floatValue);
                            arguments.Add(DynValue.NewNumber(floatValue));
                        }
                        else if (int.TryParse(a.String, out var intValue))
                        {
                            Debug.Log("int:" + intValue);
                            arguments.Add(DynValue.NewNumber(floatValue));
                        }
                        else if (bool.TryParse(a.String, out var boolValue))
                        {
                            Debug.Log("bool:" + boolValue);
                            arguments.Add(DynValue.NewBoolean(boolValue));
                        }
                        else
                        {
                            Debug.Log("str:" + a.String);
                            arguments.Add(DynValue.NewString(a.String));
                        }
                    }

                    LuaManager.LUA.Call(LuaFunc, arguments.ToArray());

                };

                Terminal.Shell.AddCommand(name, action, help: help);
                Terminal.Autocomplete.Register(name);
            };

        }

        static List<GameObject> GetAllGameObjectsInScene(Scene scene)
        {
            // Obtén todos los objetos raíz de la escena
            GameObject[] rootObjects = scene.GetRootGameObjects();
            System.Collections.Generic.List<GameObject> allObjects = new System.Collections.Generic.List<GameObject>();

            // Recorre cada objeto raíz y sus hijos
            foreach (GameObject rootObject in rootObjects)
            {
                allObjects.Add(rootObject);
                AddChildObjects(rootObject.transform, allObjects);
            }

            return allObjects;
        }
        static void AddChildObjects(Transform parent, System.Collections.Generic.List<GameObject> allObjects)
        {
            foreach (Transform child in parent)
            {
                allObjects.Add(child.gameObject);
                AddChildObjects(child, allObjects);
            }
        }
    }
}
