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
	public partial class StandardSolutionCheckEntity : IMonitoringBusinessEntityProperty
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
		
		private string m_missionID;
		public virtual string MissionID
		{
			get
			{
				return this.m_missionID;
			}
			set
			{
				this.m_missionID = value;
			}
		}
		
		private string m_actionID;
		public virtual string ActionID
		{
			get
			{
				return this.m_actionID;
			}
			set
			{
				this.m_actionID = value;
			}
		}
		
		private string m_sampleNumber;
		public virtual string SampleNumber
		{
			get
			{
				return this.m_sampleNumber;
			}
			set
			{
				this.m_sampleNumber = value;
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
		
		private string m_pollutantCode;
		public virtual string PollutantCode
		{
			get
			{
				return this.m_pollutantCode;
			}
			set
			{
				this.m_pollutantCode = value;
			}
		}
		
		private string m_unit;
		public virtual string Unit
		{
			get
			{
				return this.m_unit;
			}
			set
			{
				this.m_unit = value;
			}
		}
		
		private decimal? m_pollutantValue;
		public virtual decimal? PollutantValue
		{
			get
			{
				return this.m_pollutantValue;
			}
			set
			{
				this.m_pollutantValue = value;
			}
		}
		
		private decimal? m_standardValue;
		public virtual decimal? StandardValue
		{
			get
			{
				return this.m_standardValue;
			}
			set
			{
				this.m_standardValue = value;
			}
		}
		
		private decimal? m_addValue;
		public virtual decimal? AddValue
		{
			get
			{
				return this.m_addValue;
			}
			set
			{
				this.m_addValue = value;
			}
		}
		
		private decimal? m_pollutantValueAdd;
		public virtual decimal? PollutantValueAdd
		{
			get
			{
				return this.m_pollutantValueAdd;
			}
			set
			{
				this.m_pollutantValueAdd = value;
			}
		}
		
		private decimal? m_relativeOffset;
		public virtual decimal? RelativeOffset
		{
			get
			{
				return this.m_relativeOffset;
			}
			set
			{
				this.m_relativeOffset = value;
			}
		}
		
		private decimal? m_absoluteOffset;
		public virtual decimal? AbsoluteOffset
		{
			get
			{
				return this.m_absoluteOffset;
			}
			set
			{
				this.m_absoluteOffset = value;
			}
		}
		
		private decimal? m_offsetValue;
		public virtual decimal? OffsetValue
		{
			get
			{
				return this.m_offsetValue;
			}
			set
			{
				this.m_offsetValue = value;
			}
		}
		
		private string m_evaluate;
		public virtual string Evaluate
		{
			get
			{
				return this.m_evaluate;
			}
			set
			{
				this.m_evaluate = value;
			}
		}
		
		private string m_testType;
		public virtual string TestType
		{
			get
			{
				return this.m_testType;
			}
			set
			{
				this.m_testType = value;
			}
		}
		
		private DateTime? m_dateStartTime;
		public virtual DateTime? DateStartTime
		{
			get
			{
				return this.m_dateStartTime;
			}
			set
			{
				this.m_dateStartTime = value;
			}
		}
		
		private DateTime? m_dateEndTime;
		public virtual DateTime? DateEndTime
		{
			get
			{
				return this.m_dateEndTime;
			}
			set
			{
				this.m_dateEndTime = value;
			}
		}
		
		private DateTime? m_tstamp;
		public virtual DateTime? Tstamp
		{
			get
			{
				return this.m_tstamp;
			}
			set
			{
				this.m_tstamp = value;
			}
		}
		
		private string m_tester;
		public virtual string Tester
		{
			get
			{
				return this.m_tester;
			}
			set
			{
				this.m_tester = value;
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
		
		private string m_pollutantName;
		public virtual string PollutantName
		{
			get
			{
				return this.m_pollutantName;
			}
			set
			{
				this.m_pollutantName = value;
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
		
		private string m_instrumentName;
		public virtual string InstrumentName
		{
			get
			{
				return this.m_instrumentName;
			}
			set
			{
				this.m_instrumentName = value;
			}
		}
		
		private string m_instrumentId;
		public virtual string InstrumentId
		{
			get
			{
				return this.m_instrumentId;
			}
			set
			{
				this.m_instrumentId = value;
			}
		}
		
		private string m_instrumentNum;
		public virtual string InstrumentNum
		{
			get
			{
				return this.m_instrumentNum;
			}
			set
			{
				this.m_instrumentNum = value;
			}
		}
		
		private decimal? m_universalValue4;
		public virtual decimal? UniversalValue4
		{
			get
			{
				return this.m_universalValue4;
			}
			set
			{
				this.m_universalValue4 = value;
			}
		}
		
		private decimal? m_universalValue3;
		public virtual decimal? UniversalValue3
		{
			get
			{
				return this.m_universalValue3;
			}
			set
			{
				this.m_universalValue3 = value;
			}
		}
		
		private decimal? m_universalValue2;
		public virtual decimal? UniversalValue2
		{
			get
			{
				return this.m_universalValue2;
			}
			set
			{
				this.m_universalValue2 = value;
			}
		}
		
		private decimal? m_universalValue1;
		public virtual decimal? UniversalValue1
		{
			get
			{
				return this.m_universalValue1;
			}
			set
			{
				this.m_universalValue1 = value;
			}
		}
		
		private decimal? m_dilutionFactor;
		public virtual decimal? DilutionFactor
		{
			get
			{
				return this.m_dilutionFactor;
			}
			set
			{
				this.m_dilutionFactor = value;
			}
		}
		
		private string m_taskCode;
		public virtual string TaskCode
		{
			get
			{
				return this.m_taskCode;
			}
			set
			{
				this.m_taskCode = value;
			}
		}
		
		private string m_position;
		public virtual string Position
		{
			get
			{
				return this.m_position;
			}
			set
			{
				this.m_position = value;
			}
		}
		
		private string m_goal;
		public virtual string Goal
		{
			get
			{
				return this.m_goal;
			}
			set
			{
				this.m_goal = value;
			}
		}
		
		private string m_batchNumber;
		public virtual string BatchNumber
		{
			get
			{
				return this.m_batchNumber;
			}
			set
			{
				this.m_batchNumber = value;
			}
		}
		
		private string m_mark;
		public virtual string Mark
		{
			get
			{
				return this.m_mark;
			}
			set
			{
				this.m_mark = value;
			}
		}
		
	}
}
#pragma warning restore 1591
