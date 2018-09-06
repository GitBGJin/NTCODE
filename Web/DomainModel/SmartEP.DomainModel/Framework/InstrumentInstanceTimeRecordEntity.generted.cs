using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.Framework
{
    /// <summary>
    /// 名称：InstrumentInstanceTimeRecordEntity.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器使用记录实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public partial class InstrumentInstanceTimeRecordEntity
    {
        public InstrumentInstanceTimeRecordEntity()
        { }
        #region Model
        private string _rowguid;
        private string _typeguid;
        private DateTime? _occurtime;
        private string _operateuserGuid;
        private string _operateusername;
        private string _operatecontent;
        private string _note;
        private string _formurl;
        private string _rowstatus;
        private string _instanceguid;
        private string _changetype;
        private string _deviceguid;
        private string _occurstatus;
        private string _operateresault;
        private string _isreagent;
        private string _reagentguid;
        private int? _id;
        private int? _itemcount;
        private int? _pointid;
        /// <summary>
        /// 
        /// </summary>
        public string RowGuid
        {
            set { _rowguid = value; }
            get { return _rowguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeGuid
        {
            set { _typeguid = value; }
            get { return _typeguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OccurTime
        {
            set { _occurtime = value; }
            get { return _occurtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateUserGuid
        {
            set { _operateuserGuid = value; }
            get { return _operateuserGuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateUserName
        {
            set { _operateusername = value; }
            get { return _operateusername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateContent
        {
            set { _operatecontent = value; }
            get { return _operatecontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormUrl
        {
            set { _formurl = value; }
            get { return _formurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RowStatus
        {
            set { _rowstatus = value; }
            get { return _rowstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InstanceGuid
        {
            set { _instanceguid = value; }
            get { return _instanceguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChangeType
        {
            set { _changetype = value; }
            get { return _changetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceGuid
        {
            set { _deviceguid = value; }
            get { return _deviceguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OccurStatus
        {
            set { _occurstatus = value; }
            get { return _occurstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateResault
        {
            set { _operateresault = value; }
            get { return _operateresault; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IsReagent
        {
            set { _isreagent = value; }
            get { return _isreagent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReagentGuid
        {
            set { _reagentguid = value; }
            get { return _reagentguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ItemCount
        {
            set { _itemcount = value; }
            get { return _itemcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PointId
        {
            set { _pointid = value; }
            get { return _pointid; }
        }
        #endregion Model
    }
}
