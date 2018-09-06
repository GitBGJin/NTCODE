using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：SamplingRecordDetailEntity.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 采样记录详情表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public partial class SamplingRecordDetailEntity //:  IBaseEntityProperty
    {
        public SamplingRecordDetailEntity()
        { }
        #region Model
        private Guid _id;
        private Guid _samplingmainguid;
        private int _pointId;
        private string _pointName;
        private string _samplingUser;
        private string _samplenumber;
        private DateTime? _samplingtime;
        private string _samplingposition;
        private string _instrumentnumber;
        private string _pollutantcode;
        private decimal? _pollutantvalue;
        private string _samplingType;
        private string _comparisonanalysisproject;
        private string _remark;
        private string _creatuser;
        private DateTime? _creatdatetime;
        private string _updateuser;
        private DateTime? _updatedatetime;
        /// <summary>
        /// 
        /// </summary>
        public Guid id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid SamplingMainGuid
        {
            set { _samplingmainguid = value; }
            get { return _samplingmainguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PointId
        {
            set { _pointId = value; }
            get { return _pointId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointName
        {
            set { _pointName = value; }
            get { return _pointName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SamplingUser
        {
            set { _samplingUser = value; }
            get { return _samplingUser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SampleNumber
        {
            set { _samplenumber = value; }
            get { return _samplenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SamplingTime
        {
            set { _samplingtime = value; }
            get { return _samplingtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SamplingPosition
        {
            set { _samplingposition = value; }
            get { return _samplingposition; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InstrumentNumber
        {
            set { _instrumentnumber = value; }
            get { return _instrumentnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PollutantCode
        {
            set { _pollutantcode = value; }
            get { return _pollutantcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PollutantValue
        {
            set { _pollutantvalue = value; }
            get { return _pollutantvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SamplingType
        {
            set { _samplingType = value; }
            get { return _samplingType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ComparisonAnalysisProject
        {
            set { _comparisonanalysisproject = value; }
            get { return _comparisonanalysisproject; }
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
        #endregion Model

        //public virtual int? OrderByNum { get; set; }
        //public virtual string Description { get; set; }
    }
}

