using System.Reflection;
using UnityEngine;

namespace FarlandsCoreMod.Utiles
{
    public static class Private
    {
        public static object GetFieldValue(object instance, string field) => GetFieldValue<object>(instance, field);
        public static T GetFieldValue<T>(object instance, string field)
        {
            var type = instance.GetType();
            var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)fieldInfo.GetValue(instance);
        }

        public static T SetFieldValue<T>(object instance, string field, T value)
        {
            var type = instance.GetType();
            var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            
            fieldInfo.SetValue(instance,value);
            return value;
        }

        public static object InvokeMethod(object instance, string method) => InvokeMethod(instance, method, []);
        public static object InvokeMethod(object instance, string method, object[] value) => InvokeMethod<object>(instance, method, value);
        public static T InvokeMethod<T>(object instance, string method) => InvokeMethod<T>(instance, method, []);
        public static T InvokeMethod<T>(object instance, string method, object[] value)
        {
            var type = instance.GetType();
            var fieldInfo = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);

            return (T) fieldInfo.Invoke(instance, value);
        }
    }
}