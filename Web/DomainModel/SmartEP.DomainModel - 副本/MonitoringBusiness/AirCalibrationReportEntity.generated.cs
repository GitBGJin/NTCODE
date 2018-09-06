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
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.DomainModel.MonitoringBusiness	
{
	public partial class AirCalibrationReportEntity : IMonitoringBusinessEntityProperty
	{
		private string m_calibrationReportGuid;
		public virtual string CalibrationReportGuid
		{
			get
			{
				return this.m_calibrationReportGuid;
			}
			set
			{
				this.m_calibrationReportGuid = value;
			}
		}
		
		private int? m_pointID;
		public virtual int? PointID
		{
			get
			{
				return this.m_pointID;
			}
			set
			{
				this.m_pointID = value;
			}
		}
		
		private DateTime? m_startTime;
		public virtual DateTime? StartTime
		{
			get
			{
				return this.m_startTime;
			}
			set
			{
				this.m_startTime = value;
			}
		}
		
		private DateTime? m_endTime;
		public virtual DateTime? EndTime
		{
			get
			{
				return this.m_endTime;
			}
			set
			{
				this.m_endTime = value;
			}
		}
		
		private int? m_persistentTime;
		public virtual int? PersistentTime
		{
			get
			{
				return this.m_persistentTime;
			}
			set
			{
				this.m_persistentTime = value;
			}
		}
		
		private string m_sequence;
		public virtual string Sequence
		{
			get
			{
				return this.m_sequence;
			}
			set
			{
				this.m_sequence = value;
			}
		}
		
		private string m_calNameCode;
		public virtual string CalNameCode
		{
			get
			{
				return this.m_calNameCode;
			}
			set
			{
				this.m_calNameCode = value;
			}
		}
		
		private string m_calTypeCode;
		public virtual string CalTypeCode
		{
			get
			{
				return this.m_calTypeCode;
			}
			set
			{
				this.m_calTypeCode = value;
			}
		}
		
		private string m_calConc;
		public virtual string CalConc
		{
			get
			{
				return this.m_calConc;
			}
			set
			{
				this.m_calConc = value;
			}
		}
		
		private string m_calFlow;
		public virtual string CalFlow
		{
			get
			{
				return this.m_calFlow;
			}
			set
			{
				this.m_calFlow = value;
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
		
		private decimal? m_deviationValue;
		public virtual decimal? DeviationValue
		{
			get
			{
				return this.m_deviationValue;
			}
			set
			{
				this.m_deviationValue = value;
			}
		}
		
		private int? m_deviationPPB;
		public virtual int? DeviationPPB
		{
			get
			{
				return this.m_deviationPPB;
			}
			set
			{
				this.m_deviationPPB = value;
			}
		}
		
		private decimal? m_deviationRatio;
		public virtual decimal? DeviationRatio
		{
			get
			{
				return this.m_deviationRatio;
			}
			set
			{
				this.m_deviationRatio = value;
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
		
	}
}
#pragma warning restore 1591
