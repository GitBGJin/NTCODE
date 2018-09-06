using SmartEP.Core.Enums;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Alarm
{
    /// <summary>
    /// 名称：CreateAlarmTask.aspx
    /// 创建人：季柯
    /// 创建日期：2015-09-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：生成空气报警和通知服务，需传入数据类型参数
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class CreateAlarmTask : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //传入数据类型 【Min5：五分钟数据，Min60：小时数据】
                PollutantDataType pollutantDataType;
                ViewState["DataType"] = PageHelper.GetQueryString("DataType");
                if (ViewState["DataType"].ToString() == "Min5")
                {
                    pollutantDataType = PollutantDataType.Min5;
                }
                else
                {
                    pollutantDataType = PollutantDataType.Min60;
                }
                CreateAlarm(pollutantDataType);
            }
        }

        /// <summary>
        /// 生成报警和通知
        /// </summary>
        /// <param name="dataType">数据类型</param>
        private void CreateAlarm(PollutantDataType dataType)
        {
            //报警管理服务
            AlarmManagerService alarmService = new AlarmManagerService(ApplicationType.Air, dataType);
            //获取应用程序Uid和数据类型Uid
            string applicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationType.Air);
            string dataTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(dataType);
            //生命通知服务
            NotifySendService notifySendService = new NotifySendService();
            //调用通知方法
            notifySendService.NotifySend(applicationUid, dataTypeUid);
        }
    }
}