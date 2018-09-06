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
	public partial class ReportQualifiedRateByHourEntity : IMonitoringBusinessEntityProperty
	{
		private string m_hourQualifiedRateUid;
		public virtual string HourQualifiedRateUid
		{
			get
			{
				return this.m_hourQualifiedRateUid;
			}
			set
			{
				this.m_hourQualifiedRateUid = value;
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
		
		private int? m_collectionNumber;
		public virtual int? CollectionNumber
		{
			get
			{
				return this.m_collectionNumber;
			}
			set
			{
				this.m_collectionNumber = value;
			}
		}
		
		private int? m_qualifiedNumber;
		public virtual int? QualifiedNumber
		{
			get
			{
				return this.m_qualifiedNumber;
			}
			set
			{
				this.m_qualifiedNumber = value;
			}
		}
		
		private int? m_disqualificationNumber;
		public virtual int? DisqualificationNumber
		{
			get
			{
				return this.m_disqualificationNumber;
			}
			set
			{
				this.m_disqualificationNumber = value;
			}
		}
		
		private decimal? m_qualifiedRate;
		public virtual decimal? QualifiedRate
		{
			get
			{
				return this.m_qualifiedRate;
			}
			set
			{
				this.m_qualifiedRate = value;
			}
		}
		
		private decimal? m_disqualificationRate;
		public virtual decimal? DisqualificationRate
		{
			get
			{
				return this.m_disqualificationRate;
			}
			set
			{
				this.m_disqualificationRate = value;
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
