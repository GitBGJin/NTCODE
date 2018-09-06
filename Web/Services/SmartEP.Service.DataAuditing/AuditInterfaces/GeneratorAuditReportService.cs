using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Core.Enums;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using System.IO;
using System.Configuration;
using System.Web;
using System.Data;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    /// <summary>
    /// 名称：GeneratorAuditReportService.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：徐龙超
    /// 最新维护日期：2015-08-27
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class GeneratorAuditReportService
    {
        #region 创建委托
        //声明委托
        public delegate void AuditOperatorEventHandler(Object sender, AuditOperatorEventArgs e);
        public event AuditOperatorEventHandler AuditOperator; //声明事件

        // 定义AuditOperatorEventArgs类，传递给Observer所感兴趣的信息
        public class AuditOperatorEventArgs : EventArgs
        {
            public readonly ApplicationType applicationType;
            public readonly string[] portIds;
            public readonly DateTime dateStart;
            public readonly DateTime dateEnd;
            public readonly string type;
            public readonly bool IsCreateDBF;
            public readonly string[] factors;
            public AuditOperatorEventArgs(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, string type, bool IsCreateDBF)
            {
                this.applicationType = applicationType;
                this.portIds = portIds;
                this.dateStart = dateStart;
                this.dateEnd = dateEnd;
                this.type = type;
                this.IsCreateDBF = IsCreateDBF;

            }
            public AuditOperatorEventArgs(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, string type, bool IsCreateDBF, string[] factors)
            {
                this.applicationType = applicationType;
                this.portIds = portIds;
                this.dateStart = dateStart;
                this.dateEnd = dateEnd;
                this.type = type;
                this.IsCreateDBF = IsCreateDBF;
                this.factors = factors;
            }
        }

        // 可以供继承自 Heater 的类重写，以便继承类拒绝其他对象对它的监视
        protected virtual void OnAuditOperator(AuditOperatorEventArgs e)
        {
            if (AuditOperator != null)
            { // 如果有对象注册
                AuditOperator(this, e);  // 调用所有注册对象的方法
            }
        }

        //// 数据生成。
        //public void ReportGenerator(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, bool IsCreateDBF)
        //{
        //    //建立BoiledEventArgs 对象。
        //    AuditOperatorEventArgs e = new AuditOperatorEventArgs(applicationType, portIds, dateStart, dateEnd, IsCreateDBF);
        //    OnAuditOperator(e);  // 调用 AuditOperator方法
        //}
        // 数据生成。
        public void ReportGenerator(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, string type, bool IsCreateDBF)
        {
            //建立BoiledEventArgs 对象。
            AuditOperatorEventArgs e = new AuditOperatorEventArgs(applicationType, portIds, dateStart, dateEnd, type, IsCreateDBF);
            OnAuditOperator(e);  // 调用 AuditOperator方法
        }
        // 超级站数据生成(By Rondo)。
        public void ReportGenerator(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, string type, bool IsCreateDBF, string[] factors)
        {
            //建立BoiledEventArgs 对象。
            AuditOperatorEventArgs e = new AuditOperatorEventArgs(applicationType, portIds, dateStart, dateEnd, type, IsCreateDBF, factors);
            OnAuditOperator(e);  // 调用 AuditOperator方法
        }
        #endregion
    }

    #region 生成报表数据
    public class ReportDataService
    {
        public DateTime datetime;
        public bool isSuccess;

        public void GenerateData(Object sender, GeneratorAuditReportService.AuditOperatorEventArgs e)
        {
            try
            {
                isSuccess = true;
                GeneratorAuditReportService heater = (GeneratorAuditReportService)sender;
                AuditDataRepository auditRep = new AuditDataRepository();
                string errMsg = string.Empty;
                if (e.type == "2")
                    isSuccess = auditRep.GenerateDataNew(e.applicationType, e.portIds, e.dateStart, e.dateEnd, out errMsg);
                else
                {
                    if (e.factors == null)
                        isSuccess = auditRep.GenerateData(e.applicationType, e.portIds, e.dateStart, e.dateEnd, out errMsg);
                    else
                        isSuccess = auditRep.GenerateData(e.applicationType, e.portIds, e.dateStart, e.dateEnd, e.factors, out errMsg);
                }
                //生成区域、城市数据
                //try
                //{
                //    if (e.applicationType == ApplicationType.Air)
                //    {
                //        MonitoringPointAirService pointService = Singleton<MonitoringPointAirService>.GetInstance();
                //        auditRep.GenerateDataRegion(e.applicationType, pointService.GetRegionByPort(e.portIds), e.dateStart, e.dateEnd, out errMsg);
                //        auditRep.GenerateDataCity(e.applicationType, pointService.GetCityByPort(e.portIds), e.dateStart, e.dateEnd, out errMsg);
                //        if (e.IsCreateDBF)
                //        {
                //            AuditDataService auditDataService = new AuditDataService();
                //            DateTime dateStart = DateTime.Parse(e.dateStart.ToString("yyyy-MM-dd 00:00:00"));
                //            DateTime dateEnd = DateTime.Parse(e.dateEnd.ToString("yyyy-MM-dd 23:59:59"));
                //            auditDataService.CreateExportFile(dateStart, dateEnd, out errMsg);
                //        }
                //    }
                //}
            }
            catch
            {
                isSuccess = false;
            }
        }
        public void GenerateDataSuper(Object sender, GeneratorAuditReportService.AuditOperatorEventArgs e)
        {
            isSuccess = true;
            try
            {
                GeneratorAuditReportService heater = (GeneratorAuditReportService)sender;
                AuditDataRepository auditRep = new AuditDataRepository();
                string errMsg = string.Empty;
                isSuccess = auditRep.GenerateDataSuper(e.applicationType, e.portIds, e.dateStart, e.dateEnd, out errMsg);
            }
            catch
            {
                isSuccess = false;
            }
        }

    }
    #endregion
}
