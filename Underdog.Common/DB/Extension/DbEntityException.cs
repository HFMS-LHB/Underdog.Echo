using System;
using System.Reflection;
using SqlSugar;

namespace Underdog.Common.DB.Extension;

public static class DbEntityException
{
    public static object GetEntityTenant(this Type type)
    {
        var tenant = type.GetCustomAttribute<TenantAttribute>();
        return tenant?.configId;
    }
}