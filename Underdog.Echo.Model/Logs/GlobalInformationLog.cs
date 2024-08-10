﻿using Underdog.Echo.Model.Base;
using SqlSugar;

namespace Underdog.Echo.Model.Logs;

[Tenant("log")]
[SplitTable(SplitType.Month)] //按月分表 （自带分表支持 年、季、月、周、日）
[SugarTable($@"{nameof(GlobalInformationLog)}_{{year}}{{month}}{{day}}")]
public class GlobalInformationLog : BaseLog
{

}