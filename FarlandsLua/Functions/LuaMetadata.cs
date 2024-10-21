using Farlands.UI;
using JetBrains.Annotations;
using MoonSharp.Interpreter;
using Rewired.Libraries.SharpDX.RawInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public class LuaMetadata
    {
        private List<Meta> metadata = new();
        public enum Type
        {
            META,
            CLASS,
            FIELD,
            COMMENT,
            PARAM,
            RETURN,
            CODE
        }
        public class Meta
        {
            public Type type;
            public string values;
            public string typeString {
                get {
                    switch (type)
                    {
                        case Type.FIELD: return "field";
                        case Type.CLASS: return "class";
                        case Type.META: return "meta";
                        case Type.PARAM: return "param";
                        case Type.RETURN: return "return";
                        default: return "comment";
                    }
                }
            }
            public override string ToString()
            {
                if(type == Type.CODE) return values;
                else return $"---@{typeString} {values}";
            }
        }
        public void Add(Meta meta) => metadata.Add(meta);
        public void AddClass(string className) => metadata.Add(new Meta() { type = Type.CLASS, values = className});
        public void AddCode(string code) => metadata.Add(new Meta() { type = Type.CODE, values = code });
        public void AddFunction(string name, string parameters) => metadata.Add(new Meta() { type = Type.CODE, 
            values = $"function {name}({parameters}) end" });

        public void AddParam(string name, System.Type type) => metadata.Add(new Meta()
        {
            type = Type.PARAM,
            values = $"{name} {CSharpTypeToLuaMetadata(type)}",
        });
        public void AddReturn(System.Type type) => metadata.Add(new Meta()
        {
            type = Type.RETURN,
            values = $"{CSharpTypeToLuaMetadata(type)}",
        });
        private static string CSharpTypeToLuaMetadata(System.Type type)
        {
            if (typeof(DynValue).IsAssignableFrom(type)) return "any";
            if (typeof(int).IsAssignableFrom(type)) return "integer";
            if (typeof(float).IsAssignableFrom(type)) return "number";
            if (typeof(string).IsAssignableFrom(type)) return "string";
            if (type.IsArray)
            {
                var elementType = CSharpTypeToLuaMetadata(type.GetElementType());
                return elementType + "[]";
            }
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericType = type.IsGenericType ? type.GetGenericArguments()[0] : typeof(DynValue);
                var elementType = CSharpTypeToLuaMetadata(genericType);
                return elementType + "[]";
            }
            if (typeof(object).IsAssignableFrom(type)) return "object";
            return "object";
        }

        public override string ToString()
        {
            return string.Join("\n", metadata.Select(x => x.ToString()));
        }
    }
}
