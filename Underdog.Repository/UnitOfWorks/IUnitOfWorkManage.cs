using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Repository.UnitOfWorks
{
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();
        int TranCount { get; }

        UnitOfWork CreateUnitOfWork();

        void BeginTran();
        void BeginTran(MethodInfo method);
        void CommitTran();
        void CommitTran(MethodInfo method);
        void RollbackTran();
        void RollbackTran(MethodInfo method);
    }
}
