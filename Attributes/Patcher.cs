using FarlandsCoreMod.Patchers;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FarlandsCoreMod.Attributes
{
    // TODO: Comentar todo este tema
    [AttributeUsage(AttributeTargets.Method)]
    public class PatcherPreload : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Patcher : Attribute
    {
        public static void LoadAll(Assembly assembly)
        {
            assembly
                .GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(Patcher), false).Length > 0)
                .ToList()
                .ForEach(t =>
                {
                    t.GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes(typeof(PatcherPreload), false).Length > 0).ToList()
                    .ForEach(m => m.Invoke(null, null));
                    Harmony.CreateAndPatchAll(t);
                });
        }
        public static void LoadAll() => LoadAll(Assembly.GetCallingAssembly());
    }
}