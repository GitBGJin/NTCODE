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

namespace SmartEP.DomainModel.BaseData	
{
	public partial class DT_ConfigIDInfoEntity
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
		
		private long? m_maxid;
		public virtual long? Maxid
		{
			get
			{
				return this.m_maxid;
			}
			set
			{
				this.m_maxid = value;
			}
		}
		
		private string m_memo;
		public virtual string Memo
		{
			get
			{
				return this.m_memo;
			}
			set
			{
				this.m_memo = value;
			}
		}
		
		private DateTime? m_createdatetime;
		public virtual DateTime? Createdatetime
		{
			get
			{
				return this.m_createdatetime;
			}
			set
			{
				this.m_createdatetime = value;
			}
		}
		
		private DateTime? m_updatedatetime;
		public virtual DateTime? Updatedatetime
		{
			get
			{
				return this.m_updatedatetime;
			}
			set
			{
				this.m_updatedatetime = value;
			}
		}
		
	}
}
#pragma warning restore 1591
