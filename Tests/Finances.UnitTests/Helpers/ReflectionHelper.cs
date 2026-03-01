using System.Reflection;

namespace Tests.Helpers;

public static class ReflectionHelper
{
    //Метод расширения, чтобы писать new User(...).SetId(id)
    public static T SetId<T>(this T entity, Guid id)
    {
        var property = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

        ArgumentNullException.ThrowIfNull(property, nameof(property));

        property.SetValue(entity, id);
        return entity;
    }
}