using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：TaskFileInfoEntity.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务文件信息表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public class TaskFileInfoEntity
    {
        #region Model
        private int _id;
        private string _missionid;
        private string _actionid;
        private int? _pointid;
        private string _pointname;
        private string _filepath;
        private string _filename;
        private byte[] _filestreams;
        private string _uploaduser;
        private DateTime? _uploaddate;
        private string _remark;
        private int? _orderbynum;
        private string _description;
        private string _creatuser;
        private DateTime? _creatdatetime;
        private string _updateuser;
        private DateTime? _updatedatetime;
        private string _taskCode;
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
        public string MissionID
        {
            set { _missionid = value; }
            get { return _missionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ActionID
        {
            set { _actionid = value; }
            get { return _actionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PointId
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
        public string FilePath
        {
            set { _filepath = value; }
            get { return _filepath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            set { _filename = value; }
            get { return _filename; }
        }
        /// <summary>
        /// 文件流字符串
        /// </summary>
        public byte[] FileStreams
        {
            set { _filestreams = value; }
            get { return _filestreams; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UpLoadUser
        {
            set { _uploaduser = value; }
            get { return _uploaduser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpLoadDate
        {
            set { _uploaddate = value; }
            get { return _uploaddate; }
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
        public int? OrderByNum
        {
            set { _orderbynum = value; }
            get { return _orderbynum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreatUser
        {
            set { _creatuser = value; }
            get { return _creatuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreatDateTime
        {
            set { _creatdatetime = value; }
            get { return _creatdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UpdateUser
        {
            set { _updateuser = value; }
            get { return _updateuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateDateTime
        {
            set { _updatedatetime = value; }
            get { return _updatedatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskCode
        {
            set { _taskCode = value; }
            get { return _taskCode; }
        }
        #endregion Model
    }
}
