using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.Framework
{
      /// <summary>
    /// 名称：OM_ReagentEntiy.cs
    /// 创建人:刘长敏
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    ///标液配置表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public partial class OM_ReagentEntiy
    {
        public OM_ReagentEntiy()
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

        private string m_rowStatus;
        public virtual string RowStatus
        {
            get
            {
                return this.m_rowStatus;
            }
            set
            {
                this.m_rowStatus = value;
            }
        }


        private string m_orgGuid;
        public virtual string OrgGuid
        {
            get
            {
                return this.m_orgGuid;
            }
            set
            {
                this.m_orgGuid = value;
            }
        }

        private string m_reagentName;
        public virtual string ReagentName
        {
            get
            {
                return this.m_reagentName;
            }
            set
            {
                this.m_reagentName = value;
            }
        }


        private string m_alias;
        public virtual string Alias
        {
            get
            {
                return this.m_alias;
            }
            set
            {
                this.m_alias = value;
            }
        }

        private string m_parentType;
        public virtual string ParentType
        {
            get
            {
                return this.m_parentType;
            }
            set
            {
                this.m_parentType = value;
            }
        }

        private string m_code;
        public virtual string Code
        {
            get
            {
                return this.m_code;
            }
            set
            {
                this.m_code = value;
            }
        }
        private string m_reagentModel;
        public virtual string ReagentModel
        {
            get
            {
                return this.m_reagentModel;
            }
            set
            {
                this.m_reagentModel = value;
            }
        }

        private decimal? m_capacity;
        public virtual decimal? Capacity
        {
            get
            {
                return this.m_capacity;
            }
            set
            {
                this.m_capacity = value;
            }
        }
        private string m_capacityUnitGuid;
        public virtual string CapacityUnitGuid
        {
            get
            {
                return this.m_capacityUnitGuid;
            }
            set
            {
                this.m_capacityUnitGuid = value;
            }
        }
        private string m_qualityLevel;
        public virtual string QualityLevel
        {
            get
            {
                return this.m_qualityLevel;
            }
            set
            {
                this.m_qualityLevel = value;
            }
        }
        private string m_reagentType;
        public virtual string ReagentType
        {
            get
            {
                return this.m_reagentType;
            }
            set
            {
                this.m_reagentType = value;
            }
        }
        private decimal? m_concentration;
        public virtual decimal? Concentration
        {
            get
            {
                return this.m_concentration;
            }
            set
            {
                this.m_concentration = value;
            }
        }
        private string m_concentrationUnitGuid;
        public virtual string ConcentrationUnitGuid
        {
            get
            {
                return this.m_concentrationUnitGuid;
            }
            set
            {
                this.m_concentrationUnitGuid = value;
            }
        }
        private string m_typeName;
        public virtual string TypeName
        {
            get
            {
                return this.m_typeName;
            }
            set
            {
                this.m_typeName = value;
            }
        }
    }
}
