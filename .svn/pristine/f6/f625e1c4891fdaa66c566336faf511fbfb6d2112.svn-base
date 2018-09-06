using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：ExcessiveSettingDAL.cs
    /// 创建人：索丽娜
    /// 创建日期：2016-07-08
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：超标限值配置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ExcessiveSettingDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Day);

        private string Frame_Connection = "Frame_Connection";

        private string BaseData_Connection = "AMS_BaseDataConnection";

        #endregion



        /// <summary>
        /// 获取报警上下限数据
        /// </summary>
        /// <returns></returns>
        public DataView GetExcessiveSettingData(string DataType, string NotifyGrade, string UseFor, string portIds, string ExcessiveUid, string factorCodes)
        {
            string sql = string.Format(@"SELECT 
      [ExcessiveUid]
      ,a.[MonitoringPointUid]
      ,b.PointId
      ,b.MonitoringPointName
      ,a.[InstrumentChannelsUid]
      ,c.PollutantCode
      ,c.PollutantName
      ,[NotifyGradeUid]
      ,a.[ApplicationUid]
      ,[DataTypeUid]
      ,[AdvanceLow]
      ,[AdvanceUpper]
      ,[AdvanceRange]
      ,[ExcessiveUpper]
      ,[ExcessiveLow]
      ,[ExcessiveRange]
      ,[ReplaceStatus]
      ,[StandardType]
      ,[ExcessiveRatio]
      ,[UseForUid]
      ,[NotifyOrNot]
      ,a.[EnableOrNot]
      ,a.Description
  FROM [BusinessRule].[TB_ExcessiveSetting] as a 
  left join [MPInfo].[TB_MonitoringPoint] as b on a.MonitoringPointUid=b.MonitoringPointUid
  left join [InstrInfo].[TB_InstrumentChannels] as c on a.InstrumentChannelsUid=c.InstrumentChannelsUid where 1=1");
            if (DataType != string.Empty)
            {
                sql += string.Format(@" and a.DataTypeUid='{0}'", DataType);
            }
            if (NotifyGrade != string.Empty)
            {
                sql += string.Format(@" and a.NotifyGradeUid='{0}'", NotifyGrade);
            }
            if (UseFor != string.Empty)
            {
                sql += string.Format(@" and a.UseForUid='{0}'", UseFor);
            }
            if (portIds !=  string.Empty)
            {
                sql += string.Format(@" and a.MonitoringPointUid in ('{0}')", portIds);
            }
            if (factorCodes != string.Empty)
            {
                sql += string.Format(@" and c.PollutantCode in ('{0}')", factorCodes);
            }
            if (ExcessiveUid != string.Empty)
            {
                sql += string.Format(@" and a.ExcessiveUid='{0}'", ExcessiveUid);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, BaseData_Connection);
        }




        /// <summary>
        /// 代码项code
        /// </summary>
        /// <returns></returns>
        public DataView GetCode()
        {
            string sql = string.Format(@"select * from  TB_Frame_CodeItem where isenabled=1");
            return g_DatabaseHelper.ExecuteDataView(sql, Frame_Connection);
        }

        /// <summary>
        /// 根据ExcessiveUid删除数据
        /// </summary>
        /// <returns></returns>
        public void DeleteByExcessiveUid(string ExcessiveUid)
        {
            string sql = string.Format(@"delete from [BusinessRule].[TB_ExcessiveSetting] where ExcessiveUid='{0}'", ExcessiveUid);
            g_DatabaseHelper.ExecuteNonQuery(sql, BaseData_Connection);
        }


        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// [MonitoringPointUid]  站点guid
        /// [InstrumentChannelsUid] 仪器通GUid 
        /// [DataTypeUid] 时间类型
        /// [NotifyGradeUid] 等级
        /// <returns></returns>
        public bool IsExistByInfo(string pointId, string InstrumentChannelsUid, string DataTypeUid, string NotifyGradeUid,string ApplicationUid,string UseForUid)
        {
            bool isExist=false;
            string sql = string.Format(@"select id from [BusinessRule].[TB_ExcessiveSetting] where MonitoringPointUid='{0}' and InstrumentChannelsUid='{1}' and
            DataTypeUid='{2}' and NotifyGradeUid='{3}' and ApplicationUid='{4}' and UseForUid='{5}'", pointId, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid, ApplicationUid, UseForUid);
            DataView  isExistDV= g_DatabaseHelper.ExecuteDataView(sql, BaseData_Connection);
            if (isExistDV.Count > 0)
            {
                isExist = true;
            }
            return isExist;
        }

        /// <summary>
        /// 更新TB_ExcessiveSetting数据
        /// </summary>
        /// <returns></returns>
        public void UpdateExcessiveSet(string ApplicationUid, string pointId, string InstrumentChannelsUid, string DataTypeUid, string NotifyGradeUid, string AdvanceRange,
            decimal? AdvanceLow, decimal? AdvanceUpper, string ExcessiveRange, decimal? ExcessiveRatio, decimal? ExcessiveUpper, decimal? ExcessiveLow, string ReplaceStatus,
           string StandardType, int EnableOrNot, int NotifyOrNot, string Description, string AddUserGuid, string AddUserName, string AddOUGuid, string AddOUName, string UseForUid)
        {
            string sql = string.Format(@"update [BusinessRule].[TB_ExcessiveSetting] set AdvanceRange='{1}',ExcessiveRange='{2}',ReplaceStatus='{3}',
            StandardType='{4}',EnableOrNot={5},NotifyOrNot={6},Description='{7}',AddDate=getdate(), AddUserGuid='{8}',AddUserName='{9}',AddOUGuid='{10}',AddOUName='{11}',UseForUid='{12}'"
            , ApplicationUid, AdvanceRange, ExcessiveRange, ReplaceStatus, StandardType,
            EnableOrNot, NotifyOrNot, Description, AddUserGuid, AddUserName, AddOUGuid, AddOUName, UseForUid);

            if(AdvanceLow!=null)
                sql+=string.Format(@",AdvanceLow={0}",AdvanceLow);
            if(AdvanceUpper!=null)
                sql+=string.Format(@",AdvanceUpper={0}",AdvanceUpper);
            if(ExcessiveUpper!=null)
                sql+=string.Format(@",ExcessiveUpper={0}",ExcessiveUpper);
            if(ExcessiveLow!=null)
                sql+=string.Format(@",ExcessiveLow={0}",ExcessiveLow);
            if (ExcessiveRatio != null)
                sql += string.Format(@",ExcessiveRatio={0}", ExcessiveRatio);

            sql += string.Format(@" where MonitoringPointUid='{0}' and InstrumentChannelsUid='{1}' and DataTypeUid='{2}' and NotifyGradeUid='{3}' and ApplicationUid='{4}' and UseForUid='{5}'", pointId, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid, ApplicationUid, UseForUid);
                    
            g_DatabaseHelper.ExecuteNonQuery(sql, BaseData_Connection);
        }

        /// <summary>
        /// 插入TB_ExcessiveSetting数据
        /// </summary>
        /// <returns></returns>
        public void InsertExcessiveSet(string ApplicationUid, string pointId, string InstrumentChannelsUid, string DataTypeUid, string NotifyGradeUid, string AdvanceRange,
            decimal? AdvanceLow, decimal? AdvanceUpper, string ExcessiveRange, decimal? ExcessiveRatio, decimal? ExcessiveUpper, decimal? ExcessiveLow, string ReplaceStatus,
            string StandardType, int EnableOrNot, int NotifyOrNot, string Description, string AddUserGuid, string AddUserName, string AddOUGuid, string AddOUName, string UseForUid)
        {
            string sql = string.Format(@"insert into [BusinessRule].[TB_ExcessiveSetting](RowGuid,ApplicationUid,MonitoringPointUid, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid,AdvanceRange,AdvanceLow,AdvanceUpper,
            ExcessiveUpper, ExcessiveLow,ExcessiveRatio,ExcessiveRange,ReplaceStatus, StandardType, EnableOrNot, NotifyOrNot, Description, AddUserGuid, AddUserName, AddOUGuid, 
            AddOUName,RowStatus,UseForUid,AddDate) values (newid(),'{0}','{1}','{2}','{3}','{4}','{5}'", ApplicationUid, pointId, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid, AdvanceRange);

             if(AdvanceLow!=null)
             {
                sql+=string.Format(@",{0}",AdvanceLow);
             }
             else
             {
                 sql+=string.Format(@",null");
             }

            if(AdvanceUpper!=null)
            {
                sql+=string.Format(@",{0}",AdvanceUpper);
            }
            else
            {
                sql+=string.Format(@",null");
            }

            if(ExcessiveUpper!=null)
            {
                sql+=string.Format(@",{0}",ExcessiveUpper);
            }
            else
            {
                sql+=string.Format(@",null");
            }
            if(ExcessiveLow!=null)
            {
                sql+=string.Format(@",{0}",ExcessiveLow);
            }
             else
            {
                sql+=string.Format(@",null");
            }

            if (ExcessiveRatio != null)
            {
                sql += string.Format(@",{0}", ExcessiveRatio);
            }
             else
            {
                sql+=string.Format(@",null");
            }

            sql += string.Format(@",'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','1','{10}',getdate())",
            ExcessiveRange, ReplaceStatus, StandardType, EnableOrNot, NotifyOrNot, Description, AddUserGuid, AddUserName, AddOUGuid, AddOUName, UseForUid);
            g_DatabaseHelper.ExecuteNonQuery(sql, BaseData_Connection);
        }
   
        /// <summary>
        /// 根据站点查询InstrumentChannelsUid
        /// </summary>
        /// <returns></returns>
        public DataView GetInstrumentChBypoint(string pointId)
        {
            string sql = string.Format(@"select InstrumentChannelsUid,PollutantCode from dbo.V_Point_InstrumentChannels 
            where MonitoringPointUid='{0}' and TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'", pointId);
            return g_DatabaseHelper.ExecuteDataView(sql, BaseData_Connection);
        }

        /// <summary>
        /// 根据UserGuid查询账号部门信息
        /// </summary>
        /// <returns></returns>
        public DataView GetUserInfo()
        {
            string sql = string.Format(@"select a.rowguid,a.deptName,b.displayName from TB_Frame_Department A
            left join [TB_Frame_User] B on a.orgguid=B.orgguid  where b.rowguid='{0}'", SessionHelper.Get("UserGuid"));
            return g_DatabaseHelper.ExecuteDataView(sql, Frame_Connection);
        }
      

        /// <summary>
        /// 获取排口拼接字符串
        /// </summary>
        /// <returns></returns>
        public DataView GetPointList(string applicationUid)
        {
            string sql = string.Format(@"select MonitoringPointUid,MonitoringPointName from MPInfo.TB_MonitoringPoint where ApplicationUid='{0}'", applicationUid);
            return g_DatabaseHelper.ExecuteDataView(sql, BaseData_Connection);
        }

    }
}
