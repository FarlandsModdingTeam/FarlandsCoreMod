using System.Reflection;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class Getter
    {
        public static T Field<T>(object instance, string field)
        {
            var type = instance.GetType();
            var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)fieldInfo.GetValue(instance);
        }

    }
}