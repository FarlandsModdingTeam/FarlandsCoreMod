using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public static class LuaConverter
    {
        private static Type LuaTypeToCSharpType(DataType type)
        {
            switch (type)
            {
                case DataType.String: return typeof(string);
                case DataType.Boolean: return typeof(bool);
                case DataType.Number: return typeof(float);
                default: return typeof(object);
            }
        }

        public static object ToCS(DynValue luaArg)
        {
            Type type = LuaTypeToCSharpType(luaArg.Type);
            return ToCS(luaArg, type);
        }

        // ConvertLuaArgumentToCSharp
        public static object ToCS(DynValue luaArg, Type targetType)
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
        public static DynValue ToLua(object csharpArg)
        {
            if (csharpArg == null)
                return DynValue.Nil;

            if (csharpArg is int @int)
                return DynValue.NewNumber(@int);

            else if (csharpArg is float @float)
                return DynValue.NewNumber(@float);

            else if (csharpArg is bool @bool)
                return DynValue.NewBoolean(@bool);

            else if (csharpArg is string @string)
                return DynValue.NewString(@string);

            else if (csharpArg is Vector2 @Vector2)
            {
                Table table = LuaFactory.FromObject(csharpArg).Table;

                table.Set("x", DynValue.NewNumber(@Vector2.x));
                table.Set("y", DynValue.NewNumber(@Vector2.y));

                return DynValue.NewTable(table);
            }
            else if (csharpArg is Vector3 @Vector3)
            {

                Table table = LuaFactory.FromObject(csharpArg).Table;

                table.Set("x", DynValue.NewNumber(@Vector3.x));
                table.Set("y", DynValue.NewNumber(@Vector3.y));
                table.Set("z", DynValue.NewNumber(@Vector3.z));

                return DynValue.NewTable(table);
            }
            else if (csharpArg is GameObject gameObject)
            {
                return LuaFactory.FromGameObject(gameObject);
            }
            else if (csharpArg is Component component)
            {
                return LuaFactory.FromComponent(component);
            }
            else
            {
                return DynValue.FromObject(LuaManager.LUA, csharpArg);
            }
        }
    }
}
