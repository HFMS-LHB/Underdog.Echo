# Underdog.Echo

一个快速搭建项目的WPF框架

此项目是`StupidBear`和`Blog.Core`两个项目的缝合怪，技术有限，缝合的很失败，所以用`Underdog`命名/(ㄒoㄒ)/~~

* 文档都是Copy的`Blog.Core`的

&nbsp;
&nbsp;

-------------------------------
## 鸣谢

- [Blog.Core 开箱即用的企业级前后端分离【 .NET Core6.0 Api + Vue 2.x + RBAC】权限框架](https://github.com/anjoy8/Blog.Core)
- [StupidBear WPF开发框架](https://github.com/AelousDing/StupidBear)

 &nbsp;

---------------------

#### 框架模块：  
- [x] 采用`仓储+服务+接口`的形式封装框架；
- [x] 自定义项目模板 `CreateYourProject.bat` ，可以一键生成自己的项目；🎶  
- [x] 异步 async/await 开发；  
- [x] 接入国产数据库ORM组件 —— SqlSugar，封装数据库操作，支持级联操作；
- [x] 支持自由切换多种数据库，MySql/SqlServer/Sqlite/Oracle/Postgresql/达梦/人大金仓；
- [x] 实现项目启动，自动生成种子数据 ✨； 
- [x] 实现数据库主键类型配置化，什么类型都可以自定义 ✨； 
- [x] 四种日志记录，审计/异常/服务操作/Sql记录等,并自动持久化到数据库表🎶； 
- [x] 支持项目事务处理（若要分布式，用cap即可）✨；
- [x] 设计4种 AOP 切面编程，功能涵盖：日志、缓存、审计、事务 ✨；
- [x] 全局统一封装 Serilog 生成多种日志，并自动生成到数据库中，目前支持MySql/SqlServer/Sqlite/Oracle/Postgresql🎉；
- [x] 封装`Underdog.Echo.Template`项目模板，一键重建自己的项目 ✨；
- [x] 实现分表案例，支持分表的增删改查哈分页查询


组件模块：
- [x] 提供 Redis 做缓存处理；
- [x] 使用 Automapper 处理对象映射；  
- [x] 使用 AutoFac 做依赖注入容器，并提供批量服务注入 ✨；
- [x] 使用 Serilog 日志框架，集成原生 ILogger 接口做日志记录；
- [x] 使用 SignalR 双工通讯 ✨；
- [x] 使用 Quartz.net 做任务调度（目前单机多任务，集群调度暂不支持）;
- [x] 支持 数据库`读写分离`和多库操作 ✨;
- [x] 新增 Redis 消息队列 ✨;
- [x] 新增 RabbitMQ 消息队列 ✨;
- [x] 新增 EventBus 事件总线 ✨;
- [x] 新增 - Serilog 集成日志数据持久化到数据库;  


### 核心业务模块
#### 框架采用泛型仓储模式，以下几层为核心层，不可删除    
`Underdog.Echo.Main`、`Underdog.Echo.Common`、`Underdog.Echo.IServices`、`Underdog.Echo.Model`、`Underdog.Echo.Repository`、`Underdog.Echo.Services`、`Underdog.Echo.Tasks`、`Underdog.Echo.Serilog`    
其他代码分层是支撑层，如果自己业务涉及不到，可以删除。


&nbsp;

### 初始化项目
 

下载项目后，编译如果没问题，直接运行即可，会自动生成种子数据，数据库是`Sqlite`。


&nbsp;

## Nuget Packages

| Package | NuGet Stable |  Downloads |
| ------- | -------- | ------- |
| [Underdog.Echo.Template](https://www.nuget.org/packages/Underdog.Echo.Template/) | [![Underdog.Echo.Template](https://img.shields.io/nuget/v/Underdog.Echo.Template.svg)](https://www.nuget.org/packages/Underdog.Echo.Template/)  | [![Underdog.Echo.Template](https://img.shields.io/nuget/dt/Underdog.Echo.Template.svg)](https://www.nuget.org/packages/Underdog.Echo.Template/) |
