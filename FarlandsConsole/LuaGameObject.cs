using FarlandsCoreMod.Utiles.Loaders;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using System;
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


        public static DynValue FromGameObject(GameObject gameObject)
        {
            DynValue result = DynValue.NewTable(new Table(Manager.LUA));

            //TODO hacer que se agreguen al luaGo los componentes que ya tenga

            // añadir componente
            result.Table.Set("add_component", DynValue.NewCallback((ctx, args) =>
            {
                if (args == null || args.Count < 1)
                    return DynValue.Void;

                string componentName = args[0].String;
                DynValue properties = args.Count > 1 ? args[1] : null;


                Type componentType = null;

                foreach (var t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()))
                {
                    if (t.Name == componentName)
                    {
                        componentType = t;
                        break;
                    }
                }

                if (componentType == null || !typeof(Component).IsAssignableFrom(componentType))
                {
                    Debug.LogError("El tipo especificado no es un componente válido: " + componentName);
                    return DynValue.Void;
                }

                Component component = gameObject.AddComponent(componentType);

                if (properties != null && properties.Type == DataType.Table)
                {
                    foreach (var pair in properties.Table.Pairs)
                    {
                        string propertyName = pair.Key.String;
                        DynValue propertyValue = pair.Value;

                        PropertyInfo propertyInfo = componentType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null && propertyInfo.CanWrite)
                        {
                            object value = null;
                            if (propertyValue.Type == DataType.Boolean)
                                value = propertyValue.Boolean;
                            else if (propertyValue.Type == DataType.Number)
                                value = propertyValue.Number;
                            else if (propertyValue.Type == DataType.String)
                                value = propertyValue.String;

                            propertyInfo.SetValue(component, value);
                        }
                    }
                }

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
            if (!gameObject.TryGetComponent<scriptGenerico>(out var _Ficha))
            {
                _Ficha = gameObject.AddComponent<scriptGenerico>();
            }

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

            _Ficha.Result = result;

            return result;
        }


    }
}
