using Farlands.Inventory;
using FarlandsCoreMod.FarlandsLua;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ChatMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace FarlandsCoreMod.FarlandsConsole.Functions
{
    public static class LuaFactory
    {
        // ------------------- Variables ------------------- //
        // private static DynValue updateFunction;
        // private static DynValue startFunction;


        /* Componentes y/o propiedades que se pueden acceder
         * Escribir/Editar
         * Leer
         * Eliminar
        */
        public static DynValue FromObject(object @object)
        {
            DynValue result = DynValue.NewTable(new Table(LuaManager.LUA));

            result.Table.Set("get_field", DynValue.NewCallback((ctx, args) =>
            {
                // TODO

                return DynValue.Nil;
            }));
            result.Table.Set("set_field", DynValue.NewCallback((ctx, args) =>
            {
                // TODO

                return DynValue.Nil;
            }));
            result.Table.Set("set_property", DynValue.NewCallback((ctx, args) =>
            {
                // TODO

                return DynValue.Nil;
            }));
            result.Table.Set("get_property", DynValue.NewCallback((ctx, args) =>
            {
                // TODO

                return DynValue.Nil;
            }));
            result.Table.Set("call", DynValue.NewCallback((ctx, args) =>
            {
                // TODO

                return DynValue.Nil;
            }));

            return result;
        }

        public static DynValue FromComponent(Component component)
        {
            DynValue result = DynValue.NewTable(new Table(LuaManager.LUA));
            result = FromObject(result);

            // Devuelve el gameObject de este componente
            result.Table.Set("game_object", DynValue.NewCallback((ctx, args) =>
            {
                return FromGameObject(component.gameObject);
            }));

            return result;
        }

        public static DynValue FromGameObject(GameObject gameObject)
        {
            if (gameObject == null)
                return DynValue.Nil;

            DynValue result = DynValue.NewTable(new Table(LuaManager.LUA));

            // Sirve para obtener el nombre del GameObject
            result.Table.Set("get_name", DynValue.NewCallback((ctx, args) =>
            {
                return DynValue.NewString(gameObject.name);
            }));

            // Sirve para obtener la posición del GameObject
            result.Table.Set("get_position", DynValue.NewCallback((ctx, args) =>
            {
                var position = gameObject.transform.position;

                DynValue _r = DynValue.NewTable(new Table(LuaManager.LUA));
                _r.Table.Set("x", DynValue.NewNumber(position.x));
                _r.Table.Set("y", DynValue.NewNumber(position.y));
                _r.Table.Set("z", DynValue.NewNumber(position.z));

                return _r;
            }));

            // Sirve para modificar la posición del GameObject (x, y, z)
            result.Table.Set("set_position", DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Void;
                var nameObjects = args.GetArray().Select(x => x.String);

                var position = gameObject.transform.position;
                // Modificar el transform

                var addition = new Vector3(
                    args.Count >= 1 ? (float)args[0].Number : 0f, // x
                    args.Count >= 2 ? (float)args[1].Number : 0f, // y
                    args.Count >= 3 ? (float)args[2].Number : 0f); // z

                gameObject.transform.position = addition;

                return DynValue.Void;
            }));

            // Sirve para agregar posición al GameObject (x, y, z)
            result.Table.Set("add_position", DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Void;
                var nameObjects = args.GetArray().Select(x => x.String);

                var position = gameObject.transform.position;
                // Modificar el transform

                var addition = new Vector3(
                    args.Count >= 1 ? (float)args[0].Number : 0f,
                    args.Count >= 2 ? (float)args[1].Number : 0f,
                    args.Count >= 3 ? (float)args[2].Number : 0f);

                Debug.Log(gameObject.transform.position);
                gameObject.transform.position += addition;
                Debug.Log(gameObject.transform.position);
                return DynValue.Void;
            }));

            // Cambia el tamaño de un GameObject (x,y,z)
            result.Table.Set("set_scale", DynValue.NewCallback((ctx, args) =>
            {

                if (args.Count == 1) gameObject.transform.localScale = new((float)args[0].Number, (float)args[0].Number, 1);
                else if (args.Count == 2) gameObject.transform.localScale = new((float)args[0].Number, (float)args[1].Number, 1);
                else if (args.Count == 3) gameObject.transform.localScale = new((float)args[0].Number, (float)args[1].Number, (float)args[2].Number);

                return DynValue.Void;
            }));

            // Hace que el estado de activación de un GameObject cambie de valor
            result.Table.Set("toggle_active", DynValue.NewCallback((ctx, args) =>
            {
                gameObject.SetActive(!gameObject.activeSelf);
                return DynValue.Void;
            }));

            //TODO: Modificación completa de la lógica
            result.Table.Set("get_component", DynValue.NewCallback((ctx, args) =>
            {
                if (args == null)
                    return DynValue.Void;

                Debug.Log("Usted ah ejecutado 'get_component' sin plomo 95");

                string _nombreComponente = ""; // = args[0].String;
                DynValue _propiedades = null; // = args.Count > 1 ? args[1] : null;
                Type _componentType = null;

                // tu dices que es inicesario, yo digo que me la pela
                foreach (DynValue arg in args.GetArray())
                {
                    if (arg.Type == DataType.String)
                        _nombreComponente = arg.String;

                    if (arg.Type == DataType.Table)
                        _propiedades = arg;
                }

                foreach (var t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()))
                {
                    if (t.Name == _nombreComponente)
                    {
                        _componentType = t;
                        break;
                    }
                }

                if (_componentType == null || !typeof(Component).IsAssignableFrom(_componentType))
                {
                    Debug.LogError("El tipo especificado no es un componente válido: " + _nombreComponente);
                    return DynValue.Void;
                }

                Component _componente = gameObject.GetComponent(_componentType);
                if (_componente == null)
                    _componente = gameObject.AddComponent(_componentType);

                return FromComponent(_componente);

            }));


            // Funciones específicas

            if (gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                result.Table.Set("set_sprite", DynValue.NewCallback((ctx, args) =>
                {
                    var path = args[0].String;

                    var raw = LuaManager.GetFromMod(path);
                    var texture = new Texture2D(1, 1);

                    texture.LoadImage(raw);
                    texture.filterMode = FilterMode.Point;

                    spriteRenderer.sprite = SpriteLoader.FromTexture(texture);

                    return DynValue.Void;
                }));
            }
            if (gameObject.TryGetComponent<Image>(out var image))
            {
                result.Table.Set("set_sprite", DynValue.NewCallback((ctx, args) =>
                {
                    var path = args[0].String;

                    var raw = LuaManager.GetFromMod(path);
                    var texture = new Texture2D(1, 1);

                    texture.LoadImage(raw);
                    texture.filterMode = FilterMode.Point;

                    image.sprite = SpriteLoader.FromTexture(texture);

                    return DynValue.Void;
                }));
            }
            if (gameObject.TryGetComponent<SeedSelector>(out var seedSelector))
            {
                result.Table.Set("seed_selector", DynValue.NewTable(new Table(LuaManager.LUA)));
                var _seedSelector = result.Table.Get("seed_selector");

                _seedSelector.Table.Set("get_instance", DynValue.NewCallback((ctx, args) =>
                {
                    if (args.Count > 0 && args[0].Type == DataType.Number)
                    {
                        var list = Private.GetFieldValue<List<EquippableData>>(seedSelector, "seedInstances");
                        foreach (EquippableData seedInstance in list)
                        {
                            if (Convert.ToInt32(args[0].Number) == seedInstance.itemID)
                                return FromGameObject(seedInstance.instance);

                        }
                    }

                    return DynValue.Nil;
                }));
            }

            // Definicion para start y update

            if (!gameObject.TryGetComponent<LuaGameObjectComponent>(out var _Ficha))
            {
                _Ficha = gameObject.AddComponent<LuaGameObjectComponent>();
            }
            _Ficha.Result = result;
            result.Table.Set("set_update", DynValue.NewCallback((ctx, args) =>
            {
                result.Table.Set("Update", args[0]);
                return DynValue.Void;
            }));
            result.Table.Set("set_start", DynValue.NewCallback((ctx, args) =>
            {
                result.Table.Set("Start", args[0]);
                return DynValue.Void;
            }));

            return result;
        }

        // ConvertLuaArgumentToCSharp
        private static object ConvertLuaArgumentToCSharp(DynValue luaArg, Type targetType)
        {
            if (targetType == typeof(int))
                return (int)luaArg.Number;

            else if (targetType == typeof(float))
                return (float)luaArg.Number;

            else if (targetType == typeof(bool))
                return luaArg.Boolean;

            else if (targetType == typeof(string))
                return luaArg.String;

            else if (targetType == typeof(Vector2))
            {
                var table = luaArg.Table;
                float x = table.Get("x").Type == DataType.Number ? (float)table.Get("x").Number : 0f;
                float y = table.Get("y").Type == DataType.Number ? (float)table.Get("y").Number : 0f;
                return new Vector2(x, y);
            }

            else
                return Convert.ChangeType(luaArg.ToObject(), targetType);
        }

        // ConvertCSharpArgumentToLua
        private static DynValue ConvertToLua(object csharpArg)
        {
            if (csharpArg == null)
                return DynValue.Nil;

            Type type = csharpArg.GetType();

            if (type == typeof(int))
                return DynValue.NewNumber((int)csharpArg);

            else if (type == typeof(float))
                return DynValue.NewNumber((float)csharpArg);

            else if (type == typeof(bool))
                return DynValue.NewBoolean((bool)csharpArg);

            else if (type == typeof(string))
                return DynValue.NewString((string)csharpArg);

            else if (type == typeof(Vector2))
            {
                Vector2 vector = (Vector2)csharpArg;
                Table table = new Table(LuaManager.LUA);
                table.Set("x", DynValue.NewNumber(vector.x));
                table.Set("y", DynValue.NewNumber(vector.y));
                return DynValue.NewTable(table);
            }
            else if (type == typeof(Vector3))
            {
                Vector3 vector = (Vector3)csharpArg;
                Table table = new Table(LuaManager.LUA);
                table.Set("x", DynValue.NewNumber(vector.x));
                table.Set("y", DynValue.NewNumber(vector.y));
                table.Set("z", DynValue.NewNumber(vector.z));
                return DynValue.NewTable(table);
            }
            else
            {
                return DynValue.FromObject(LuaManager.LUA, csharpArg);
            }
        }

    }
}
