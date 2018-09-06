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
	public partial class AuditMonitoringPointEntity : IMonitoringBusinessEntityProperty
	{
		private string m_auditMonitoringPointUid;
		public virtual string AuditMonitoringPointUid
		{
			get
			{
				return this.m_auditMonitoringPointUid;
			}
			set
			{
				this.m_auditMonitoringPointUid = value;
			}
		}
		
		private string m_applicationUid;
		public virtual string ApplicationUid
		{
			get
			{
				return this.m_applicationUid;
			}
			set
			{
				this.m_applicationUid = value;
			}
		}
		
		private string m_monitoringPointUid;
		public virtual string MonitoringPointUid
		{
			get
			{
				return this.m_monitoringPointUid;
			}
			set
			{
				this.m_monitoringPointUid = value;
			}
		}
		
		private string m_auditTypeUid;
		public virtual string AuditTypeUid
		{
			get
			{
				return this.m_auditTypeUid;
			}
			set
			{
				this.m_auditTypeUid = value;
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
		
		private int? m_pointType;
		public virtual int? PointType
		{
			get
			{
				return this.m_pointType;
			}
			set
			{
				this.m_pointType = value;
			}
		}
		
		private IList<AuditMonitoringPointPollutantEntity> m_auditMonitoringPointPollutantEntities = new List<AuditMonitoringPointPollutantEntity>();
		public virtual IList<AuditMonitoringPointPollutantEntity> AuditMonitoringPointPollutantEntities
		{
			get
			{
				return this.m_auditMonitoringPointPollutantEntities;
			}
		}
		
	}
}
#pragma warning restore 1591
