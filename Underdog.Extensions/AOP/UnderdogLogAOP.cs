﻿using Castle.DynamicProxy;

using Underdog.Common.Helper;
using Underdog.Common.LogHelper;

using Newtonsoft.Json;
using StackExchange.Profiling;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Extensions.AOP
{
    /// <summary>
    /// 拦截器HZGLLogAOP 继承IInterceptor接口
    /// </summary>
    public class UnderdogLogAOP : IInterceptor
    {
        public UnderdogLogAOP()
        {

        }


        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            string UserName = "Root";
            string json;
            try
            {
                json = JsonConvert.SerializeObject(invocation.Arguments);
            }
            catch (Exception ex)
            {
                json = "无法序列化，可能是兰姆达表达式等原因造成，按照框架优化代码" + ex.ToString();
            }

            DateTime startTime = DateTime.Now;
            AOPLogInfo apiLogAopInfo = new AOPLogInfo
            {
                RequestTime = startTime.ToString("yyyy-MM-dd hh:mm:ss fff"),
                OpUserName = UserName,
                RequestMethodName = invocation.Method.Name,
                RequestParamsName = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()),
                ResponseJsonData = json
            };

            //测试异常记录
            //Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));

            //记录被拦截方法信息的日志信息
            //var dataIntercept = "" +
            //    $"【当前操作用户】：{ UserName} \r\n" +
            //    $"【当前执行方法】：{ invocation.Method.Name} \r\n" +
            //    $"【携带的参数有】： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            try
            {
                MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();


                // 异步获取异常，先执行
                if (IsAsyncMethod(invocation.Method))
                {
                    #region 方案一

                    //Wait task execution and modify return value
                    if (invocation.Method.ReturnType == typeof(Task))
                    {
                        invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                            (Task)invocation.ReturnValue,
                            async () => await SuccessAction(invocation, apiLogAopInfo, startTime), /*成功时执行*/
                            ex =>
                            {
                                LogEx(ex, apiLogAopInfo);
                            });
                    }
                    //Task<TResult>
                    else
                    {
                        invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                            invocation.Method.ReturnType.GenericTypeArguments[0],
                            invocation.ReturnValue,
                            //async () => await SuccessAction(invocation, dataIntercept),/*成功时执行*/
                            async (o) => await SuccessAction(invocation, apiLogAopInfo, startTime, o), /*成功时执行*/
                            ex =>
                            {
                                LogEx(ex, apiLogAopInfo);
                            });
                    }

                    #endregion


                    // 如果方案一不行，试试这个方案
                    //#region 方案二

                    //var type = invocation.Method.ReturnType;
                    //var resultProperty = type.GetProperty("Result");
                    //DateTime endTime = DateTime.Now;
                    //string ResponseTime = (endTime - startTime).Milliseconds.ToString();
                    //apiLogAopInfo.ResponseTime = endTime.ToString("yyyy-MM-dd hh:mm:ss fff");
                    //apiLogAopInfo.ResponseIntervalTime = ResponseTime + "ms";
                    //apiLogAopInfo.ResponseJsonData = JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue));

                    ////dataIntercept += ($"【响应时间】：{ResponseTime}ms\r\n");
                    ////dataIntercept += ($"【执行完成时间】：{endTime.ToString("yyyy-MM-dd hh:mm:ss fff")}\r\n");
                    ////dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}\r\n");

                    //Parallel.For(0, 1, e =>
                    //{
                    //    //LogLock.OutLogAOP("AOPLog", new string[] { dataIntercept });
                    //    LogLock.OutLogAOP("AOPLog", new string[] { apiLogAopInfo.GetType().ToString() + " - ResponseJsonDataType:" + type, JsonConvert.SerializeObject(apiLogAopInfo) });
                    //});

                    //#endregion
                }
                else
                {
                    // 同步1
                    string jsonResult;
                    try
                    {
                        jsonResult = JsonConvert.SerializeObject(invocation.ReturnValue);
                    }
                    catch (Exception ex)
                    {
                        jsonResult = "无法序列化，可能是兰姆达表达式等原因造成，按照框架优化代码" + ex.ToString();
                    }

                    var type = invocation.Method.ReturnType;
                    var resultProperty = type.GetProperty("Result");
                    DateTime endTime = DateTime.Now;
                    string ResponseTime = (endTime - startTime).Milliseconds.ToString();
                    apiLogAopInfo.ResponseTime = endTime.ToString("yyyy-MM-dd hh:mm:ss fff");
                    apiLogAopInfo.ResponseIntervalTime = ResponseTime + "ms";
                    //apiLogAopInfo.ResponseJsonData = JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue));
                    apiLogAopInfo.ResponseJsonData = jsonResult;
                    //dataIntercept += ($"【执行完成结果】：{jsonResult}");
                    Parallel.For(0, 1, e =>
                    {
                        //LogLock.OutLogAOP("AOPLog", new string[] { dataIntercept });
                        LogLock.OutLogAOP("AOPLog", "Root",
                            new string[] { apiLogAopInfo.GetType().ToString(), JsonConvert.SerializeObject(apiLogAopInfo) });
                    });
                }
            }
            catch (Exception ex) // 同步2
            {
                LogEx(ex, apiLogAopInfo);
                throw;
            }
        }

        private async Task SuccessAction(IInvocation invocation, AOPLogInfo apiLogAopInfo, DateTime startTime, object o = null)
        {
            //invocation.ReturnValue = o;
            //var type = invocation.Method.ReturnType;
            //if (typeof(Task).IsAssignableFrom(type))
            //{
            //    //var resultProperty = type.GetProperty("Result");
            //    //类型错误 都可以不要invocation参数，直接将o系列化保存到日记中
            //    dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(invocation.ReturnValue)}");
            //}
            //else
            //{
            //    dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
            //}
            DateTime endTime = DateTime.Now;
            string ResponseTime = (endTime - startTime).Milliseconds.ToString();
            apiLogAopInfo.ResponseTime = endTime.ToString("yyyy-MM-dd hh:mm:ss fff");
            apiLogAopInfo.ResponseIntervalTime = ResponseTime + "ms";
            apiLogAopInfo.ResponseJsonData = JsonConvert.SerializeObject(o);


            await Task.Run(() =>
            {
                Parallel.For(0, 1, e =>
                {
                    //LogLock.OutSql2Log("AOPLog", new string[] { JsonConvert.SerializeObject(apiLogAopInfo) });
                    LogLock.OutLogAOP("AOPLog", "Root",
                        new string[] { apiLogAopInfo.GetType().ToString(), JsonConvert.SerializeObject(apiLogAopInfo) });
                });
            });
        }

        private void LogEx(Exception ex, AOPLogInfo dataIntercept)
        {
            if (ex != null)
            {
                //执行的 service 中，收录异常
                MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
                //执行的 service 中，捕获异常
                //dataIntercept += ($"【执行完成结果】：方法中出现异常：{ex.Message + ex.InnerException}\r\n");
                AOPLogExInfo apiLogAopExInfo = new AOPLogExInfo
                {
                    ExMessage = ex.Message,
                    InnerException = "InnerException-内部异常:\r\n" + (ex.InnerException == null ? "" : ex.InnerException.InnerException.ToString()) +
                                     ("\r\nStackTrace-堆栈跟踪:\r\n") + (ex.StackTrace == null ? "" : ex.StackTrace.ToString()),
                    ApiLogAopInfo = dataIntercept
                };
                // 异常日志里有详细的堆栈信息
                Parallel.For(0, 1, e =>
                {
                    //LogLock.OutLogAOP("AOPLogEx", new string[] { dataIntercept });
                    LogLock.OutLogAOP("AOPLogEx", "Root",
                        new string[] { apiLogAopExInfo.GetType().ToString(), JsonConvert.SerializeObject(apiLogAopExInfo) });
                });
            }
        }


        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }
    }


    internal static class InternalAsyncHelper
    {
        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
                await postAction();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<object, Task> postAction,
            Action<Exception> finalAction)
        {
            Exception exception = null;
            try
            {
                var result = await actualReturnValue;
                await postAction(result);
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue,
            Func<object, Task> action, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
        }
    }
}
