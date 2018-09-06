using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：InstrumentUsedRecordEntity.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器使用记录实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public partial class InstrumentUsedRecordEntity //: IBaseEntityProperty
    {
        public InstrumentUsedRecordEntity()
        { }
        #region Model
        private Guid _id;
        private int? _pointid;
        private string _instrumentnumber;
        private string _useduser;
        private DateTime _useddate;
        private DateTime? _shouldreturndate;
        private DateTime? _realreturndate;
        private string _usecontent;
        private string _creatuser;
        private DateTime? _creatdatetime;
        private string _updateuser;
        private DateTime? _updatedatetime;
        private string _taskCode;
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
        public int? PointId
        {
            set { _pointid = value; }
            get { return _pointid; }
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
        public string UsedUser
        {
            set { _useduser = value; }
            get { return _useduser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UsedDate
        {
            set { _useddate = value; }
            get { return _useddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ShouldReturnDate
        {
            set { _shouldreturndate = value; }
            get { return _shouldreturndate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RealReturnDate
        {
            set { _realreturndate = value; }
            get { return _realreturndate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UseContent
        {
            set { _usecontent = value; }
            get { return _usecontent; }
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

        //public virtual int? OrderByNum { get; set; }
        //public virtual string Description { get; set; }
    }
}

