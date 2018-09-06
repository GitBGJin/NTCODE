#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using SmartEP.DomainModel.AirMonitoringBusiness;

namespace SmartEP.DomainModel.AirMonitoringBusiness	
{
	public partial class WeekReportEntity : IAirMonitoringBusinessEntityProperty
	{
		private string m_weekReportUid;
		public virtual string WeekReportUid
		{
			get
			{
				return this.m_weekReportUid;
			}
			set
			{
				this.m_weekReportUid = value;
			}
		}
		
		private int? m_pointId;
		public virtual int? PointId
		{
			get
			{
				return this.m_pointId;
			}
			set
			{
				this.m_pointId = value;
			}
		}
		
		private DateTime? m_reportDateTime;
		public virtual DateTime? ReportDateTime
		{
			get
			{
				return this.m_reportDateTime;
			}
			set
			{
				this.m_reportDateTime = value;
			}
		}
		
		private int? m_weekOfYear;
		public virtual int? WeekOfYear
		{
			get
			{
				return this.m_weekOfYear;
			}
			set
			{
				this.m_weekOfYear = value;
			}
		}
		
		private string m_reportTypeUid;
		public virtual string ReportTypeUid
		{
			get
			{
				return this.m_reportTypeUid;
			}
			set
			{
				this.m_reportTypeUid = value;
			}
		}
		
		private string m_pollutantCode;
		public virtual string PollutantCode
		{
			get
			{
				return this.m_pollutantCode;
			}
			set
			{
				this.m_pollutantCode = value;
			}
		}
		
		private decimal? m_pollutantValue;
		public virtual decimal? PollutantValue
		{
			get
			{
				return this.m_pollutantValue;
			}
			set
			{
				this.m_pollutantValue = value;
			}
		}
		
		private int? m_eQI;
		public virtual int? EQI
		{
			get
			{
				return this.m_eQI;
			}
			set
			{
				this.m_eQI = value;
			}
		}
		
		private string m_monitoringDataTypeCode;
		public virtual string MonitoringDataTypeCode
		{
			get
			{
				return this.m_monitoringDataTypeCode;
			}
			set
			{
				this.m_monitoringDataTypeCode = value;
			}
		}
		
		private int? m_orderByNum;
		public virtual int? OrderByNum
		{
			get
			{
				return this.m_orderByNum;
			}
			set
			{
				this.m_orderByNum = value;
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
		
		private string m_creatUser;
		public virtual string CreatUser
		{
			get
			{
				return this.m_creatUser;
			}
			set
			{
				this.m_creatUser = value;
			}
		}
		
		private DateTime? m_creatDateTime;
		public virtual DateTime? CreatDateTime
		{
			get
			{
				return this.m_creatDateTime;
			}
			set
			{
				this.m_creatDateTime = value;
			}
		}
		
		private string m_updateUser;
		public virtual string UpdateUser
		{
			get
			{
				return this.m_updateUser;
			}
			set
			{
				this.m_updateUser = value;
			}
		}
		
		private DateTime? m_updateDateTime;
		public virtual DateTime? UpdateDateTime
		{
			get
			{
				return this.m_updateDateTime;
			}
			set
			{
				this.m_updateDateTime = value;
			}
		}
		
		private int? m_year;
		public virtual int? Year
		{
			get
			{
				return this.m_year;
			}
			set
			{
				this.m_year = value;
			}
		}
		
	}
}
#pragma warning restore 1591
