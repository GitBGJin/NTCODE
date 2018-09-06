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

namespace Model	
{
	public partial class TB_ExcessiveSetting
	{
		private int _id;
		public virtual int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
		
		private string _rowGuid;
		public virtual string RowGuid
		{
			get
			{
				return this._rowGuid;
			}
			set
			{
				this._rowGuid = value;
			}
		}
		
		private string _excessiveUid;
		public virtual string ExcessiveUid
		{
			get
			{
				return this._excessiveUid;
			}
			set
			{
				this._excessiveUid = value;
			}
		}
		
		private string _monitoringPointUid;
		public virtual string MonitoringPointUid
		{
			get
			{
				return this._monitoringPointUid;
			}
			set
			{
				this._monitoringPointUid = value;
			}
		}
		
		private string _instrumentChannelsUid;
		public virtual string InstrumentChannelsUid
		{
			get
			{
				return this._instrumentChannelsUid;
			}
			set
			{
				this._instrumentChannelsUid = value;
			}
		}
		
		private string _notifyGradeUid;
		public virtual string NotifyGradeUid
		{
			get
			{
				return this._notifyGradeUid;
			}
			set
			{
				this._notifyGradeUid = value;
			}
		}
		
		private string _applicationUid;
		public virtual string ApplicationUid
		{
			get
			{
				return this._applicationUid;
			}
			set
			{
				this._applicationUid = value;
			}
		}
		
		private string _dataTypeUid;
		public virtual string DataTypeUid
		{
			get
			{
				return this._dataTypeUid;
			}
			set
			{
				this._dataTypeUid = value;
			}
		}
		
		private decimal? _advanceLow;
		public virtual decimal? AdvanceLow
		{
			get
			{
				return this._advanceLow;
			}
			set
			{
				this._advanceLow = value;
			}
		}
		
		private decimal? _advanceUpper;
		public virtual decimal? AdvanceUpper
		{
			get
			{
				return this._advanceUpper;
			}
			set
			{
				this._advanceUpper = value;
			}
		}
		
		private string _advanceRange;
		public virtual string AdvanceRange
		{
			get
			{
				return this._advanceRange;
			}
			set
			{
				this._advanceRange = value;
			}
		}
		
		private decimal? _excessiveUpper;
		public virtual decimal? ExcessiveUpper
		{
			get
			{
				return this._excessiveUpper;
			}
			set
			{
				this._excessiveUpper = value;
			}
		}
		
		private decimal? _excessiveLow;
		public virtual decimal? ExcessiveLow
		{
			get
			{
				return this._excessiveLow;
			}
			set
			{
				this._excessiveLow = value;
			}
		}
		
		private string _excessiveRange;
		public virtual string ExcessiveRange
		{
			get
			{
				return this._excessiveRange;
			}
			set
			{
				this._excessiveRange = value;
			}
		}
		
		private string _replaceStatus;
		public virtual string ReplaceStatus
		{
			get
			{
				return this._replaceStatus;
			}
			set
			{
				this._replaceStatus = value;
			}
		}
		
		private string _standardType;
		public virtual string StandardType
		{
			get
			{
				return this._standardType;
			}
			set
			{
				this._standardType = value;
			}
		}
		
		private decimal? _excessiveRatio;
		public virtual decimal? ExcessiveRatio
		{
			get
			{
				return this._excessiveRatio;
			}
			set
			{
				this._excessiveRatio = value;
			}
		}
		
		private string _useForUid;
		public virtual string UseForUid
		{
			get
			{
				return this._useForUid;
			}
			set
			{
				this._useForUid = value;
			}
		}
		
		private bool? _enableOrNot;
		public virtual bool? EnableOrNot
		{
			get
			{
				return this._enableOrNot;
			}
			set
			{
				this._enableOrNot = value;
			}
		}
		
		private bool? _notifyOrNot;
		public virtual bool? NotifyOrNot
		{
			get
			{
				return this._notifyOrNot;
			}
			set
			{
				this._notifyOrNot = value;
			}
		}
		
		private int? _orderByNum;
		public virtual int? OrderByNum
		{
			get
			{
				return this._orderByNum;
			}
			set
			{
				this._orderByNum = value;
			}
		}
		
		private string _description;
		public virtual string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}
		
		private string _creatUser;
		public virtual string CreatUser
		{
			get
			{
				return this._creatUser;
			}
			set
			{
				this._creatUser = value;
			}
		}
		
		private DateTime? _creatDateTime;
		public virtual DateTime? CreatDateTime
		{
			get
			{
				return this._creatDateTime;
			}
			set
			{
				this._creatDateTime = value;
			}
		}
		
		private string _updateUser;
		public virtual string UpdateUser
		{
			get
			{
				return this._updateUser;
			}
			set
			{
				this._updateUser = value;
			}
		}
		
		private DateTime? _updateDateTime;
		public virtual DateTime? UpdateDateTime
		{
			get
			{
				return this._updateDateTime;
			}
			set
			{
				this._updateDateTime = value;
			}
		}
		
		private DateTime? _addDate;
		public virtual DateTime? AddDate
		{
			get
			{
				return this._addDate;
			}
			set
			{
				this._addDate = value;
			}
		}
		
		private string _addUserGuid;
		public virtual string AddUserGuid
		{
			get
			{
				return this._addUserGuid;
			}
			set
			{
				this._addUserGuid = value;
			}
		}
		
		private string _addUserName;
		public virtual string AddUserName
		{
			get
			{
				return this._addUserName;
			}
			set
			{
				this._addUserName = value;
			}
		}
		
		private string _addOUGuid;
		public virtual string AddOUGuid
		{
			get
			{
				return this._addOUGuid;
			}
			set
			{
				this._addOUGuid = value;
			}
		}
		
		private string _addOUName;
		public virtual string AddOUName
		{
			get
			{
				return this._addOUName;
			}
			set
			{
				this._addOUName = value;
			}
		}
		
		private string _rowStatus;
		public virtual string RowStatus
		{
			get
			{
				return this._rowStatus;
			}
			set
			{
				this._rowStatus = value;
			}
		}
		
	}
}
#pragma warning restore 1591
