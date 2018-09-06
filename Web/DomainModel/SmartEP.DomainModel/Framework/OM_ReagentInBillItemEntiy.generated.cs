using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.Framework
{
    /// <summary>
    /// 名称：OM_ReagentInBillItemEntiy.cs
    /// 创建人:刘长敏
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    ///标液配置表实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
    [Serializable]
    public partial class OM_ReagentInBillItemEntiy
    {
        public OM_ReagentInBillItemEntiy()
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

        private string m_billguid;
        public virtual string BillGuid
        {
            get
            {
                return this.m_billguid;
            }
            set
            {
                this.m_billguid = value;
            }
        }

        private string m_reagentguid;
        public virtual string ReagentGuid
        {
            get
            {
                return this.m_reagentguid;
            }
            set
            {
                this.m_reagentguid = value;
            }
        }

        private decimal? m_itemcount;
        public virtual decimal? ItemCount
        {
            get
            {
                return this.m_itemcount;
            }
            set
            {
                this.m_itemcount = value;
            }
        }

        private string m_reagentname;
        public virtual string ReagentName
        {
            get
            {
                return this.m_reagentname;
            }
            set
            {
                this.m_reagentname = value;
            }
        }

        private string m_reagentparentname;
        public virtual string ReagentParentName
        {
            get
            {
                return this.m_reagentparentname;
            }
            set
            {
                this.m_reagentparentname = value;
            }
        }

        private string m_productSN;
        public virtual string ProductSN
        {
            get
            {
                return this.m_productSN;
            }
            set
            {
                this.m_productSN = value;
            }
        }

        private int? m_guaranteenumber;
        public virtual int? GuaranteeNumber
        {
            get
            {
                return this.m_guaranteenumber;
            }
            set
            {
                this.m_guaranteenumber = value;
            }
        }

        private string m_periodType;
        public virtual string PeriodType
        {
            get
            {
                return this.m_periodType;
            }
            set
            {
                this.m_periodType = value;
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

        private DateTime? m_endvalidDate;
        public virtual DateTime? EndValidDate
        {
            get
            {
                return this.m_endvalidDate;
            }
            set
            {
                this.m_endvalidDate = value;
            }
        }

        private string m_source;
        public virtual string Source
        {
            get
            {
                return this.m_source;
            }
            set
            {
                this.m_source = value;
            }
        }

        private decimal? m_parentCount;
        public virtual decimal? ParentCount
        {
            get
            {
                return this.m_parentCount;
            }
            set
            {
                this.m_parentCount = value;
            }
        }

        private string m_reagentparentGuid;
        public virtual string ReagentParentGuid
        {
            get
            {
                return this.m_reagentparentGuid;
            }
            set
            {
                this.m_reagentparentGuid = value;
            }
        }

        private string m_note;
        public virtual string Note
        {
            get
            {
                return this.m_note;
            }
            set
            {
                this.m_note = value;
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

        private string m_rowstatus;
        public virtual string RowStatus
        {
            get
            {
                return this.m_rowstatus;
            }
            set
            {
                this.m_rowstatus = value;
            }
        }

        private string m_typeName1;
        public virtual string TypeName1
        {
            get
            {
                return this.m_typeName1;
            }
            set
            {
                this.m_typeName1 = value;
            }
        }

        private string m_typeName2;
        public virtual string TypeName2
        {
            get
            {
                return this.m_typeName2;
            }
            set
            {
                this.m_typeName2 = value;
            }
        }

        private string m_middleproductSN;
        public virtual string MiddleProductSN
        {
            get
            {
                return this.m_middleproductSN;
            }
            set
            {
                this.m_middleproductSN = value;
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

        private string m_sourceType;
        public virtual string SourceType
        {
            get
            {
                return this.m_sourceType;
            }
            set
            {
                this.m_sourceType = value;
            }
        }

        private string m_number;
        public virtual string Number
        {
            get
            {
                return this.m_number;
            }
            set
            {
                this.m_number = value;
            }
        }

        private string m_concentration;
        public virtual string Concentration
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

        private string m_multiple;
        public virtual string Multiple
        {
            get
            {
                return this.m_multiple;
            }
            set
            {
                this.m_multiple = value;
            }
        }

        private string m_diluteConcen;
        public virtual string DiluteConcen
        {
            get
            {
                return this.m_diluteConcen;
            }
            set
            {
                this.m_diluteConcen = value;
            }
        }

        private string m_oldproductSN;
        public virtual string OldProductSN
        {
            get
            {
                return this.m_oldproductSN;
            }
            set
            {
                this.m_oldproductSN = value;
            }
        }

        private string m_typename;
        public virtual string TypeName
        {
            get
            {
                return this.m_typename;
            }
            set
            {
                this.m_typename = value;
            }
        }

        private string m_ilution;
        public virtual string Dilution
        {
            get
            {
                return this.m_ilution;
            }
            set
            {
                this.m_ilution = value;
            }
        }
        private string m_unit;
        public virtual string Unit
        {
            get
            {
                return this.m_unit;
            }
            set
            {
                this.m_unit = value;
            }
        }
        private string m_oldsystemSN;
        public virtual string OldSystemSN
        {
            get
            {
                return this.m_oldsystemSN;
            }
            set
            {
                this.m_oldsystemSN = value;
            }
        }
        private string m_description;
        public virtual string Description
        {
            get
            {
                return this.m_description;
            }
            set
            {
                this.m_description = value;
            }
        }
        private string m_actualConcentration;
        public virtual string ActualConcentration
        {
            get
            {
                return this.m_actualConcentration;
            }
            set
            {
                this.m_actualConcentration = value;
            }
        }
        private string m_actualMultiple;
        public virtual string ActualMultiple
        {
            get
            {
                return this.m_actualMultiple;
            }
            set
            {
                this.m_actualMultiple = value;
            }
        }
        private bool? m_isblindSolution;
        public virtual bool? IsBlindSolution
        {
            get
            {
                return this.m_isblindSolution;
            }
            set
            {
                this.m_isblindSolution = value;
            }
        }
        private string m_numberNew;
        public virtual string NumberNew
        {
            get
            {
                return this.m_numberNew;
            }
            set
            {
                this.m_numberNew = value;
            }
        }
        private string m_configPeople;
        public virtual string ConfigPeople
        {
            get
            {
                return this.m_configPeople;
            }
            set
            {
                this.m_configPeople = value;
            }
        }
        private int? m_recordFlag;
        public virtual int? RecordFlag
        {
            get
            {
                return this.m_recordFlag;
            }
            set
            {
                this.m_recordFlag = value;
            }
        }
        private string m_numberUnit;
        public virtual string NumberUnit
        {
            get
            {
                return this.m_numberUnit;
            }
            set
            {
                this.m_numberUnit = value;
            }
        }
        private string m_configAmount;
        public virtual string ConfigAmount
        {
            get
            {
                return this.m_configAmount;
            }
            set
            {
                this.m_configAmount = value;
            }
        }
        private string m_guaranteeUnit;
        public virtual string GuaranteeUnit
        {
            get
            {
                return this.m_guaranteeUnit;
            }
            set
            {
                this.m_guaranteeUnit = value;
            }
        }
    }
}
