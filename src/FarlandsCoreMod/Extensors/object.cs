using System.Reflection;

namespace FarlandsCoreMod.Core.Extensors;

public static class ObjectExtensor
{
    public static object GetPrivateField(this object obj, string key)
    {
        var type = obj.GetType();
        return type.GetField(key, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
    }

    public static T GetPrivateField<T>(this object obj, string key) => (T)GetPrivateField(obj, key);

    public static void SetPrivateField(this object obj, string key, object val)
    {
        var type = obj.GetType();
        type.GetField(key, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(obj, val);
    }

    public static object InvokePrivateMethod(this object obj, string key, params object[] args)
    {
        if (obj == null) return null;

        var type = obj.GetType();

        var method = type.GetMethod(
            key,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (method == null)
        {
            UnityEngine.Debug.LogError($"[Extensors] No se encontró el método '{key}' en la clase '{type.FullName}'.");
            return null;
        }

        return method.Invoke(obj, args);
    }

    public static T InvokePrivateMethod<T>(this object obj, string key, params object[] args) => (T)InvokePrivateMethod(obj, key, args);
}
