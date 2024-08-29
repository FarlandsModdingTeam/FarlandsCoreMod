﻿using FarlandsCoreMod.Utiles.Loaders;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FarlandsCoreMod.FarlandsConsole
{
    public static class LuaGameObject
    {
        public static DynValue FromGameObject(GameObject gameObject)
        {
            DynValue result = DynValue.NewTable(new Table(Manager.LUA));

            result.Table.Set("toggle_active", DynValue.NewCallback((ctx, args) =>
            {
                gameObject.SetActive(gameObject.activeSelf);
                return DynValue.Void;
            }));

            result.Table.Set("add_component", DynValue.NewCallback((ctx, args) =>
            {
                if (args != null || args.Count < 1)
                    return DynValue.Void;

                foreach (DynValue arg in args.GetArray())
                { 
                    if(arg.String == "image") gameObject.AddComponent<Image>();
                    if(arg.String == "sprite") gameObject.AddComponent<SpriteRenderer>();
                }

                return DynValue.Void;
            }));

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
            return result;
        }
    }
}
