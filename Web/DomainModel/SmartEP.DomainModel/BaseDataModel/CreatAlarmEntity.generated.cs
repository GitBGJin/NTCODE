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
using SmartEP.DomainModel.BaseData;

namespace SmartEP.DomainModel.BaseData	
{
	public partial class CreatAlarmEntity : IAlarmNotifyEntityProperty
	{
		private string m_alarmUid;
		public virtual string AlarmUid
		{
			get
			{
				return this.m_alarmUid;
			}
			set
			{
				this.m_alarmUid = value;
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
		
		private DateTime? m_recordDateTime;
		public virtual DateTime? RecordDateTime
		{
			get
			{
				return this.m_recordDateTime;
			}
			set
			{
				this.m_recordDateTime = value;
			}
		}
		
		private string m_alarmSourceUid;
		public virtual string AlarmSourceUid
		{
			get
			{
				return this.m_alarmSourceUid;
			}
			set
			{
				this.m_alarmSourceUid = value;
			}
		}
		
		private string m_alarmEventUid;
		public virtual string AlarmEventUid
		{
			get
			{
				return this.m_alarmEventUid;
			}
			set
			{
				this.m_alarmEventUid = value;
			}
		}
		
		private string m_alarmGradeUid;
		public virtual string AlarmGradeUid
		{
			get
			{
				return this.m_alarmGradeUid;
			}
			set
			{
				this.m_alarmGradeUid = value;
			}
		}
		
		private string m_content;
		public virtual string Content
		{
			get
			{
				return this.m_content;
			}
			set
			{
				this.m_content = value;
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
		
		private string m_itemValue;
		public virtual string ItemValue
		{
			get
			{
				return this.m_itemValue;
			}
			set
			{
				this.m_itemValue = value;
			}
		}
		
		private string m_itemName;
		public virtual string ItemName
		{
			get
			{
				return this.m_itemName;
			}
			set
			{
				this.m_itemName = value;
			}
		}
		
		private string m_dataTypeUid;
		public virtual string DataTypeUid
		{
			get
			{
				return this.m_dataTypeUid;
			}
			set
			{
				this.m_dataTypeUid = value;
			}
		}
		
		private DateTime? m_dealTime;
		public virtual DateTime? DealTime
		{
			get
			{
				return this.m_dealTime;
			}
			set
			{
				this.m_dealTime = value;
			}
		}
		
		private string m_dealMan;
		public virtual string DealMan
		{
			get
			{
				return this.m_dealMan;
			}
			set
			{
				this.m_dealMan = value;
			}
		}
		
		private bool? m_dealFlag;
		public virtual bool? DealFlag
		{
			get
			{
				return this.m_dealFlag;
			}
			set
			{
				this.m_dealFlag = value;
			}
		}
		
		private string m_dealContent;
		public virtual string DealContent
		{
			get
			{
				return this.m_dealContent;
			}
			set
			{
				this.m_dealContent = value;
			}
		}
		
		private DateTime? m_auditTime;
		public virtual DateTime? AuditTime
		{
			get
			{
				return this.m_auditTime;
			}
			set
			{
				this.m_auditTime = value;
			}
		}
		
		private string m_auditMan;
		public virtual string AuditMan
		{
			get
			{
				return this.m_auditMan;
			}
			set
			{
				this.m_auditMan = value;
			}
		}
		
		private bool? m_auditFlag;
		public virtual bool? AuditFlag
		{
			get
			{
				return this.m_auditFlag;
			}
			set
			{
				this.m_auditFlag = value;
			}
		}
		
		private string m_auditContent;
		public virtual string AuditContent
		{
			get
			{
				return this.m_auditContent;
			}
			set
			{
				this.m_auditContent = value;
			}
		}
		
		private string m_sendContent;
		public virtual string SendContent
		{
			get
			{
				return this.m_sendContent;
			}
			set
			{
				this.m_sendContent = value;
			}
		}
		
		private string m_monitoringPoint;
		public virtual string MonitoringPoint
		{
			get
			{
				return this.m_monitoringPoint;
			}
			set
			{
				this.m_monitoringPoint = value;
			}
		}
		
		private string m_itemCode;
		public virtual string ItemCode
		{
			get
			{
				return this.m_itemCode;
			}
			set
			{
				this.m_itemCode = value;
			}
		}
		
		private string m_dealMode;
		public virtual string DealMode
		{
			get
			{
				return this.m_dealMode;
			}
			set
			{
				this.m_dealMode = value;
			}
		}
		
		private string m_auditMode;
		public virtual string AuditMode
		{
			get
			{
				return this.m_auditMode;
			}
			set
			{
				this.m_auditMode = value;
			}
		}
		
		private IList<AlarmInfoEntity> m_alarmInfoEntities = new List<AlarmInfoEntity>();
		public virtual IList<AlarmInfoEntity> AlarmInfoEntities
		{
			get
			{
				return this.m_alarmInfoEntities;
			}
		}
		
	}
}
#pragma warning restore 1591
