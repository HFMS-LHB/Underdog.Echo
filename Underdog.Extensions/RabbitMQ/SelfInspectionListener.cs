using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Underdog.Common.Helper;
using Underdog.EventBus;
using Underdog.IServices;

using Underdog.Model.Models;

using Underdog.Model;

using Underdog.Repository.UnitOfWorks;
using Underdog.Tasks.QuartzNet;

namespace Underdog.Extensions.RabbitMQ
{
    public class SelfInspectionListener : IRabbitMQListener
    {
        private readonly ILogger<SelfInspectionListener> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ISchedulerCenter _schedulerCenter;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ITasksQzServices _tasksQzServices;
        private IModel? _channel;
        private AsyncEventingBasicConsumer? _consumer;

        public SelfInspectionListener(ILogger<SelfInspectionListener> logger,
                                      IRabbitMQPersistentConnection persistentConnection,
                                      ISchedulerCenter schedulerCenter,
                                      IUnitOfWorkManage unitOfWorkManage,
                                      ITasksQzServices tasksQzServices)
        {
            _logger = logger;
            _persistentConnection = persistentConnection;
            _schedulerCenter = schedulerCenter;
            _unitOfWorkManage = unitOfWorkManage;
            _tasksQzServices = tasksQzServices;
        }
        public void StartListening()
        {
            if (!_persistentConnection.IsConnected)
            {
                if (!_persistentConnection.TryConnect())
                {
                    _logger.LogWarning("RabbitMQ 无法连接");
                    return;
                }
            }

            // 获取设备标识
            var deviceNo = AppSettings.app(["Startup", "DeviceNo"]);
            var queueName = $"SelfInspection_{deviceNo}"; // 自检队列
            _channel = _persistentConnection.CreateModel();
            var arguments = new Dictionary<string, object>
            {
                { "x-message-ttl", 30000 } // 队列中消息的TTL，单位是毫秒，这里设置为60秒
            };
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: arguments);

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += new AsyncEventHandler<BasicDeliverEventArgs>(
                async (a, b) =>
                {
                    var msgBody = b.Body.ToArray();
                    var message = Encoding.UTF8.GetString(msgBody);

                    TasksQz tasksQz = new TasksQz();
                    var excuteTime = DateTime.Now;
                    tasksQz.Name = "清点任务";
                    tasksQz.JobGroup = "清盘点任务";
                    tasksQz.AssemblyName = "Eagle.hzgl.Tasks";
                    tasksQz.ClassName = "Job_EagleTakeStock_Quartz";
                    tasksQz.Cron = "0 */1 * * * ?";
                    tasksQz.BeginTime = excuteTime;
                    tasksQz.EndTime = excuteTime.AddDays(1);
                    tasksQz.TriggerType = 0;
                    tasksQz.IntervalSecond = 0;
                    tasksQz.CycleRunTimes = 1;
                    tasksQz.JobParams = message;
                    tasksQz.IsStart = true;
                    tasksQz.IsDeleted = false;

                    // 检查定时任务是否已经存在，如果不存在则创建，存在则直接返回
                    await AddTaskQzAndStartAsync(tasksQz);

                    Console.WriteLine("Received message: {0}", message);
                    await Task.Delay(1000);
                    // 手动确认消息
                    _channel.BasicAck(deliveryTag: b.DeliveryTag, multiple: false);
                });

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: _consumer);

            Console.WriteLine("Consuming messages...");
        }

        private async Task<bool> AddTaskQzAndStartAsync(TasksQz tasksQz)
        {
            var resultModel = new MessageModel<string>();
            // 检查任务是否已经存在
            var existList = await _tasksQzServices.Query(x => x.IsDeleted == true && x.JobParams == tasksQz.JobParams);
            if (existList.Count != 0)
            {
                return true;
            }

            _unitOfWorkManage.BeginTran();
            try
            {
                var id = await _tasksQzServices.Add(tasksQz);
                if (id > 0 && tasksQz.IsStart)
                {
                    resultModel = await _schedulerCenter.AddScheduleJobAsync(tasksQz);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定时任务启动失败");
            }
            finally
            {
                if (resultModel.success)
                    _unitOfWorkManage.CommitTran();
                else
                    _unitOfWorkManage.RollbackTran();
            }

            return resultModel.success;
        }

        public void StopListening()
        {
            _channel?.Close();
            _channel?.Dispose();
        }
    }
}
