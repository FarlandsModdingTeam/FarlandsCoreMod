using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.FarlandsLua
{
    public static class LuaExtensor
    {
        public static int Integer(this DynValue val)
        { 
            return Convert.ToInt32(val.Number);
        }

        public static float Float(this DynValue val)
        {
            return Convert.ToSingle(val.Number);
        }
    }
}
