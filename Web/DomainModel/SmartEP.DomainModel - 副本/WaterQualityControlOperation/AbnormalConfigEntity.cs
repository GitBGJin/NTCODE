using System;
namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：AbnormalConfigEntity.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-9
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 异常配置表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
	[Serializable]
	public partial class AbnormalConfigEntity
	{
         public AbnormalConfigEntity()
		{}
        #region Model
        private int _id;
        private Guid _rowguid;
        private string _abnormalname;
        private string _abnormalitemtype;
        private string _rowstatus;
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
        public Guid RowGuid
        {
            set { _rowguid = value; }
            get { return _rowguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AbnormalName
        {
            set { _abnormalname = value; }
            get { return _abnormalname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AbnormalItemType
        {
            set { _abnormalitemtype = value; }
            get { return _abnormalitemtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RowStatus
        {
            set { _rowstatus = value; }
            get { return _rowstatus; }
        }
        #endregion Model

	}
}

