using Autofac;
using Autofac.Extras.DynamicProxy;

using Underdog.Common.Helper;
using Underdog.Extensions.AOP;
using Underdog.IServices.BASE;
using Underdog.Repository.BASE;
using Underdog.Repository.UnitOfWorks;
using Underdog.Services.BASE;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Extensions.ServiceExtensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "Underdog.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Underdog.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                Log.Error(msg);
                throw new Exception(msg);
            }


            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if (AppSettings.app(["AppSettings", "CachingAOP", "Enabled"]).ObjToBool())
            {
                builder.RegisterType<UnderdogCacheAOP>();
                cacheType.Add(typeof(UnderdogCacheAOP));
            }

            if (AppSettings.app(["AppSettings", "TranAOP", "Enabled"]).ObjToBool())
            {
                builder.RegisterType<UnderdogTranAOP>();
                cacheType.Add(typeof(UnderdogTranAOP));
            }

            if (AppSettings.app(["AppSettings", "LogAOP", "Enabled"]).ObjToBool())
            {
                builder.RegisterType<UnderdogLogAOP>();
                cacheType.Add(typeof(UnderdogLogAOP));
            }

            if (AppSettings.app(["AppSettings", "UserAuditAOP", "Enabled"]).ObjToBool())
            {
                builder.RegisterType<UnderdogAuditAOP>();
                cacheType.Add(typeof(UnderdogAuditAOP));
            }

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency(); //注册仓储
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();     //注册服务

            builder.RegisterGeneric(typeof(BaseApiRepository<>)).As(typeof(IBaseApiRepository<>)).InstancePerDependency(); //注册仓储
            builder.RegisterGeneric(typeof(BaseApiServices<>)).As(typeof(IBaseApiServices<>)).InstancePerDependency();     //注册服务

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors()       //引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(cacheType.ToArray()); //允许将拦截器服务的列表分配给注册。

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();

            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            #endregion
        }
    }
}
