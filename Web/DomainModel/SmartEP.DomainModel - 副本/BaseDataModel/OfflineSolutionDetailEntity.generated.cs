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
	public partial class OfflineSolutionDetailEntity : IStandardEntityProperty
	{
		private int m_id;
		public virtual int Id
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
		
		private string m_rowGuid;
		public virtual string RowGuid
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
		
		private string m_offlineSolutionUid;
		public virtual string OfflineSolutionUid
		{
			get
			{
				return this.m_offlineSolutionUid;
			}
			set
			{
				this.m_offlineSolutionUid = value;
			}
		}
		
		private string m_solutionUid;
		public virtual string SolutionUid
		{
			get
			{
				return this.m_solutionUid;
			}
			set
			{
				this.m_solutionUid = value;
			}
		}
		
		private string m_gradeUid;
		public virtual string GradeUid
		{
			get
			{
				return this.m_gradeUid;
			}
			set
			{
				this.m_gradeUid = value;
			}
		}
		
		private int? m_offLineTimeSpan;
		public virtual int? OffLineTimeSpan
		{
			get
			{
				return this.m_offLineTimeSpan;
			}
			set
			{
				this.m_offLineTimeSpan = value;
			}
		}
		
		private string m_useForUid;
		public virtual string UseForUid
		{
			get
			{
				return this.m_useForUid;
			}
			set
			{
				this.m_useForUid = value;
			}
		}
		
		private bool? m_enableOrNot;
		public virtual bool? EnableOrNot
		{
			get
			{
				return this.m_enableOrNot;
			}
			set
			{
				this.m_enableOrNot = value;
			}
		}
		
		private bool? m_notifyOrNot;
		public virtual bool? NotifyOrNot
		{
			get
			{
				return this.m_notifyOrNot;
			}
			set
			{
				this.m_notifyOrNot = value;
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
		
		private string m_rowStatus;
		public virtual string RowStatus
		{
			get
			{
				return this.m_rowStatus;
			}
			set
			{
				this.m_rowStatus = value;
			}
		}
		
		private DateTime? m_addDate;
		public virtual DateTime? AddDate
		{
			get
			{
				return this.m_addDate;
			}
			set
			{
				this.m_addDate = value;
			}
		}
		
		private string m_addUserGuid;
		public virtual string AddUserGuid
		{
			get
			{
				return this.m_addUserGuid;
			}
			set
			{
				this.m_addUserGuid = value;
			}
		}
		
		private string m_addUserName;
		public virtual string AddUserName
		{
			get
			{
				return this.m_addUserName;
			}
			set
			{
				this.m_addUserName = value;
			}
		}
		
		private string m_addOUGuid;
		public virtual string AddOUGuid
		{
			get
			{
				return this.m_addOUGuid;
			}
			set
			{
				this.m_addOUGuid = value;
			}
		}
		
		private string m_addOUName;
		public virtual string AddOUName
		{
			get
			{
				return this.m_addOUName;
			}
			set
			{
				this.m_addOUName = value;
			}
		}
		
		private RuleSolutionEntity m_ruleSolutionEntity;
		public virtual RuleSolutionEntity RuleSolutionEntity
		{
			get
			{
				return this.m_ruleSolutionEntity;
			}
			set
			{
				this.m_ruleSolutionEntity = value;
			}
		}
		
	}
}
#pragma warning restore 1591
