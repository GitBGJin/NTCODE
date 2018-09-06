using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Service.OperatingMaintenance.Water;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    /// <summary>
    /// 名称：ShowFitting.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2016-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：显示仪器已添加的备件
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>

    public partial class ShowFitting : SmartEP.WebUI.Common.BasePage
    {
        public string instrumentInstanceGuid;
        public string type;

        //服务处理
        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();

        InstrumentFittingInstanceService m_Service = new InstrumentFittingInstanceService();
        private string m_OperationSubmitWebServiceResourceInfoUrl = System.Configuration.ConfigurationManager.AppSettings["OperationSubmitWebServiceResourceInfoUrl"].ToString();

        SmartEP.DomainModel.FrameworkModel model = new SmartEP.DomainModel.FrameworkModel();
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                instrumentInstanceGuid = PageHelper.GetQueryString("InstrumentInstanceGuid");
                this.ViewState["instrumentInstanceGuid"] = instrumentInstanceGuid;
                hd.Value = instrumentInstanceGuid;
                type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
                hdType.Value = type;
                if (!string.IsNullOrEmpty(instrumentInstanceGuid))
                {
                    //InitControl();
                }

            }
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grid.Rebind();
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                instrumentInstanceGuid = PageHelper.GetQueryString("InstrumentInstanceGuid");
                string fittingName = FittingName.Text.Trim();
                hd.Value = instrumentInstanceGuid;
                DataView dtInstrumentFittingInstance = m_MaintenanceService.GetInstrumentFittingInstance(instrumentInstanceGuid, fittingName);//查询仪器配件

                grid.DataSource = dtInstrumentFittingInstance;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// ToolBar按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            //RadGrid radGrid = ((RadGrid)sender);
            //int Index = -1;
            switch (e.CommandName)
            {
                case "DeleteSelected":
                    #region DeleteSelected
                    ArrayList SelGuid = new ArrayList();
                    ArrayList SelFittingInstance = new ArrayList();
                    DateTime createTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string note = string.Format("配件于{0}重新放入仓库", createTime);
                    string UserName = SessionHelper.Get("DisplayName").ToString();
                    IQueryable<Frame_UserEntity> userObjQueryable = model.Frame_UserEntities
                    .Where(x => x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1);
                    Frame_UserEntity userEntity = userObjQueryable.FirstOrDefault(x => x.DisplayName.Equals(UserName));
                    string userGuid = (userEntity != null) ? userEntity.RowGuid : string.Empty;
                    foreach (GridDataItem item in grid.SelectedItems)
                    {
                        SelGuid.Add(grid.MasterTableView.DataKeyValues[item.ItemIndex]["RowGuid"].ToString());
                        //配件实例Guid
                        string FittingInstanceGuid = model.OMMP_InstrumentInstanceEntities.Where(p => (p.FixedAssetNumber == item["FittingFixedAssetNumber"].Text)).Select(p => p.RowGuid).FirstOrDefault();
                        //仪器编号
                        string InstrumentFixedAssetNumber = model.OMMP_InstrumentInstanceEntities.Where(p => (p.RowGuid == this.ViewState["instrumentInstanceGuid"].ToString())).Select(p => p.FixedAssetNumber).FirstOrDefault();
                        //向操作记录表添加【仪器配件配对】信息
                        string JSONstring = "[";
                        JSONstring += "{";
                        JSONstring += "\"" + "FixedAssetNumber" + "\":\"" + InstrumentFixedAssetNumber + "\",";//仪器ID	取自仪器固定资产编号
                        JSONstring += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                        JSONstring += "\"" + "statusguid" + "\":\"" + "491A543B-5409-4FE5-A7DA-4AE195220B32" + "\",";//运行  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                        JSONstring += "\"" + "timetypeguid" + "\":\"" + "E1B7B10B-8698-4A9A-BC71-6ADC26F06955" + "\",";//备件更换  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                        JSONstring += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                        JSONstring += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                        JSONstring += "\"" + "OperateContent" + "\":\"" + "准用" + "\",";//操作内容
                        JSONstring += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                        JSONstring += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                        JSONstring += "\"" + "changetype" + "\":\"" + "1" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                        JSONstring += "\"" + "deviceguid" + "\":\"" + "" + "\",";//更换的配件标识/耗材标识
                        JSONstring += "\"" + "OrgDeviceGuid" + "\":\"" + FittingInstanceGuid + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                        JSONstring += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                        JSONstring += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                        JSONstring += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                        JSONstring += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                        JSONstring += "}";
                        JSONstring += "]";
                        if (JSONstring != "[]")
                        {
                            object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                            "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstring });

                        }
                        //向操作记录表添加【配件】操作信息
                        string JSONstringFitting = "[";
                        JSONstringFitting += "{";
                        JSONstringFitting += "\"" + "FixedAssetNumber" + "\":\"" + item["FittingFixedAssetNumber"].Text + "\",";//配件编号	取自仪器固定资产编号
                        JSONstringFitting += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                        JSONstringFitting += "\"" + "statusguid" + "\":\"" + "491A543B-5409-4FE5-A7DA-4AE195220B32" + "\",";//运行  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                        JSONstringFitting += "\"" + "timetypeguid" + "\":\"" + "E1B7B10B-8698-4A9A-BC71-6ADC26F06955" + "\",";//备件更换  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                        JSONstringFitting += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                        JSONstringFitting += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                        JSONstringFitting += "\"" + "OperateContent" + "\":\"" + "备件换下" + "\",";//操作内容
                        JSONstringFitting += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                        JSONstringFitting += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                        JSONstringFitting += "\"" + "changetype" + "\":\"" + "0" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                        JSONstringFitting += "\"" + "deviceguid" + "\":\"" + "" + "\",";//更换的配件标识/耗材标识
                        JSONstringFitting += "\"" + "OrgDeviceGuid" + "\":\"" + "" + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                        JSONstringFitting += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                        JSONstringFitting += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                        JSONstringFitting += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                        JSONstringFitting += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                        JSONstringFitting += "}";
                        JSONstringFitting += "]";
                        if (JSONstringFitting != "[]")
                        {
                            object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                            "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstringFitting });

                        }
                        //向出入库表中添加配件重新入库信息
                        m_MaintenanceService.InsertInstrumentInstanceSite(0, note, FittingInstanceGuid);
                        //向操作记录表添加【配件】重新入库信息
                        string JSONstringFittingInSite = "[";
                        JSONstringFittingInSite += "{";
                        JSONstringFittingInSite += "\"" + "FixedAssetNumber" + "\":\"" + item["FittingFixedAssetNumber"].Text + "\",";//配件编号	取自仪器固定资产编号
                        JSONstringFittingInSite += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                        JSONstringFittingInSite += "\"" + "statusguid" + "\":\"" + "688ED583-2C04-4ABD-94BE-D34A15E8BCDC" + "\",";//库存  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                        JSONstringFittingInSite += "\"" + "timetypeguid" + "\":\"" + "ef043e35-525f-41cb-ba97-e303b8b7cf9a" + "\",";//入库  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                        JSONstringFittingInSite += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                        JSONstringFittingInSite += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                        JSONstringFittingInSite += "\"" + "OperateContent" + "\":\"" + note + "\",";//操作内容
                        JSONstringFittingInSite += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                        JSONstringFittingInSite += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                        JSONstringFittingInSite += "\"" + "changetype" + "\":\"" + "0" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                        JSONstringFittingInSite += "\"" + "deviceguid" + "\":\"" + "" + "\",";//更换的配件标识/耗材标识
                        JSONstringFittingInSite += "\"" + "OrgDeviceGuid" + "\":\"" + "" + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                        JSONstringFittingInSite += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                        JSONstringFittingInSite += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                        JSONstringFittingInSite += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                        JSONstringFittingInSite += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                        JSONstringFittingInSite += "}";
                        JSONstringFittingInSite += "]";
                        if (JSONstringFittingInSite != "[]")
                        {
                            object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                            "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstringFittingInSite });

                        }

                        //SelFittingInstance.Add(FittingInstanceGuid);
                    }
                    string[] SelGid = (string[])SelGuid.ToArray(typeof(string));
                    //string[] SelFitingInstance = (string[])SelFittingInstance.ToArray(typeof(string));
                    //foreach (string FittingInstanceGuid in SelFitingInstance)
                    //{

                    //    ////更新旧的配件状态 01 库存，02 试运行，03 运行，04 维修，05 停用，06 报废(对FittingInstance表操作目前无效)
                    //    //object objUpdateResult = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl,
                    //    //    "TempGetDataWebService", "UpdateFittingInstanceStatus", new object[] { FittingInstanceGuid, "05" });
                    //}

                    IQueryable<OMMP_InstrumentFittingInstanceEntity> entities = m_Service.RetrieveListByUids(SelGid);
                    if (entities != null && entities.Count() > 0)
                    {
                        m_Service.BatchDelete(entities.ToList());
                        base.Alert("删除成功！");
                    }
                    #endregion
                    break;
            }
        }

    }
}