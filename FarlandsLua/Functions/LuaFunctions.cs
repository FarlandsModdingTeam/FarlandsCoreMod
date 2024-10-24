using CommandTerminal;
using Farlands;
using Farlands.DataBase;
using Farlands.Dev;
using Farlands.Inventory;
using Farlands.PlantSystem;
using Farlands.UsableItems;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.FarlandsLua;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using I2.Loc;
using JanduSoft;
using Language.Lua;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Articy.Articy_1_4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.SceneManagement;
using static PixelCrushers.DialogueSystem.UnityGUI.GUIProgressBar;
using static System.Collections.Specialized.BitVector32;
using Debug = UnityEngine.Debug;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    /// <summary>
    /// **Clase** donde guardamos las funciones LUA   
    /// > ME CAGO EN MI PUTA VIDA 
    /// </summary>
    public static class LuaFunctions
    {
        [Functions("scenes")]
        public static class FunctionsScenes
        {

            /// <summary>
            /// Carga la escena especificada por nombre o índice.  
            /// <param name="scene">puede ser el nombre o indice</param>
            /// </summary>
            public static void load_scene(DynValue scene)
            {
                if (scene.Type == DataType.String)
                    SceneManager.LoadScene(scene.String);
                else if (scene.Type == DataType.Number)
                    SceneManager.LoadScene(scene.Integer());
            }

            public static void print_scene()
            {
                Scene currentScene = SceneManager.GetActiveScene();
                Terminal.Log($"({currentScene.buildIndex}) {currentScene.name}");
            }

        }

        [Functions]
        public static class GlobalFunctions
        {
            /// <summary>
            /// TODO: hacer
            /// </summary>
            /// <param name="tag">Nombre/Identificador del MOD</param>
            public static void MOD(string tag)
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
            }

            /// <summary>
            /// descrcion basica TODO: hacer luego
            /// </summary>
            /// <example>
            /// > _mod_.config = guarda las secciones que haya  
            /// > _mod_.config guarda la configuracion del mod  
            /// > if -> si la configuracion es un booleano  
            /// >   agremaos la entra a la configuracion  
            /// >   Cojes el mod en LUA  
            /// >   Cojer la sepcion  
            /// >   Cojer la clave que devulva el valor  
            /// </example>
            /// <param name="section">La seccion de la configuracion, puede haber varias</param>
            /// <param name="key">codigo de la configuracion</param>
            /// <param name="def">valor de la configuracion</param>
            /// <param name="description">descripcion de ella</param>
            public static void config(string section, string key, DynValue def, string description)
            {
                var code =
        @$"
_mod_.config = _mod_.config or {{}}
_mod_.config.{section} = _mod_.config.{section} or {{}}
";
                LuaManager.Execute(code, null); //TODO: agregar todas las posibilidades de configuracion

                if (def.Type == DataType.Boolean)
                {
                    var entry = LuaManager.CURRENT_MOD.ConfigFile.Bind(section, key, def.Boolean, description);

                    LuaManager.LUA.Globals.Get("_mod_")
                        .Table.Get("config")
                        .Table.Get(section)
                        .Table.Set(key, DynValue.NewCallback((ctx, args) => DynValue.NewBoolean(entry.Value)));
                }

            }

            /// <summary>
            /// Devuelve si as pulsado el boton o tecla especificada  
            /// </summary>
            /// <param name="input">Input esperado</param>
            /// <example>
            /// > Obtiene el player
            /// > Si el input especificado está pulsado
            /// </example>
            public static bool get_input(string input)
            {
                var player = GameObject.FindObjectOfType<PlayerController>();
                return player.player.GetButtonDown(input) && player.inputEnabled;
            }


            /// <summary>
            /// TODO
            /// </summary>
            /// <param name="_comando">Comando a ejecutar</param>
            /// <example>
            /// > Si no existe el objeto de comandos  
            /// > Obtiene la lista de comandos  
            /// > Divide el comando en palabras  
            /// > Recorre la lista de comandos  
            /// > Si el comando es igual al _comando  
            /// > Codigo Original :)
            /// </example>
            public static void execute_command(string _comando)
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
            }

            /// <summary>
            /// TODO
            /// </summary>
            /// <example>
            /// > Si no existe el objeto de comandos  
            /// > si un parametro, el nombre del archivo es la textura que va ah remplazar (no recomendado)  
            /// > si dos parametros, el primero es el nombre de la textura que va ah remplazar y el segundo es la ruta de la textura  
            /// </example>
            /// <param name="arg0"></param>
            /// <param name="arg1"></param>
            public static void texture_override(DynValue arg0, DynValue arg1)
            {
                if (arg0.IsNil()) throw new Exception("Invalid args for TextureOverride");
                else if (arg1.IsNil())
                {
                    var path = arg0.String;
                    TexturesModifier.ReplaceTexture(Path.GetFileNameWithoutExtension(path), LuaManager.GetFromMod(path));
                }
                else
                {
                    var origin = arg0.String;
                    var path = arg1.String;
                    TexturesModifier.ReplaceTexture(origin, LuaManager.GetFromMod(path));
                }
            }

            // TODO: no se recomienda su uso, a lo mejor se quita
            public static void texture_override_in(string path)
            {
                Debug.Log(string.Join('\n', LuaManager.GetFilesInMod(path)));

                foreach (var item in LuaManager.GetFilesInMod(path))
                    TexturesModifier.ReplaceTexture(Path.GetFileNameWithoutExtension(item), LuaManager.GetFromMod(item));

            }

            // TODO: No funciona, cremo
            /// <summary>
            /// En vez de remplaza una region de la textura  
            /// </summary>
            /// <param name="origin">textura que se va a remplazar</param>
            /// <param name="position">posicion inicial(abajo -> arriba, izquierda -> derecha)</param>
            /// <param name="path">direccion del archivo</param>
            public static void sprite_override(string origin, int[] position, string path)
            {
                var vec = new Vector2Int(position[0], position[1]);
                TexturesModifier.ReplaceSprite(origin, vec, LuaManager.GetFromMod(path));
            }

            /// <summary>
            /// Es un **texture_override** pero con un personaje  
            /// </summary>
            /// <example>
            /// > Crea una función en LUA que se ejecuta cuando se llama a la función  
            /// > La función llama a **texture_override** con los parametros que se le pasan  
            /// > **texture_override** remplaza la textura  
            /// </example>
            /// <param name="origin">portrait a reemplazar</param>
            /// <param name="path">dirección del archivo</param>
            public static void portrait_override(string origin, string path)
            {
                string code =
@$"
function _mod_.event.dialogue.portrait:{origin}()
    texture_override('{origin}', '{path}')
end
";
                LuaManager.LUA.DoString(code);
            }

            /// <summary>
            /// Se llama add_language, ¿Que cres que va hah hacer?    
            /// </summary>
            /// <param name="path">direccion del json</param>
            public static void add_language(string path) => FarlandsDialogue.FarlandsDialogueManager.AddSourcePreStartFromBytes(LuaManager.GetFromMod(path));

            public static string get_language() => LocalizationManager.CurrentLanguage;

            /// <summary>
            /// printea el texto  
            /// </summary>
            /// <param name="txt">lo que le pases lo dibuja por consola</param>
            public static void print(string txt)
            {
                if (!LuaManager.UnityDebug.Value) Debug.Log(txt);
                Terminal.Log(txt);
            }

            /// <summary>
            /// te da un objeto dada la ruta de los objetos  
            /// </summary>
            /// <param name="args">ruta en gameObject</param>
            public static DynValue get_object(List<DynValue>  args)
            {
                if (args.Count() < 1) return DynValue.Nil;

                var scene = SceneManager.GetActiveScene();
                GameObject previous = null;
                var notFound = false;
                foreach (var go in args)
                {
                    if (previous == null && !scene.GetRootGameObjects().Any(x => x.name == go.String))
                    {
                        notFound = true;
                        break;
                    }
                    if (previous == null) previous = scene.GetRootGameObjects().First(x => x.name == go.String);
                    else
                    {
                        for (var i = 0; i < previous.transform.childCount; i++)
                        {
                            var next = previous.transform.GetChild(i).gameObject;
                            if (next.name == go.String)
                            {
                                previous = next;
                                break;
                            }
                        }
                    }
                }
                if (notFound) return DynValue.Nil;
                return LuaFactory.FromGameObject(previous);
            }

            /// <summary>
            /// Te busca un gameObject en la escena por el nombre (cuidado con objeton con el mismo nombre)  
            /// </summary>
            /// <param name="args">nombre del objeto</param>
            public static DynValue find_object(string name, string scene)
            {
                if (name == null) return DynValue.Nil;

                if (scene == null)
                {
                    var go = GetAllGameObjectsInScene(SceneManager.GetActiveScene()).First(x => x.name == name);
                    return LuaFactory.FromGameObject(go);
                }

                var gameObject = GetAllGameObjectsInScene(SceneManager.GetSceneByName(scene)).First(x => x.name == name);
                return LuaFactory.FromGameObject(gameObject);
            }

            /// <summary>
            ///   
            /// > si el id es un numero da el objeto por el id  
            /// > si ex 0x es un hexadecimal  
            /// > si es un string busca el objeto por el nombre  
            /// </summary>
            /// <param name="id">Id del objeto, sirve (nombre (creo), id, id en exadecimal)</param>
            /// <param name="amount">Cantidad a añadir (no opcional)</param>
            public static void add_item(DynValue id, int amount)
            {
                if (id.Type == DataType.Number)
                    UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(id.Integer(), amount);
                else
                {
                    if (id.String.StartsWith("0x"))
                    {
                        UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(
                            Convert.ToInt32(id.String.Replace("0x", ""), 16),
                            amount);
                    }
                    else
                    {
                        foreach (var item in FarlandsItems.FarlandsItemsManager.DB.inventoryItems)
                        {
                            if (item.itemName.ToUpper() == id.String.ToUpper())
                            {
                                UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(item.itemID, amount);
                                break;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Añade los creidtos especificados  
            /// </summary>
            /// <param name="amount">Cantidad añadir</param>
            public static void add_credits(int amount)
            {
                Singleton<FarlandsGameManager>.Instance.persistentDataScript.credits += amount;
                UnityEngine.Object.FindObjectOfType<HUDMoneyScript>().UpdateCredits();
            }

            /// <summary>
            /// Crea un nuevo gameObject con el nombre especificado  
            /// </summary>
            /// <param name="name">nombre del gameObject a crear</param>
            public static DynValue create_object(string name)
            {
                var go = new GameObject(name);
                go.AddComponent<LuaGameObjectComponent>();
                return LuaFactory.FromGameObject(go);
            }

            /// <summary>
            /// Añade a la lista un objeto de inventario  
            /// > Crea un nuevo objeto de inventario  
            /// > Si  
            /// </summary>
            /// <param name="args">name->string, type->string, sprite->string, buy_price->int, sell_price->int, stackeable->bool, matter_percent->float</param>
            public static int create_inventory_item(DynValue args)
            {
                string name = args.Table.Get("name").String;
                string itemType = args.Table.Get("type").String.ToUpper();
                string spritePath = args.Table.Get("sprite").String;
                int buyPrice = args.Table.Get("buy_price").Integer();
                int sellPrice = args.Table.Get("sell_price").Integer();
                bool canBeStacked = args.Table.Get("stackeable").Boolean;
                bool canBeDestroyed = args.Table.Get("destroyable").Boolean;
                float matterPercent = args.Table.Get("matter_percent").Float();

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
            }

            //TODO agregar múltiple
            /// <summary>
            /// agrega a una tabla un objeto de planta  
            /// > algomas  
            /// > genera la planta  
            /// >   
            /// > agrega la planta  
            /// > devuelve el id  
            /// </summary>
            /// <param name="args">
            /// name->string, days_for_death->int, days_for_stage->int, grow_season->int, resources->table->(item->int, algomas->X, algomas->X), seed->string, stage_1->string, stage_2->string, stage_3->string, stage_4->string, stage_5->string
            /// </param> 
            public static int create_plant(DynValue args)
            {
                string name = args.Table.Get("name").String;
                int daysForDeath = args.Table.Get("days_for_death").Integer();
                int daysForStage = args.Table.Get("days_for_stage").Integer();
                int growSeason = args.Table.Get("grow_season").Integer();
                DynValue resources = args.Table.Get("resources");
                string seedSprite = args.Table.Get("seed").String;
                string s1Sprite = args.Table.Get("stage_1").String;
                string s2Sprite = args.Table.Get("stage_2").String;
                string s3Sprite = args.Table.Get("stage_3").String;
                string s4Sprite = args.Table.Get("stage_4").String;
                string s5Sprite = args.Table.Get("stage_5").String;

                var plant = new PlantScriptableObject();
                plant.plantName = name;
                plant.daysForDeath = daysForDeath;
                plant.daysForStage = daysForStage;
                plant.growSeason = growSeason;

                var _resources = new List<DBResourceProbability>();


                if (resources.Type == DataType.Number)
                {
                    var res = new DBResourceProbability();
                    res.itemID = resources.Integer();
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
            }

            /// Crear una semilla
            /// </summary>
            /// <param name="inventoryId">id de la planta</param>
            /// <param name="plantsId">id del inventory Item</param>
            public static void create_seed(int inventoryId, List<DynValue> plantsId)
            {
                FarlandsItems.FarlandsItemsManager.seeds.Add(new FarlandsItems.FarlandsItemsManager.SeedData()
                {
                    ItemId = inventoryId,
                    PlantsId = plantsId.Select(x => x.Integer()).ToList()
                });
            }

            /// <summary>
            /// Traducion del objeto de inventario
            /// </summary>
            /// <param name="id">id del objeto</param>
            /// <param name="name">nombres</param>
            /// <param name="description">descripciones</param>
            public static void translate_inventory_item(int id, List<DynValue> name, List<DynValue> description)
            {
                FarlandsDialogue.FarlandsDialogueManager.AddInventoryTranslation(
                    id, 
                    name.Select(x=>x.String).ToList(),
                    description.Select(x => x.String).ToList());
            }

            /// <summary>
            /// Crea una escena con el nombre
            /// </summary>
            /// <param name="name">nombre de la escena</param>
            public static void create_scene(string name)
            {
                var scene = SceneManager.CreateScene(name);
            }

            /// <summary>
            /// añade un comando a la terminal  
            /// > sufrimiento TODO  
            /// </summary>
            /// <param name="name">nombre del comando</param>
            /// <param name="LuaFunc">funcion lua</param>
            /// <param name="help">texto del help de help</param>
            public static void add_command(string name, DynValue LuaFunc, string help)
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
            }

            /// <summary>
            /// TODO agregar los nuevos
            /// </summary>
            public static DynValue rayCast(Vector3 origen, Vector3 direction, float max, int _mascara, UserData user)
            {
                Debug.Log("Usted ah ejecutado 'rayCast' sin plomo 95");

                RaycastHit2D hit = Physics2D.Raycast(origen, direction, max); //, _mascara
                IToolInteraction component = hit.collider.GetComponent<IToolInteraction>();
                if (hit.collider != null)
                {
                    Debug.Log($"rayCast: Colisión detectada con {hit.collider.gameObject.name} a una distancia de {hit.distance} unidades.");

                    // Si el Raycast colisiona con un objeto, devolver información sobre el objeto
                    var hitInfo = new Table(LuaManager.LUA);
                    hitInfo["collider"] = LuaFactory.FromGameObject(hit.collider.gameObject);
                    hitInfo["point"] = LuaConverter.ToLua(hit.point);
                    hitInfo["normal"] = LuaConverter.ToLua(hit.normal);
                    hitInfo["distance"] = DynValue.NewNumber(hit.distance);

                    component.ToolHit(5, PlayerTool.PICKAXE, 5, GameObject.FindObjectOfType<PlayerController>());

                    return DynValue.NewTable(hitInfo);
                }

                Debug.Log("rayCast: No se detectó ninguna colisión.");
                return DynValue.Nil;
            }

            public static int get_layer_number(string layerName)
            {
                int layerNumber = LayerMask.NameToLayer(layerName);
                if (layerNumber == -1)
                {
                    Debug.LogWarning($"Layer '{layerName}' no encontrado.");
                }
                return layerNumber;
            }

            public static DynValue CSF(string assembly, string className, string method)
            {
                List<Assembly> asm = [];

                foreach (var x in asm)
                {
                    var t = x.GetType(className);
                    if (t != null)
                    {
                        var m = t.GetMethod(method);
                        if (m != null)
                        {
                            return DynValue.NewCallback((ctx, args) =>
                                LuaConverter.ToLua(
                                    m.Invoke(null, LuaConverter.CallbackArgumentToObjectArray(args, 
                                        m.GetParameters().Select(p=>p.ParameterType).ToList()))));
                        }
                    }
                }

                return DynValue.Nil;

            }

        }

        [AttributeUsage(AttributeTargets.Class)]
        public class Functions : Attribute
        {
            public string path;
            public Functions()
            {
                path = "";
            }
            public Functions(string path)
            {
                this.path = path;
            }
        }
        public static void AddToLua()
        {
            mathsFuncions();

            var metadata = new LuaMetadata();
            metadata.Add(new() { type = LuaMetadata.Type.META, values = "farlands" });

            typeof(LuaFunctions)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.GetCustomAttributes<Functions>().Count() >= 1)
                .ToList()
                .ForEach(x => {
                    Table t = LuaManager.LUA.Globals;
                    var stroke = x.GetCustomAttribute<Functions>().path.Split('.');
                    string parent = "";
                    foreach (var str in stroke)
                    {
                        if (str == "") break;

                        var name = str.Trim().ToLower();

                        if (t.Get(name).Table == null) t[name] = new Table(LuaManager.LUA);
                        t = t.Get(name).Table;

                        metadata.AddClass(name);

                        parent = name;
                    }
                    x.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .ToList().ForEach(m =>
                    {
                        var types = m.GetParameters().Select(p =>
                        {
                            if(parent == "") metadata.AddParam(p.Name, p.ParameterType);
                            return p.ParameterType;
                        }).ToList();
                        if (parent == "")
                        {
                            if(typeof(void).IsAssignableFrom(m.ReturnType)) metadata.AddReturn(m.ReturnType);
                            metadata.AddFunction(m.Name, string.Join(',', m.GetParameters().Select(p=>p.Name)));
                        }
                        else
                        {
                            metadata.AddFieldFun(m.Name, string.Join(',',m.GetParameters().Select(p=>$"{p.Name}:{p.ParameterType}")), m.ReturnType);
                        }
                        Debug.Log(m.Name);

                        t[m.Name] = DynValue.NewCallback((ctx, args) =>
                        {
                            Debug.Log($"Calling Lua: {m.Name}");
                            var array = LuaConverter.CallbackArgumentToObjectArray(args, types).ToList();

                            while (array.Count() > m.GetParameters().Count())
                            {
                                array.RemoveAt(array.Count() - 1);
                            }

                            while (array.Count() < m.GetParameters().Count())
                            { 
                                array.Add(null);
                            }

                            var res = m.Invoke(null, array.ToArray());
                            return LuaConverter.ToLua(res);
                        });
                    });

                    metadata.AddCode($"{parent} = {{}}");
                });

            File.WriteAllText(
                    Path.Combine(Paths.Plugin, "metadata.lua"),
                    metadata.ToString()
                );
            // ----------------------- BUSCAR OBJETOS ----------------------- //
        }

        /// <summary>
        /// MATENME
        /// </summary>
        private static void mathsFuncions()
        {
            var math = LuaManager.LUA.Globals.Get("math").Table;
            math.Set("normalize", DynValue.NewCallback((ctx, args) =>
            {
                if(args.Count < 0) return DynValue.Nil;
                var v = args[0].Table;
                float x = 0f;
                float y = 0f;
                float z = 0f;
                if (v.Get("x") != DynValue.Nil) x = Convert.ToSingle(v.Get("x").Number);
                if (v.Get("y") != DynValue.Nil) y = Convert.ToSingle(v.Get("y").Number);
                if (v.Get("z") != DynValue.Nil) z = Convert.ToSingle(v.Get("z").Number);

                var vector = new Vector3(x, y, z).normalized;
                return LuaConverter.ToLua(vector);
            }));
        }

        /// <summary>
        ///  MATENME
        /// </summary>
        /// <param name="scene">la escena necesaria</param>
        /// <returns>allObjects</returns>
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

        /// <summary>
        /// MATENME
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="allObjects"></param>
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