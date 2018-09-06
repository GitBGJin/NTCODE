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
	public partial class DustHazeDayEntity : IMonitoringBusinessEntityProperty
	{
		private string m_dayReportUid;
		public virtual string DayReportUid
		{
			get
			{
				return this.m_dayReportUid;
			}
			set
			{
				this.m_dayReportUid = value;
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
		
		private DateTime? m_dateTime;
		public virtual DateTime? DateTime
		{
			get
			{
				return this.m_dateTime;
			}
			set
			{
				this.m_dateTime = value;
			}
		}
		
		private decimal? m_vis;
		public virtual decimal? Vis
		{
			get
			{
				return this.m_vis;
			}
			set
			{
				this.m_vis = value;
			}
		}
		
		private decimal? m_rH;
		public virtual decimal? RH
		{
			get
			{
				return this.m_rH;
			}
			set
			{
				this.m_rH = value;
			}
		}
		
		private decimal? m_pM2d5;
		public virtual decimal? PM2d5
		{
			get
			{
				return this.m_pM2d5;
			}
			set
			{
				this.m_pM2d5 = value;
			}
		}
		
		private decimal? m_pM1;
		public virtual decimal? PM1
		{
			get
			{
				return this.m_pM1;
			}
			set
			{
				this.m_pM1 = value;
			}
		}
		
		private decimal? m_kext;
		public virtual decimal? Kext
		{
			get
			{
				return this.m_kext;
			}
			set
			{
				this.m_kext = value;
			}
		}
		
		private int? m_isDustHaze;
		public virtual int? IsDustHaze
		{
			get
			{
				return this.m_isDustHaze;
			}
			set
			{
				this.m_isDustHaze = value;
			}
		}
		
		private string m_dustHazeGrade;
		public virtual string DustHazeGrade
		{
			get
			{
				return this.m_dustHazeGrade;
			}
			set
			{
				this.m_dustHazeGrade = value;
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
		
	}
}
#pragma warning restore 1591
