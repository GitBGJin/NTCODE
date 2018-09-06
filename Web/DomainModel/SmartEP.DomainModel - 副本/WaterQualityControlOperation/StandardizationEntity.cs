using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：StandardizationEntity.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 标定实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    [Serializable]
    public partial class StandardizationEntity
    {
        public StandardizationEntity()
        { }
        #region Model
        private int _id;
        private string _missionid;
        private string _actionid;
        private string _pointid;
        private string _pointname;
        private string _factorid;
        private string _factorname;
        private string _standardvalue;
        private string _standardizationvalue;
        private decimal? _comparisonValue;
        private string _standardizationpeople;
        private DateTime? _standardizationdate;
        private string _isqualified;
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
        public string FactorId
        {
            set { _factorid = value; }
            get { return _factorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FactorName
        {
            set { _factorname = value; }
            get { return _factorname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StandardValue
        {
            set { _standardvalue = value; }
            get { return _standardvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StandardizationValue
        {
            set { _standardizationvalue = value; }
            get { return _standardizationvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ComparisonValue
        {
            set { _comparisonValue = value; }
            get { return _comparisonValue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StandardizationPeople
        {
            set { _standardizationpeople = value; }
            get { return _standardizationpeople; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StandardizationDate
        {
            set { _standardizationdate = value; }
            get { return _standardizationdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IsQualified
        {
            set { _isqualified = value; }
            get { return _isqualified; }
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
