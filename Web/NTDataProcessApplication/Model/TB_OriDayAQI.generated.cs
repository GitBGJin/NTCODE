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
	public partial class TB_OriDayAQI
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
		
		private DateTime? _dateTime;
		public virtual DateTime? DateTime
		{
			get
			{
				return this._dateTime;
			}
			set
			{
				this._dateTime = value;
			}
		}
		
		private string _sO2;
		public virtual string SO2
		{
			get
			{
				return this._sO2;
			}
			set
			{
				this._sO2 = value;
			}
		}
		
		private string _sO2_IAQI;
		public virtual string SO2_IAQI
		{
			get
			{
				return this._sO2_IAQI;
			}
			set
			{
				this._sO2_IAQI = value;
			}
		}
		
		private string _nO2;
		public virtual string NO2
		{
			get
			{
				return this._nO2;
			}
			set
			{
				this._nO2 = value;
			}
		}
		
		private string _nO2_IAQI;
		public virtual string NO2_IAQI
		{
			get
			{
				return this._nO2_IAQI;
			}
			set
			{
				this._nO2_IAQI = value;
			}
		}
		
		private string _pM10;
		public virtual string PM10
		{
			get
			{
				return this._pM10;
			}
			set
			{
				this._pM10 = value;
			}
		}
		
		private string _pM10_IAQI;
		public virtual string PM10_IAQI
		{
			get
			{
				return this._pM10_IAQI;
			}
			set
			{
				this._pM10_IAQI = value;
			}
		}
		
		private string _cO;
		public virtual string CO
		{
			get
			{
				return this._cO;
			}
			set
			{
				this._cO = value;
			}
		}
		
		private string _cO_IAQI;
		public virtual string CO_IAQI
		{
			get
			{
				return this._cO_IAQI;
			}
			set
			{
				this._cO_IAQI = value;
			}
		}
		
		private string _maxOneHourO3;
		public virtual string MaxOneHourO3
		{
			get
			{
				return this._maxOneHourO3;
			}
			set
			{
				this._maxOneHourO3 = value;
			}
		}
		
		private string _maxOneHourO3_IAQI;
		public virtual string MaxOneHourO3_IAQI
		{
			get
			{
				return this._maxOneHourO3_IAQI;
			}
			set
			{
				this._maxOneHourO3_IAQI = value;
			}
		}
		
		private string _max8HourO3;
		public virtual string Max8HourO3
		{
			get
			{
				return this._max8HourO3;
			}
			set
			{
				this._max8HourO3 = value;
			}
		}
		
		private string _max8HourO3_IAQI;
		public virtual string Max8HourO3_IAQI
		{
			get
			{
				return this._max8HourO3_IAQI;
			}
			set
			{
				this._max8HourO3_IAQI = value;
			}
		}
		
		private string _pM25;
		public virtual string PM25
		{
			get
			{
				return this._pM25;
			}
			set
			{
				this._pM25 = value;
			}
		}
		
		private string _pM25_IAQI;
		public virtual string PM25_IAQI
		{
			get
			{
				return this._pM25_IAQI;
			}
			set
			{
				this._pM25_IAQI = value;
			}
		}
		
		private string _aQIValue;
		public virtual string AQIValue
		{
			get
			{
				return this._aQIValue;
			}
			set
			{
				this._aQIValue = value;
			}
		}
		
		private string _primaryPollutant;
		public virtual string PrimaryPollutant
		{
			get
			{
				return this._primaryPollutant;
			}
			set
			{
				this._primaryPollutant = value;
			}
		}
		
		private string _range;
		public virtual string Range
		{
			get
			{
				return this._range;
			}
			set
			{
				this._range = value;
			}
		}
		
		private string _rGBValue;
		public virtual string RGBValue
		{
			get
			{
				return this._rGBValue;
			}
			set
			{
				this._rGBValue = value;
			}
		}
		
		private string _picturePath;
		public virtual string PicturePath
		{
			get
			{
				return this._picturePath;
			}
			set
			{
				this._picturePath = value;
			}
		}
		
		private string _class;
		public virtual string Class
		{
			get
			{
				return this._class;
			}
			set
			{
				this._class = value;
			}
		}
		
		private string _grade;
		public virtual string Grade
		{
			get
			{
				return this._grade;
			}
			set
			{
				this._grade = value;
			}
		}
		
		private string _healthEffect;
		public virtual string HealthEffect
		{
			get
			{
				return this._healthEffect;
			}
			set
			{
				this._healthEffect = value;
			}
		}
		
		private string _takeStep;
		public virtual string TakeStep
		{
			get
			{
				return this._takeStep;
			}
			set
			{
				this._takeStep = value;
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
		
	}
}
#pragma warning restore 1591
