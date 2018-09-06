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
	public partial class InstrumentOPDatumEntity : IMonitoringBusinessEntityProperty
	{
		private Guid m_rowGuid;
		public virtual Guid RowGuid
		{
			get
			{
				return this.m_rowGuid;
			}
			set
			{
				this.m_rowGuid = value;
			}
		}
		
		private string m_instanceName;
		public virtual string InstanceName
		{
			get
			{
				return this.m_instanceName;
			}
			set
			{
				this.m_instanceName = value;
			}
		}
		
		private string m_fixedAssetNumber;
		public virtual string FixedAssetNumber
		{
			get
			{
				return this.m_fixedAssetNumber;
			}
			set
			{
				this.m_fixedAssetNumber = value;
			}
		}
		
		private DateTime? m_operateDate;
		public virtual DateTime? OperateDate
		{
			get
			{
				return this.m_operateDate;
			}
			set
			{
				this.m_operateDate = value;
			}
		}
		
		private string m_operateUser;
		public virtual string OperateUser
		{
			get
			{
				return this.m_operateUser;
			}
			set
			{
				this.m_operateUser = value;
			}
		}
		
		private string m_operateStatus;
		public virtual string OperateStatus
		{
			get
			{
				return this.m_operateStatus;
			}
			set
			{
				this.m_operateStatus = value;
			}
		}
		
		private string m_operateNote;
		public virtual string OperateNote
		{
			get
			{
				return this.m_operateNote;
			}
			set
			{
				this.m_operateNote = value;
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
		
		private string m_mN;
		public virtual string MN
		{
			get
			{
				return this.m_mN;
			}
			set
			{
				this.m_mN = value;
			}
		}
		
		private string m_pointName;
		public virtual string PointName
		{
			get
			{
				return this.m_pointName;
			}
			set
			{
				this.m_pointName = value;
			}
		}
		
		private string m_pointGuid;
		public virtual string PointGuid
		{
			get
			{
				return this.m_pointGuid;
			}
			set
			{
				this.m_pointGuid = value;
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
		
		private string m_statusNote;
		public virtual string StatusNote
		{
			get
			{
				return this.m_statusNote;
			}
			set
			{
				this.m_statusNote = value;
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
