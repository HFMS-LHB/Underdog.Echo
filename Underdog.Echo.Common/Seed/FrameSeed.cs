using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Common.Seed
{
    public class FrameSeed
    {
        /// <summary>
        /// 生成Model层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <param name="isMuti"></param>
        /// <returns></returns>
        public static bool CreateModels(SqlSugarScope sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {
            Create_Model_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Underdog.Echo.Model", "Underdog.Echo.Model.Models", tableNames, "", isMuti);
            return true;
        }

        /// <summary>
        /// 生成IRepository层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateIRepositorys(SqlSugarScope sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {
            Create_IRepository_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Underdog.Echo.IRepository", "Underdog.Echo.IRepository", tableNames, "", isMuti);
            return true;
        }



        /// <summary>
        /// 生成 IService 层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateIServices(SqlSugarScope sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {
            Create_IServices_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Underdog.Echo.IServices", "Underdog.Echo.IServices", tableNames, "", isMuti);
            return true;
        }



        /// <summary>
        /// 生成 Repository 层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateRepository(SqlSugarScope sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {
            Create_Repository_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Underdog.Echo.Repository", "Underdog.Echo.Repository", tableNames, "", isMuti);
            return true;
        }



        /// <summary>
        /// 生成 Service 层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateServices(SqlSugarScope sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {
            Create_Services_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Underdog.Echo.Services", "Underdog.Echo.Services", tableNames, "", isMuti);
            return true;
        }


        #region 根据数据库表生产Model层

        /// <summary>
        /// 功能描述:根据数据库表生产Model层
        /// 作　　者:Underdog
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        /// <param name="blnSerializable">是否序列化</param>
        private static void Create_Model_ClassFileByDBTalbe(
          SqlSugarScope sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false,
          bool blnSerializable = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\Models\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"{using}

namespace " + strNameSpace + @"
{
{ClassDescription}
    [SugarTable( ""{ClassName}"", """ + ConnId + @""")]" + (blnSerializable ? "\n    [Serializable]" : "") + @"
    public class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
           public {ClassName}()
           {
           }
{PropertyName}
    }
}")
                  //.SettingPropertyDescriptionTemplate(p => p = string.Empty)
                  .SettingPropertyTemplate(p => p =
@"{SugarColumn}
           public {PropertyType} {PropertyName} { get; set; }")

                   //.SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")

                   .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "{0}");
        }
        #endregion


        #region 根据数据库表生产IRepository层

        /// <summary>
        /// 功能描述:根据数据库表生产IRepository层
        /// 作　　者:Underdog
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_IRepository_ClassFileByDBTalbe(
          SqlSugarScope sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false
            )
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                 .SettingClassTemplate(p => p =
@"using Underdog.Echo.IRepository.Base;
using Underdog.Echo.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";

namespace " + strNameSpace + @"
{
	/// <summary>
	/// I{ClassName}Repository
	/// </summary>	
    public interface I{ClassName}Repository : IBaseRepository<{ClassName}>" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
    }
}")

                  .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "I{0}Repository");
        }
        #endregion


        #region 根据数据库表生产IServices层

        /// <summary>
        /// 功能描述:根据数据库表生产IServices层
        /// 作　　者:Underdog
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_IServices_ClassFileByDBTalbe(
          SqlSugarScope sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"using Underdog.Echo.IServices.BASE;
using Underdog.Echo.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";

namespace " + strNameSpace + @"
{	
	/// <summary>
	/// I{ClassName}Services
	/// </summary>	
    public interface I{ClassName}Services :IBaseServices<{ClassName}>" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
	{
    }
}")

                   .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "I{0}Services");
        }
        #endregion



        #region 根据数据库表生产 Repository 层

        /// <summary>
        /// 功能描述:根据数据库表生产 Repository 层
        /// 作　　者:Underdog
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_Repository_ClassFileByDBTalbe(
          SqlSugarScope sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"using Underdog.Echo.IRepository" + (isMuti ? "." + ConnId + "" : "") + @";
using Underdog.Echo.IRepository.UnitOfWork;
using Underdog.Echo.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";
using Underdog.Echo.Repository.Base;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// {ClassName}Repository
	/// </summary>
    public class {ClassName}Repository : BaseRepository<{ClassName}>, I{ClassName}Repository" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
        public {ClassName}Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}")
                  .ToClassStringList(strNameSpace);


            CreateFilesByClassStringList(ls, strPath, "{0}Repository");
        }
        #endregion


        #region 根据数据库表生产 Services 层

        /// <summary>
        /// 功能描述:根据数据库表生产 Services 层
        /// 作　　者:Underdog
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_Services_ClassFileByDBTalbe(
          SqlSugarScope sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"
using Underdog.Echo.IServices" + (isMuti ? "." + ConnId + "" : "") + @";
using Underdog.Echo.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";
using Underdog.Echo.Services.BASE;
using Underdog.Echo.IRepository.Base;

namespace " + strNameSpace + @"
{
    public class {ClassName}Services : BaseServices<{ClassName}>, I{ClassName}Services" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
        private readonly IBaseRepository<{ClassName}> _dal;
        public {ClassName}Services(IBaseRepository<{ClassName}> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}")
                  .ToClassStringList(strNameSpace);

            CreateFilesByClassStringList(ls, strPath, "{0}Services");
        }
        #endregion


        #region 根据模板内容批量生成文件
        /// <summary>
        /// 根据模板内容批量生成文件
        /// </summary>
        /// <param name="ls">类文件字符串list</param>
        /// <param name="strPath">生成路径</param>
        /// <param name="fileNameTp">文件名格式模板</param>
        private static void CreateFilesByClassStringList(Dictionary<string, string> ls, string strPath, string fileNameTp)
        {

            foreach (var item in ls)
            {
                var fileName = $"{string.Format(fileNameTp, item.Key)}.cs";
                var fileFullPath = Path.Combine(strPath, fileName);
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                File.WriteAllText(fileFullPath, item.Value, Encoding.UTF8);
            }
        }
        #endregion
    }
}
