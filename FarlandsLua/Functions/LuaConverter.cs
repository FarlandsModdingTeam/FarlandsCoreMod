using Language.Lua;
using MoonSharp.Interpreter;
using Rewired.Libraries.SharpDX.RawInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public static class LuaConverter
    {
        public static object[] CallbackArgumentToObjectArray(CallbackArguments args, List<Type> types)
        { 
            var res = new List<object>();
            for (int i = 0; i < args.Count; i++)
            { 
                res.Add(ToCS(args[i], types[i]));
            }
            return res.ToArray();
        }
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

        public static T ToCS<T>(DynValue luaArg) => (T) ToCS(luaArg, typeof(T));
        public static object ToCS(DynValue luaArg)
        {
            Type type = LuaTypeToCSharpType(luaArg.Type);
            return ToCS(luaArg, type);
        }

        // ConvertLuaArgumentToCSharp
        public static object ToCS(DynValue luaArg, Type targetType)
        {
            if (typeof(DynValue).IsAssignableFrom(targetType))
                return luaArg;

            if (targetType == typeof(int))
                return luaArg.Integer();

            else if (targetType == typeof(float))
                return luaArg.Float();

            else if (targetType == typeof(bool))
                return luaArg.Boolean;

            else if (targetType == typeof(string))
                return luaArg.String ?? luaArg.Number.ToString() ?? luaArg.Boolean.ToString();

            else if (targetType == typeof(Vector2))
            {
                var table = luaArg.Table;
                float x = table.Get("x").Type == DataType.Number ? (float)table.Get("x").Number : 0f;
                float y = table.Get("y").Type == DataType.Number ? (float)table.Get("y").Number : 0f;

                return new Vector2(x, y);
            }

            else if (targetType == typeof(Vector3))
            {
                var table = luaArg.Table;
                float x = table.Get("x").Type == DataType.Number ? (float)table.Get("x").Number : 0f;
                float y = table.Get("y").Type == DataType.Number ? (float)table.Get("y").Number : 0f;
                float z = table.Get("z").Type == DataType.Number ? (float)table.Get("z").Number : 0f;

                return new Vector3(x, y, z);
            }

            else if (targetType == typeof(List<DynValue>))
            {
                List<DynValue> dyn = [.. luaArg.Table.Values.ToList()];
                return dyn;
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
                return DynValue.NewTable(table);
            }
            else if (csharpArg is Vector3 @Vector3)
            {
                Table table = LuaFactory.FromObject(csharpArg).Table;
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
