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

namespace SmartEP.DomainModel.MonitoringBusiness	
{
	public partial class V_AuditOperatorSettingEntity
	{
		private string m_toolTip;
		public virtual string ToolTip
		{
			get
			{
				return this.m_toolTip;
			}
			set
			{
				this.m_toolTip = value;
			}
		}
		
		private string m_statusName;
		public virtual string StatusName
		{
			get
			{
				return this.m_statusName;
			}
			set
			{
				this.m_statusName = value;
			}
		}
		
		private string m_statusIdentify;
		public virtual string StatusIdentify
		{
			get
			{
				return this.m_statusIdentify;
			}
			set
			{
				this.m_statusIdentify = value;
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
		
		private string m_operatorName;
		public virtual string OperatorName
		{
			get
			{
				return this.m_operatorName;
			}
			set
			{
				this.m_operatorName = value;
			}
		}
		
		private string m_auditOperatorUid;
		public virtual string AuditOperatorUid
		{
			get
			{
				return this.m_auditOperatorUid;
			}
			set
			{
				this.m_auditOperatorUid = value;
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
		
		private string m_auditType;
		public virtual string AuditType
		{
			get
			{
				return this.m_auditType;
			}
			set
			{
				this.m_auditType = value;
			}
		}
		
	}
}
#pragma warning restore 1591
