using System;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：PartChangeEntity.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 备品备件更换实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    [Serializable]
    public partial class PartChangeEntity
    {
        public PartChangeEntity()
        { }
        #region Model
        private int _id;
        private string _missionid;
        private string _actionid;
        private string _pointid;
        private string _pointname;
        private Guid _instrumentid;
        private string _instrumentname;
        private string _partid;
        private string _partname;
        private string _model;
        private string _purpose;
        private string _changereason;
        private string _oldnumber;
        private string _newnumber;
        private DateTime? _changedate;
        private DateTime? _lastchange;
        private bool _istest;
        private string _changepeople;
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
        public Guid InstrumentId
        {
            set { _instrumentid = value; }
            get { return _instrumentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InstrumentName
        {
            set { _instrumentname = value; }
            get { return _instrumentname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PartId
        {
            set { _partid = value; }
            get { return _partid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PartName
        {
            set { _partname = value; }
            get { return _partname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Model
        {
            set { _model = value; }
            get { return _model; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Purpose
        {
            set { _purpose = value; }
            get { return _purpose; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChangeReason
        {
            set { _changereason = value; }
            get { return _changereason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OldNumber
        {
            set { _oldnumber = value; }
            get { return _oldnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewNumber
        {
            set { _newnumber = value; }
            get { return _newnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ChangeDate
        {
            set { _changedate = value; }
            get { return _changedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastChange
        {
            set { _lastchange = value; }
            get { return _lastchange; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool isTest
        {
            set { _istest = value; }
            get { return _istest; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChangePeople
        {
            set { _changepeople = value; }
            get { return _changepeople; }
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
