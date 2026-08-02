namespace Extensors;

public static class ObjectExtensor
{
    public static object GetPrivateField(this object obj, string key)
    {
        var type = obj.GetType();
        return type.GetField(key).GetValue(obj);
    }

    public static T GetPrivateField<T>(this object obj, string key) => (T)GetPrivateField(obj, key);
}
