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
using SmartEP.DomainModel.WaterAutoMonitoring;

namespace SmartEP.DomainModel.WaterAutoMonitoring	
{
	public partial class InfectantBy60Entity : IWaterAutoMonitoringEntityProperty
	{
		private long m_id;
		public virtual long Id
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
			}
		}
		
		private int m_pointId;
		public virtual int PointId
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
		
		private DateTime m_tstamp;
		public virtual DateTime Tstamp
		{
			get
			{
				return this.m_tstamp;
			}
			set
			{
				this.m_tstamp = value;
			}
		}
		
		private DateTime m_receiveTime;
		public virtual DateTime ReceiveTime
		{
			get
			{
				return this.m_receiveTime;
			}
			set
			{
				this.m_receiveTime = value;
			}
		}
		
		private string m_status;
		public virtual string Status
		{
			get
			{
				return this.m_status;
			}
			set
			{
				this.m_status = value;
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
		
	}
}
#pragma warning restore 1591
