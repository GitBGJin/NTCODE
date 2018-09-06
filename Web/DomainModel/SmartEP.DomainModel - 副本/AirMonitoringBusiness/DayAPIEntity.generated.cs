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
	public partial class DayAPIEntity : IAirMonitoringBusinessEntityProperty
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
		
		private string m_o3;
		public virtual string O3
		{
			get
			{
				return this.m_o3;
			}
			set
			{
				this.m_o3 = value;
			}
		}
		
		private string m_aPI_SO2;
		public virtual string API_SO2
		{
			get
			{
				return this.m_aPI_SO2;
			}
			set
			{
				this.m_aPI_SO2 = value;
			}
		}
		
		private string m_aPI_NO2;
		public virtual string API_NO2
		{
			get
			{
				return this.m_aPI_NO2;
			}
			set
			{
				this.m_aPI_NO2 = value;
			}
		}
		
		private string m_aPI_PM10;
		public virtual string API_PM10
		{
			get
			{
				return this.m_aPI_PM10;
			}
			set
			{
				this.m_aPI_PM10 = value;
			}
		}
		
		private string m_aPI_CO;
		public virtual string API_CO
		{
			get
			{
				return this.m_aPI_CO;
			}
			set
			{
				this.m_aPI_CO = value;
			}
		}
		
		private string m_aPI_O3;
		public virtual string API_O3
		{
			get
			{
				return this.m_aPI_O3;
			}
			set
			{
				this.m_aPI_O3 = value;
			}
		}
		
		private string m_aPI_Max;
		public virtual string API_Max
		{
			get
			{
				return this.m_aPI_Max;
			}
			set
			{
				this.m_aPI_Max = value;
			}
		}
		
		private string m_aPI_MaxValue;
		public virtual string API_MaxValue
		{
			get
			{
				return this.m_aPI_MaxValue;
			}
			set
			{
				this.m_aPI_MaxValue = value;
			}
		}
		
		private string m_aPI_Condition;
		public virtual string API_Condition
		{
			get
			{
				return this.m_aPI_Condition;
			}
			set
			{
				this.m_aPI_Condition = value;
			}
		}
		
		private string m_aPI_JiBie;
		public virtual string API_JiBie
		{
			get
			{
				return this.m_aPI_JiBie;
			}
			set
			{
				this.m_aPI_JiBie = value;
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
