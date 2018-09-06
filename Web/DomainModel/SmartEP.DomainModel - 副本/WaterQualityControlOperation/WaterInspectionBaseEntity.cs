using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：WaterInspectionBaseEntity.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务记录表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    [Serializable]
    public class WaterInspectionBaseEntity
    {
        #region Model
        private int _id;
        private Guid _missionid;
        private string _missionname;
        private string _pointid;
        private string _pointname;
        private DateTime? _startdate;
        private DateTime? _enddate;
        private string _team;
        private DateTime? _actiondate;
        private DateTime? _finishdate;
        private string _status;
        private string _taskCode;
        private string _type;
        private string _taskType;
        private string _fileName;
        private string _filePath;
        private DateTime? _fileUpLoadDate;
        private string _actionUserName;
        private byte[] _fileStrems;
        private string _tempTaskID;
        private string _reportCode;
        private string _reportName;
        private string _formCode;
        private string _MN;
        private DateTime? _reportCreateDate;
        private DateTime? _reportUpdateDate;
        private DateTime? _reportUpLoadDate;
        private string _remark;
        private int _operateStatus;
        private Guid _taskguid;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid MissionID
        {
            set { _missionid = value; }
            get { return _missionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MissionName
        {
            set { _missionname = value; }
            get { return _missionname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointId
        {
            set { _pointid = value; }
            get { return _pointid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointName
        {
            set { _pointname = value; }
            get { return _pointname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Team
        {
            set { _team = value; }
            get { return _team; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ActionDate
        {
            set { _actiondate = value; }
            get { return _actiondate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FinishDate
        {
            set { _finishdate = value; }
            get { return _finishdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskCode
        {
            set { _taskCode = value; }
            get { return _taskCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskType
        {
            set { _taskType = value; }
            get { return _taskType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            set { _fileName = value; }
            get { return _fileName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            set { _filePath = value; }
            get { return _filePath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FileUpLoadDate
        {
            set { _fileUpLoadDate = value; }
            get { return _fileUpLoadDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ActionUserName
        {
            set { _actionUserName = value; }
            get { return _actionUserName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] FileStrems
        {
            set { _fileStrems = value; }
            get { return _fileStrems; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TempTaskID
        {
            set { _tempTaskID = value; }
            get { return _tempTaskID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReportCode
        {
            set { _reportCode = value; }
            get { return _reportCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReportName
        {
            set { _reportName = value; }
            get { return _reportName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormCode
        {
            set { _formCode = value; }
            get { return _formCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MN
        {
            set { _MN = value; }
            get { return _MN; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReportCreateDate
        {
            set { _reportCreateDate = value; }
            get { return _reportCreateDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReportUpdateDate
        {
            set { _reportUpdateDate = value; }
            get { return _reportUpdateDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReportUpLoadDate
        {
            set { _reportUpLoadDate = value; }
            get { return _reportUpLoadDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OperateStatus
        {
            set { _operateStatus = value; }
            get { return _operateStatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid TaskGuid
        {
            set { _taskguid = value; }
            get { return _taskguid; }
        }
        #endregion Model
    }
}
