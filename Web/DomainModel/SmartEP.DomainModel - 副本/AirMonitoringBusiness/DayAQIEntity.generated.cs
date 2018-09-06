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
using SmartEP.DomainModel.AirMonitoringBusiness;

namespace SmartEP.DomainModel.AirMonitoringBusiness	
{
	public partial class DayAQIEntity : IAirMonitoringBusinessEntityProperty
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
		
		private string m_sO2;
		public virtual string SO2
		{
			get
			{
				return this.m_sO2;
			}
			set
			{
				this.m_sO2 = value;
			}
		}
		
		private string m_sO2_IAQI;
		public virtual string SO2_IAQI
		{
			get
			{
				return this.m_sO2_IAQI;
			}
			set
			{
				this.m_sO2_IAQI = value;
			}
		}
		
		private string m_nO2;
		public virtual string NO2
		{
			get
			{
				return this.m_nO2;
			}
			set
			{
				this.m_nO2 = value;
			}
		}
		
		private string m_nO2_IAQI;
		public virtual string NO2_IAQI
		{
			get
			{
				return this.m_nO2_IAQI;
			}
			set
			{
				this.m_nO2_IAQI = value;
			}
		}
		
		private string m_pM10;
		public virtual string PM10
		{
			get
			{
				return this.m_pM10;
			}
			set
			{
				this.m_pM10 = value;
			}
		}
		
		private string m_pM10_IAQI;
		public virtual string PM10_IAQI
		{
			get
			{
				return this.m_pM10_IAQI;
			}
			set
			{
				this.m_pM10_IAQI = value;
			}
		}
		
		private string m_cO;
		public virtual string CO
		{
			get
			{
				return this.m_cO;
			}
			set
			{
				this.m_cO = value;
			}
		}
		
		private string m_cO_IAQI;
		public virtual string CO_IAQI
		{
			get
			{
				return this.m_cO_IAQI;
			}
			set
			{
				this.m_cO_IAQI = value;
			}
		}
		
		private string m_maxOneHourO3;
		public virtual string MaxOneHourO3
		{
			get
			{
				return this.m_maxOneHourO3;
			}
			set
			{
				this.m_maxOneHourO3 = value;
			}
		}
		
		private string m_maxOneHourO3_IAQI;
		public virtual string MaxOneHourO3_IAQI
		{
			get
			{
				return this.m_maxOneHourO3_IAQI;
			}
			set
			{
				this.m_maxOneHourO3_IAQI = value;
			}
		}
		
		private string m_max8HourO3;
		public virtual string Max8HourO3
		{
			get
			{
				return this.m_max8HourO3;
			}
			set
			{
				this.m_max8HourO3 = value;
			}
		}
		
		private string m_max8HourO3_IAQI;
		public virtual string Max8HourO3_IAQI
		{
			get
			{
				return this.m_max8HourO3_IAQI;
			}
			set
			{
				this.m_max8HourO3_IAQI = value;
			}
		}
		
		private string m_pM25;
		public virtual string PM25
		{
			get
			{
				return this.m_pM25;
			}
			set
			{
				this.m_pM25 = value;
			}
		}
		
		private string m_pM25_IAQI;
		public virtual string PM25_IAQI
		{
			get
			{
				return this.m_pM25_IAQI;
			}
			set
			{
				this.m_pM25_IAQI = value;
			}
		}
		
		private string m_aQIValue;
		public virtual string AQIValue
		{
			get
			{
				return this.m_aQIValue;
			}
			set
			{
				this.m_aQIValue = value;
			}
		}
		
		private string m_primaryPollutant;
		public virtual string PrimaryPollutant
		{
			get
			{
				return this.m_primaryPollutant;
			}
			set
			{
				this.m_primaryPollutant = value;
			}
		}
		
		private string m_range;
		public virtual string Range
		{
			get
			{
				return this.m_range;
			}
			set
			{
				this.m_range = value;
			}
		}
		
		private string m_rGBValue;
		public virtual string RGBValue
		{
			get
			{
				return this.m_rGBValue;
			}
			set
			{
				this.m_rGBValue = value;
			}
		}
		
		private string m_picturePath;
		public virtual string PicturePath
		{
			get
			{
				return this.m_picturePath;
			}
			set
			{
				this.m_picturePath = value;
			}
		}
		
		private string m_class;
		public virtual string Class
		{
			get
			{
				return this.m_class;
			}
			set
			{
				this.m_class = value;
			}
		}
		
		private string m_grade;
		public virtual string Grade
		{
			get
			{
				return this.m_grade;
			}
			set
			{
				this.m_grade = value;
			}
		}
		
		private string m_healthEffect;
		public virtual string HealthEffect
		{
			get
			{
				return this.m_healthEffect;
			}
			set
			{
				this.m_healthEffect = value;
			}
		}
		
		private string m_takeStep;
		public virtual string TakeStep
		{
			get
			{
				return this.m_takeStep;
			}
			set
			{
				this.m_takeStep = value;
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
