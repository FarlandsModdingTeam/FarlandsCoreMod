using Farlands.Inventory;
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


namespace FarlandsCoreMod.FarlandsConsole
{
    public static class LuaGameObject
    {
        // ------------------- Variables ------------------- //
        // private static DynValue updateFunction;
        // private static DynValue startFunction;


        /* Componentes y/o propiedades que se pueden acceder
         * Escribir/Editar
         * Leer
         * Eliminar
        */
        public static DynValue FromGameObject(GameObject gameObject)
        {
            if (gameObject == null)
                return DynValue.Nil;

            DynValue result = DynValue.NewTable(new Table(Manager.LUA));

            //TODO hacer que se agreguen al luaGo los componentes que ya tenga

            // añadir componente
            result.Table.Set("add_component", DynValue.NewCallback((ctx, args) =>
            {
                if (args == null)
                    return DynValue.Void;


                Debug.Log("Usted ah ejecutado 'add_component' sin plomo 95");

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


                // Añadir propiedades
                if (_propiedades != null)
                {
                    foreach (var _Pares in _propiedades.Table.Pairs)
                    {
                        string propertyName = _Pares.Key.String;
                        DynValue propertyValue = _Pares.Value;


                        // Busca una propiedad con propertyName en el componente (Saca la pripiedad)
                        PropertyInfo propertyInfo = _componentType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null && propertyInfo.CanWrite && propertyValue != null)
                        {
                            object value = null;


                            if (propertyInfo.PropertyType == typeof(int))
                                value = (int)propertyValue.Number;

                            else if (propertyInfo.PropertyType == typeof(float))
                                value = (float)propertyValue.Number;

                            else if (propertyInfo.PropertyType == typeof(bool))
                                value = propertyValue.Boolean;

                            else if (propertyInfo.PropertyType == typeof(string))
                                value = propertyValue.String;

                            else if (propertyInfo.PropertyType == typeof(Sprite))
                                value = SpriteLoader.FromRaw(Manager.GetFromMod(propertyValue.String));
                            
                            else
                                value = Convert.ChangeType(propertyValue.ToObject(), propertyInfo.PropertyType);
                            
                            propertyInfo.SetValue(_componente, value);
                        }
                    }
                }


                // Funciona (se supone)
                if (_propiedades != null)
                {
                    Debug.Log("Devolviendo propiedades");

                    Table _resultado = new Table(Manager.LUA);
                    foreach (var _i in _propiedades.Table.Pairs)
                    {
                        PropertyInfo propertyInfo = _componentType.GetProperty(_i.Key.String, BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null)
                        {
                            _resultado.Set(_i.Key.String, DynValue.NewString(propertyInfo.GetValue(_componente).ToString()));
                        }
                    }
                    return DynValue.NewTable(_resultado);
                }
                else
                    return DynValue.Void;

            }));


            result.Table.Set("get_name", DynValue.NewCallback((ctx, args) =>
            {
                return DynValue.NewString(gameObject.name);
            }));
            // X, Y , Z
            result.Table.Set("set_position", DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Void;
                var nameObjects = args.GetArray().Select(x => x.String);

                var position = gameObject.transform.position;
                // Modificar el transform

                var addition = new Vector3(
                    args.Count < 1 ? (float)args[0].Number : 0f,
                    args.Count < 2 ? (float)args[1].Number : 0f,
                    args.Count < 3 ? (float)args[2].Number : 0f);

                gameObject.transform.position = addition;

                return DynValue.Void;
            }));
            result.Table.Set("get_position", DynValue.NewCallback((ctx, args) =>
            {
                var position = gameObject.transform.position;

                DynValue _r = DynValue.NewTable(new Table(Manager.LUA));
                _r.Table.Set("x", DynValue.NewNumber(position.x));
                _r.Table.Set("y", DynValue.NewNumber(position.y));
                _r.Table.Set("z", DynValue.NewNumber(position.z));

                return _r;
            }));

            // X, Y , Z
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

            result.Table.Set("set_scale", DynValue.NewCallback((ctx, args) =>
            {

                if (args.Count == 1) gameObject.transform.localScale = new((float)args[0].Number, (float)args[0].Number, 1);
                else if (args.Count == 2) gameObject.transform.localScale = new((float)args[0].Number, (float)args[1].Number, 1);
                else if (args.Count == 3) gameObject.transform.localScale = new((float)args[0].Number, (float)args[1].Number, (float)args[2].Number);

                return DynValue.Void;
            }));

            result.Table.Set("toggle_active", DynValue.NewCallback((ctx, args) =>
            {
                gameObject.SetActive(!gameObject.activeSelf);
                return DynValue.Void;
            }));

            //result.Table.Set("add_component", DynValue.NewCallback((ctx, args) =>
            //{
            //    if (args != null || args.Count < 1)
            //        return DynValue.Void;

            //    foreach (DynValue arg in args.GetArray())
            //    { 
            //        if(arg.String == "image") gameObject.AddComponent<Image>();
            //        if(arg.String == "sprite") gameObject.AddComponent<SpriteRenderer>();
            //        if (arg.String == "camera") gameObject.AddComponent<Camera>();
            //    }

            //    return DynValue.Void;
            //}));

            if (gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                result.Table.Set("set_sprite", DynValue.NewCallback((ctx, args) =>
                {
                    var path = args[0].String;

                    var raw = Manager.GetFromMod(path);
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

                    var raw = Manager.GetFromMod(path);
                    var texture = new Texture2D(1, 1);

                    texture.LoadImage(raw);
                    texture.filterMode = FilterMode.Point;

                    image.sprite = SpriteLoader.FromTexture(texture);

                    return DynValue.Void;
                }));
            }
            if (gameObject.TryGetComponent<SeedSelector>(out var seedSelector))
            {
                result.Table.Set("seed_selector", DynValue.NewTable(new Table(Manager.LUA)));
                var _seedSelector = result.Table.Get("seed_selector");

                _seedSelector.Table.Set("get_instance", DynValue.NewCallback((ctx, args) =>
                {
                    if (args.Count > 0 && args[0].Type == DataType.Number)
                    {
                        var list = Private.GetFieldValue<List<EquippableData>>(seedSelector, "seedInstances");
                        foreach (EquippableData seedInstance in list)
                        {
                            if (Convert.ToInt32(args[0].Number) == seedInstance.itemID)
                                return LuaGameObject.FromGameObject(seedInstance.instance);
                            
                        }
                    }

                    return DynValue.Nil;
                }));
            }

            if (!gameObject.TryGetComponent<scriptGenerico>(out var _Ficha))
            {
                _Ficha = gameObject.AddComponent<scriptGenerico>();
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


    }
}
