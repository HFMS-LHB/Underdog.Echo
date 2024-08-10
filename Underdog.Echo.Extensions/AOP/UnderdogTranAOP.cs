using Castle.DynamicProxy;

using Underdog.Echo.Common.Attributes;
using Underdog.Echo.Common.DB;
using Underdog.Echo.Repository.UnitOfWorks;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.AOP
{
    /// <summary>
    /// 事务拦截器HZGLTranAOP 继承IInterceptor接口
    /// </summary>
    public class UnderdogTranAOP : IInterceptor
    {
        private readonly ILogger<UnderdogTranAOP> _logger;
        private readonly IUnitOfWorkManage _unitOfWorkManage;

        public UnderdogTranAOP(IUnitOfWorkManage unitOfWorkManage, ILogger<UnderdogTranAOP> logger)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            if (method.GetCustomAttribute<UseTranAttribute>(true) is { } uta)
            {
                try
                {
                    Before(method, uta.Propagation);

                    invocation.Proceed();

                    // 异步获取异常，先执行
                    if (IsAsyncMethod(invocation.Method))
                    {
                        var result = invocation.ReturnValue;
                        if (result is Task)
                        {
                            Task.WaitAll(result as Task);
                        }
                    }

                    After(method);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    AfterException(method);
                    throw;
                }
            }
            else
            {
                invocation.Proceed(); //直接执行被拦截方法
            }
        }

        private void Before(MethodInfo method, Propagation propagation)
        {
            switch (propagation)
            {
                case Propagation.Required:
                    if (_unitOfWorkManage.TranCount <= 0)
                    {
                        _logger.LogDebug($"Begin Transaction");
                        Console.WriteLine($"Begin Transaction");
                        _unitOfWorkManage.BeginTran(method);
                    }

                    break;
                case Propagation.Mandatory:
                    if (_unitOfWorkManage.TranCount <= 0)
                    {
                        throw new Exception("事务传播机制为:[Mandatory],当前不存在事务");
                    }

                    break;
                case Propagation.Nested:
                    _logger.LogDebug($"Begin Transaction");
                    Console.WriteLine($"Begin Transaction");
                    _unitOfWorkManage.BeginTran(method);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propagation), propagation, null);
            }
        }

        private void After(MethodInfo method)
        {
            _unitOfWorkManage.CommitTran(method);
        }

        private void AfterException(MethodInfo method)
        {
            _unitOfWorkManage.RollbackTran(method);
        }

        /// <summary>
        /// 获取变量的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private async Task SuccessAction(IInvocation invocation)
        {
            await Task.Run(() =>
            {
                //...
            });
        }

        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }

        private async Task TestActionAsync(IInvocation invocation)
        {
            await Task.Run(null);
        }
    }
}
