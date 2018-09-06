using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.Framework
{
    /// <summary>
    /// 名称：OM_ReagentInItemDetailEntiy.cs
    /// 创建人:刘长敏
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    ///标液配置表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public partial class OM_ReagentInItemDetailEntiy
    {
        public OM_ReagentInItemDetailEntiy()
        { }
        private int m_iD;
        public virtual int ID
        {
            get
            {
                return this.m_iD;
            }
            set
            {
                this.m_iD = value;
            }
        }

        private string m_rowguid;
        public virtual string RowGuid
        {
            get
            {
                return this.m_rowguid;
            }
            set
            {
                this.m_rowguid = value;
            }
        }

        private string m_fixCode;
        public virtual string FixCode
        {
            get
            {
                return this.m_fixCode;
            }
            set
            {
                this.m_fixCode = value;
            }
        }

        private string m_billItemGuid;
        public virtual string BillItemGuid
        {
            get
            {
                return this.m_billItemGuid;
            }
            set
            {
                this.m_billItemGuid = value;
            }
        }

        private DateTime? m_manufactureDate;
        public virtual DateTime? ManufactureDate
        {
            get
            {
                return this.m_manufactureDate;
            }
            set
            {
                this.m_manufactureDate = value;
            }
        }

        private DateTime? m_endValidDate;
        public virtual DateTime? EndValidDate
        {
            get
            {
                return this.m_endValidDate;
            }
            set
            {
                this.m_endValidDate = value;
            }
        }

        private string m_isPrint;
        public virtual string IsPrint
        {
            get
            {
                return this.m_isPrint;
            }
            set
            {
                this.m_isPrint = value;
            }
        }
    }
}
