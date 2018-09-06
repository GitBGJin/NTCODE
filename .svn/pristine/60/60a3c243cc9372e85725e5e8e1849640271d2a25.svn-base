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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    /// <summary>
    /// 名称：AddFitting.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2016-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器添加备件
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AddFitting : SmartEP.WebUI.Common.BasePage
    {
        public string instrumentInstanceGuid;
        public string type;

        SmartEP.DomainModel.FrameworkModel model = new SmartEP.DomainModel.FrameworkModel();

        InstrumentFittingInstanceService m_Service = new InstrumentFittingInstanceService();

        private string m_OperationSubmitWebServiceResourceInfoUrl = System.Configuration.ConfigurationManager.AppSettings["OperationSubmitWebServiceResourceInfoUrl"].ToString();
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                instrumentInstanceGuid = PageHelper.GetQueryString("InstrumentInstanceGuid");
                this.ViewState["instrumentInstanceGuid"] = instrumentInstanceGuid;
                type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
                this.ViewState["systemType"] = type;
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
        /// 给仪器添加备件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string UserName = SessionHelper.Get("DisplayName").ToString();
                IQueryable<Frame_UserEntity> userObjQueryable = model.Frame_UserEntities
                                    .Where(x => x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1);
                DateTime createTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                instrumentInstanceGuid = PageHelper.GetQueryString("InstrumentInstanceGuid");
                Frame_UserEntity userEntity = userObjQueryable.FirstOrDefault(x => x.DisplayName.Equals(UserName));
                string userGuid = (userEntity != null) ? userEntity.RowGuid : string.Empty;

                //获取当前仪器的所在测点
                string point = m_MaintenanceService.GetInstanceSite(instrumentInstanceGuid);
                string OutNote = string.Format("配件于{0}出库，现转移到{1}中", createTime, point);
                string InNote = string.Format("配件于{0}重新放入仓库", createTime);


                //TODO获取选中的备件行，插入到新表（仪器备件表）中
                foreach (GridDataItem item in grid.SelectedItems)
                {
                    OMMP_InstrumentFittingInstanceEntity InstrumentFittingInstance = new OMMP_InstrumentFittingInstanceEntity();

                    InstrumentFittingInstance.RowGuid = Guid.NewGuid().ToString();
                    //仪器类型Guid
                    InstrumentFittingInstance.InstrumentGuid = model.OMMP_InstrumentInstanceEntities.Where(p => p.RowGuid == instrumentInstanceGuid).Select(p => p.InfoGuid).FirstOrDefault().ToString();
                    //仪器类型名称
                    InstrumentFittingInstance.InstrumentName = model.OMMP_InstrumentInstanceEntities.Where(p => p.RowGuid == instrumentInstanceGuid).Select(p => p.InstanceName).FirstOrDefault();
                    //仪器实例guid
                    InstrumentFittingInstance.InstrumentInstanceGuid = instrumentInstanceGuid;
                    //仪器编号
                    InstrumentFittingInstance.InstrumentFixedAssetNumber = model.OMMP_InstrumentInstanceEntities.Where(p => p.RowGuid == instrumentInstanceGuid).Select(p => p.FixedAssetNumber).FirstOrDefault();
                    //配件类型Guid
                    InstrumentFittingInstance.FittingGuid = model.OMMP_InstrumentInstanceEntities.Where(p => (p.FixedAssetNumber == item["FixedAssetNumber"].Text)).Select(p => p.InfoGuid).FirstOrDefault();
                    //配件类型名称
                    InstrumentFittingInstance.FittingName = item["InstanceName"].Text;
                    //配件实例Guid
                    InstrumentFittingInstance.FittingInstanceGuid = model.OMMP_InstrumentInstanceEntities.Where(p => (p.FixedAssetNumber == item["FixedAssetNumber"].Text)).Select(p => p.RowGuid).FirstOrDefault();
                    //配件编号
                    InstrumentFittingInstance.FittingFixedAssetNumber = item["FixedAssetNumber"].Text;
                    InstrumentFittingInstance.DateTime = createTime;
                    m_Service.Add(InstrumentFittingInstance);
                    #region 查看当前仪器是否已添加过当前类型的配件，若有则替换掉原有配件
                    string OrgFittingInstanceGuid = m_MaintenanceService.GetIsFitting(InstrumentFittingInstance.InstrumentGuid, InstrumentFittingInstance.FittingGuid);
                    if (!string.IsNullOrWhiteSpace(OrgFittingInstanceGuid))
                    { 
                        //获取原有仪器配件配对RowGuid
                        string OrgRowGuid=m_MaintenanceService.GetIsFittingKey(InstrumentFittingInstance.InstrumentGuid, InstrumentFittingInstance.FittingGuid);
                        //配件编号
                        string FittingFixedAssetNumber = model.OMMP_InstrumentInstanceEntities.Where(p => (p.RowGuid == OrgRowGuid)).Select(p => p.FixedAssetNumber).FirstOrDefault();

                        ArrayList SelGuid = new ArrayList();
                        SelGuid.Add(OrgRowGuid);
                        string[] SelGid = (string[])SelGuid.ToArray(typeof(string));

                        IQueryable<OMMP_InstrumentFittingInstanceEntity> entities = m_Service.RetrieveListByUids(SelGid);
                        if (entities != null && entities.Count() > 0)
                        {
                            m_Service.BatchDelete(entities.ToList());
                        }

                        //向操作记录表添加【被替换配件】的操作信息
                        string JSONstringdel = "[";
                        JSONstringdel += "{";
                        JSONstringdel += "\"" + "FixedAssetNumber" + "\":\"" + FittingFixedAssetNumber + "\",";//配件ID	取自仪器固定资产编号
                        JSONstringdel += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                        JSONstringdel += "\"" + "statusguid" + "\":\"" + "491A543B-5409-4FE5-A7DA-4AE195220B32" + "\",";//运行  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                        JSONstringdel += "\"" + "timetypeguid" + "\":\"" + "E1B7B10B-8698-4A9A-BC71-6ADC26F06955" + "\",";//备件更换  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                        JSONstringdel += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                        JSONstringdel += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                        JSONstringdel += "\"" + "OperateContent" + "\":\"" + "备件换下" + "\",";//操作内容
                        JSONstringdel += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                        JSONstringdel += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                        JSONstringdel += "\"" + "changetype" + "\":\"" + "0" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                        JSONstringdel += "\"" + "deviceguid" + "\":\"" + "" + "\",";//更换的配件标识/耗材标识
                        JSONstringdel += "\"" + "OrgDeviceGuid" + "\":\"" + "" + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                        JSONstringdel += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                        JSONstringdel += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                        JSONstringdel += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                        JSONstringdel += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                        JSONstringdel += "}";
                        JSONstringdel += "]";
                        if (JSONstringdel != "[]")
                        {
                            object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                            "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstringdel });

                        }
                        //向出入库表中添加【被替换配件】重新入库信息
                        m_MaintenanceService.InsertInstrumentInstanceSite(0, InNote, OrgFittingInstanceGuid);
                        //向操作记录表中添加【被替换配件】重新入库信息
                        string JSONstringdelInSite = "[";
                        JSONstringdelInSite += "{";
                        JSONstringdelInSite += "\"" + "FixedAssetNumber" + "\":\"" + FittingFixedAssetNumber + "\",";//配件ID	取自仪器固定资产编号
                        JSONstringdelInSite += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                        JSONstringdelInSite += "\"" + "statusguid" + "\":\"" + "688ED583-2C04-4ABD-94BE-D34A15E8BCDC" + "\",";//库存  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                        JSONstringdelInSite += "\"" + "timetypeguid" + "\":\"" + "ef043e35-525f-41cb-ba97-e303b8b7cf9a" + "\",";//入库  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                        JSONstringdelInSite += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                        JSONstringdelInSite += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                        JSONstringdelInSite += "\"" + "OperateContent" + "\":\"" + InNote + "\",";//操作内容
                        JSONstringdelInSite += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                        JSONstringdelInSite += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                        JSONstringdelInSite += "\"" + "changetype" + "\":\"" + "0" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                        JSONstringdelInSite += "\"" + "deviceguid" + "\":\"" + "" + "\",";//更换的配件标识/耗材标识
                        JSONstringdelInSite += "\"" + "OrgDeviceGuid" + "\":\"" + "" + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                        JSONstringdelInSite += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                        JSONstringdelInSite += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                        JSONstringdelInSite += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                        JSONstringdelInSite += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                        JSONstringdelInSite += "}";
                        JSONstringdelInSite += "]";
                        if (JSONstringdelInSite != "[]")
                        {
                            object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                            "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstringdelInSite });
                        }


                    }
                    #endregion
                    //向操作记录表添加【仪器配件配对】信息
                    string JSONstring = "[";
                    JSONstring += "{";
                    JSONstring += "\"" + "FixedAssetNumber" + "\":\"" + InstrumentFittingInstance.InstrumentFixedAssetNumber + "\",";//仪器编号	取自仪器固定资产编号
                    JSONstring += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                    JSONstring += "\"" + "statusguid" + "\":\"" + "491A543B-5409-4FE5-A7DA-4AE195220B32" + "\",";//运行  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                    JSONstring += "\"" + "timetypeguid" + "\":\"" + "E1B7B10B-8698-4A9A-BC71-6ADC26F06955" + "\",";//备件更换  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                    JSONstring += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                    JSONstring += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                    JSONstring += "\"" + "OperateContent" + "\":\"" + "准用" + "\",";//操作内容
                    JSONstring += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                    JSONstring += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                    JSONstring += "\"" + "changetype" + "\":\"" + "1" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                    JSONstring += "\"" + "deviceguid" + "\":\"" + InstrumentFittingInstance.FittingInstanceGuid + "\",";//更换的配件标识/耗材标识
                    JSONstring += "\"" + "OrgDeviceGuid" + "\":\"" + OrgFittingInstanceGuid + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                    JSONstring += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                    JSONstring += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                    JSONstring += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                    JSONstring += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                    JSONstring += "}";
                    JSONstring += "]";
                    if (JSONstring != "[]")
                    {
                        //将新的配件放置仪器上
                        object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                                                    "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstring });
                        ////更新旧的配件状态 01 库存，02 试运行，03 运行，04 维修，05 停用，06 报废（目前不对FittngInstance表操作，目前此操作无效）
                        //if (!string.IsNullOrWhiteSpace(OrgFittingInstanceGuid))
                        //{
                        //    object objUpdateResult = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl,
                        //            "TempGetDataWebService", "UpdateFittingInstanceStatus", new object[] { OrgFittingInstanceGuid, "05" });

                        //}

                    }
                    //向出入库表中添加备件的出库信息
                    m_MaintenanceService.InsertInstrumentInstanceSite(1, OutNote, InstrumentFittingInstance.FittingInstanceGuid);
                    //向操作记录表插入【新添加配件】出库信息
                    string JSONstringdOutSite = "[";
                    JSONstringdOutSite += "{";
                    JSONstringdOutSite += "\"" + "FixedAssetNumber" + "\":\"" + InstrumentFittingInstance.FittingFixedAssetNumber + "\",";//配件编号	取自仪器固定资产编号
                    JSONstringdOutSite += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                    JSONstringdOutSite += "\"" + "statusguid" + "\":\"" + "491A543B-5409-4FE5-A7DA-4AE195220B32" + "\",";//运行  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                    JSONstringdOutSite += "\"" + "timetypeguid" + "\":\"" + "e7d12f42-1bd1-46cc-9fec-6ff8cfff9baf" + "\",";//出库  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                    JSONstringdOutSite += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                    JSONstringdOutSite += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                    JSONstringdOutSite += "\"" + "OperateContent" + "\":\"" + OutNote + "\",";//操作内容
                    JSONstringdOutSite += "\"" + "Note" + "\":\"" + "" + "\",";//备注
                    JSONstringdOutSite += "\"" + "FormUrl" + "\":\"" + "" + "\",";//操作表单链接地址
                    JSONstringdOutSite += "\"" + "changetype" + "\":\"" + "0" + "\",";//是否更换配件或耗材	0-未更换 1-更换配件 2-更换耗材
                    JSONstringdOutSite += "\"" + "deviceguid" + "\":\"" + "" + "\",";//更换的配件标识/耗材标识
                    JSONstringdOutSite += "\"" + "OrgDeviceGuid" + "\":\"" + "" + "\",";//换下的配件标识	用于更新换下来的备品备件信息用到的相关字段信息
                    JSONstringdOutSite += "\"" + "OperateResault" + "\":\"" + "" + "\",";//操作结果
                    JSONstringdOutSite += "\"" + "ItemCount" + "\":\"" + "" + "\",";//更换耗材数量	若更换耗材，需要传此参数；若未更换耗材，则可以不传此参数
                    JSONstringdOutSite += "\"" + "IsReagent" + "\":\"" + "0" + "\",";//是否更换试剂	0-未更换 1-更换试剂
                    JSONstringdOutSite += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";//试剂标识	试剂入库明细标识
                    JSONstringdOutSite += "}";
                    JSONstringdOutSite += "]";
                    if (JSONstringdOutSite != "[]")
                    {
                        object objSubmitResult = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl,
                        "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstringdOutSite });
                    }

                    //向操作记录表插入【新添加配件】操作信息
                    string JSONstringFitting = "[";
                    JSONstringFitting += "{";
                    JSONstringFitting += "\"" + "FixedAssetNumber" + "\":\"" + InstrumentFittingInstance.FittingFixedAssetNumber + "\",";//配件编号	取自仪器固定资产编号
                    JSONstringFitting += "\"" + "Date" + "\":\"" + createTime + "\",";//操作时间	注意时间格式：yyyy-MM-dd
                    JSONstringFitting += "\"" + "statusguid" + "\":\"" + "491A543B-5409-4FE5-A7DA-4AE195220B32" + "\",";//运行  //仪器操作后状态	取自接口GetInstanceStatus()中的输出参数RowGuid
                    JSONstringFitting += "\"" + "timetypeguid" + "\":\"" + "E1B7B10B-8698-4A9A-BC71-6ADC26F06955" + "\",";//备件更换  //操作类型标识	取自接口GetInstanceTimeType()中的输出参数：RowGid
                    JSONstringFitting += "\"" + "UserGuid" + "\":\"" + userGuid + "\",";//操作人标识
                    JSONstringFitting += "\"" + "UserName" + "\":\"" + UserName + "\",";//操作人姓名
                    JSONstringFitting += "\"" + "OperateContent" + "\":\"" + "备件绑定仪器" + "\",";//操作内容
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

                }
                Alert("添加成功！");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);

            }
            catch (Exception ex)
            { 
                Alert("添加失败！"); 
            }
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

                string fittingName = FittingName.Text.Trim();
                grid.DataSource = m_MaintenanceService.GetFittingInstance(fittingName, this.ViewState["systemType"].ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        protected void grid_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "InstanceName")
                {
                    col.HeaderText = "配件名称";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);

                }
                else if (col.DataField == "FixedAssetNumber")
                {
                    col.HeaderText = "系统编号";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception ex) { throw ex; }
        }


    }
}