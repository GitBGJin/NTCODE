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
	public partial class TB_HourReport
	{
		private string _hourReportUid;
		public virtual string HourReportUid
		{
			get
			{
				return this._hourReportUid;
			}
			set
			{
				this._hourReportUid = value;
			}
		}
		
		private int? _pointId;
		public virtual int? PointId
		{
			get
			{
				return this._pointId;
			}
			set
			{
				this._pointId = value;
			}
		}
		
		private DateTime? _reportDateTime;
		public virtual DateTime? ReportDateTime
		{
			get
			{
				return this._reportDateTime;
			}
			set
			{
				this._reportDateTime = value;
			}
		}
		
		private DateTime? _tstamp;
		public virtual DateTime? Tstamp
		{
			get
			{
				return this._tstamp;
			}
			set
			{
				this._tstamp = value;
			}
		}
		
		private int? _hourOfDay;
		public virtual int? HourOfDay
		{
			get
			{
				return this._hourOfDay;
			}
			set
			{
				this._hourOfDay = value;
			}
		}
		
		private string _pollutantCode;
		public virtual string PollutantCode
		{
			get
			{
				return this._pollutantCode;
			}
			set
			{
				this._pollutantCode = value;
			}
		}
		
		private decimal? _pollutantValue;
		public virtual decimal? PollutantValue
		{
			get
			{
				return this._pollutantValue;
			}
			set
			{
				this._pollutantValue = value;
			}
		}
		
		private string _dataFlag;
		public virtual string DataFlag
		{
			get
			{
				return this._dataFlag;
			}
			set
			{
				this._dataFlag = value;
			}
		}
		
		private string _auditFlag;
		public virtual string AuditFlag
		{
			get
			{
				return this._auditFlag;
			}
			set
			{
				this._auditFlag = value;
			}
		}
		
		private int? _eQI;
		public virtual int? EQI
		{
			get
			{
				return this._eQI;
			}
			set
			{
				this._eQI = value;
			}
		}
		
		private string _isExternalData;
		public virtual string IsExternalData
		{
			get
			{
				return this._isExternalData;
			}
			set
			{
				this._isExternalData = value;
			}
		}
		
		private string _monitoringDataTypeCode;
		public virtual string MonitoringDataTypeCode
		{
			get
			{
				return this._monitoringDataTypeCode;
			}
			set
			{
				this._monitoringDataTypeCode = value;
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
		
		private int? _eTLFlag;
		public virtual int? ETLFlag
		{
			get
			{
				return this._eTLFlag;
			}
			set
			{
				this._eTLFlag = value;
			}
		}
		
		private long _id;
		public virtual long Id
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
		
	}
}
#pragma warning restore 1591
