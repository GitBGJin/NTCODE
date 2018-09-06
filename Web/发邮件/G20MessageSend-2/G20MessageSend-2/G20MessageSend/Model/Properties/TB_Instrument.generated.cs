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

namespace BaseDataModel	
{
	public partial class TB_Instrument
	{
		private int _id;
		[System.ComponentModel.DataAnnotations.Required()]
		[System.ComponentModel.DataAnnotations.Key()]
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
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		[System.ComponentModel.DataAnnotations.Required()]
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
		
		private string _instrumentName;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string InstrumentName
		{
			get
			{
				return this._instrumentName;
			}
			set
			{
				this._instrumentName = value;
			}
		}
		
		private string _applyTypeUid;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string ApplyTypeUid
		{
			get
			{
				return this._applyTypeUid;
			}
			set
			{
				this._applyTypeUid = value;
			}
		}
		
		private string _businessTypeUid;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string BusinessTypeUid
		{
			get
			{
				return this._businessTypeUid;
			}
			set
			{
				this._businessTypeUid = value;
			}
		}
		
		private string _instrumentTypeUid;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string InstrumentTypeUid
		{
			get
			{
				return this._instrumentTypeUid;
			}
			set
			{
				this._instrumentTypeUid = value;
			}
		}
		
		private string _brand;
		[System.ComponentModel.DataAnnotations.StringLength(20)]
		public virtual string Brand
		{
			get
			{
				return this._brand;
			}
			set
			{
				this._brand = value;
			}
		}
		
		private string _factory;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string Factory
		{
			get
			{
				return this._factory;
			}
			set
			{
				this._factory = value;
			}
		}
		
		private string _producePlace;
		[System.ComponentModel.DataAnnotations.StringLength(20)]
		public virtual string ProducePlace
		{
			get
			{
				return this._producePlace;
			}
			set
			{
				this._producePlace = value;
			}
		}
		
		private string _supplier;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string Supplier
		{
			get
			{
				return this._supplier;
			}
			set
			{
				this._supplier = value;
			}
		}
		
		private string _productNumber;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string ProductNumber
		{
			get
			{
				return this._productNumber;
			}
			set
			{
				this._productNumber = value;
			}
		}
		
		private decimal? _referencePrice;
		public virtual decimal? ReferencePrice
		{
			get
			{
				return this._referencePrice;
			}
			set
			{
				this._referencePrice = value;
			}
		}
		
		private string _useConditions;
		public virtual string UseConditions
		{
			get
			{
				return this._useConditions;
			}
			set
			{
				this._useConditions = value;
			}
		}
		
		private string _measuringRange;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string MeasuringRange
		{
			get
			{
				return this._measuringRange;
			}
			set
			{
				this._measuringRange = value;
			}
		}
		
		private string _nicetyRate;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string NicetyRate
		{
			get
			{
				return this._nicetyRate;
			}
			set
			{
				this._nicetyRate = value;
			}
		}
		
		private string _controlMeasures;
		public virtual string ControlMeasures
		{
			get
			{
				return this._controlMeasures;
			}
			set
			{
				this._controlMeasures = value;
			}
		}
		
		private string _useMethod;
		public virtual string UseMethod
		{
			get
			{
				return this._useMethod;
			}
			set
			{
				this._useMethod = value;
			}
		}
		
		private decimal? _maintenanceCyc;
		public virtual decimal? MaintenanceCyc
		{
			get
			{
				return this._maintenanceCyc;
			}
			set
			{
				this._maintenanceCyc = value;
			}
		}
		
		private string _maintenanceContent;
		public virtual string MaintenanceContent
		{
			get
			{
				return this._maintenanceContent;
			}
			set
			{
				this._maintenanceContent = value;
			}
		}
		
		private decimal? _inspectPeriod;
		public virtual decimal? InspectPeriod
		{
			get
			{
				return this._inspectPeriod;
			}
			set
			{
				this._inspectPeriod = value;
			}
		}
		
		private string _inspectMethod;
		public virtual string InspectMethod
		{
			get
			{
				return this._inspectMethod;
			}
			set
			{
				this._inspectMethod = value;
			}
		}
		
		private decimal? _originCyc;
		public virtual decimal? OriginCyc
		{
			get
			{
				return this._originCyc;
			}
			set
			{
				this._originCyc = value;
			}
		}
		
		private string _originTypeUid;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string OriginTypeUid
		{
			get
			{
				return this._originTypeUid;
			}
			set
			{
				this._originTypeUid = value;
			}
		}
		
		private string _originUnit;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string OriginUnit
		{
			get
			{
				return this._originUnit;
			}
			set
			{
				this._originUnit = value;
			}
		}
		
		private string _introduction;
		public virtual string Introduction
		{
			get
			{
				return this._introduction;
			}
			set
			{
				this._introduction = value;
			}
		}
		
		private string _techniqueParameters;
		public virtual string TechniqueParameters
		{
			get
			{
				return this._techniqueParameters;
			}
			set
			{
				this._techniqueParameters = value;
			}
		}
		
		private string _keyFeatures;
		public virtual string KeyFeatures
		{
			get
			{
				return this._keyFeatures;
			}
			set
			{
				this._keyFeatures = value;
			}
		}
		
		private string _attachmentUid;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string AttachmentUid
		{
			get
			{
				return this._attachmentUid;
			}
			set
			{
				this._attachmentUid = value;
			}
		}
		
		private string _instructionPaper;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string InstructionPaper
		{
			get
			{
				return this._instructionPaper;
			}
			set
			{
				this._instructionPaper = value;
			}
		}
		
		private string _instructionSoft;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string InstructionSoft
		{
			get
			{
				return this._instructionSoft;
			}
			set
			{
				this._instructionSoft = value;
			}
		}
		
		private string _qualifiedCertificate;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string QualifiedCertificate
		{
			get
			{
				return this._qualifiedCertificate;
			}
			set
			{
				this._qualifiedCertificate = value;
			}
		}
		
		private string _packingBill;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string PackingBill
		{
			get
			{
				return this._packingBill;
			}
			set
			{
				this._packingBill = value;
			}
		}
		
		private int? _yearsRange;
		public virtual int? YearsRange
		{
			get
			{
				return this._yearsRange;
			}
			set
			{
				this._yearsRange = value;
			}
		}
		
		private string _guarantee;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string Guarantee
		{
			get
			{
				return this._guarantee;
			}
			set
			{
				this._guarantee = value;
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
		[System.ComponentModel.DataAnnotations.StringLength(500)]
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
		[System.ComponentModel.DataAnnotations.StringLength(10)]
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
		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
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
		[System.ComponentModel.DataAnnotations.StringLength(10)]
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
		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
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
		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
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
		[System.ComponentModel.DataAnnotations.StringLength(50)]
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
		[System.ComponentModel.DataAnnotations.StringLength(50)]
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
		[System.ComponentModel.DataAnnotations.StringLength(50)]
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
		[System.ComponentModel.DataAnnotations.StringLength(50)]
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
		
		private string _rowStatus1;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
		public virtual string RowStatus1
		{
			get
			{
				return this._rowStatus1;
			}
			set
			{
				this._rowStatus1 = value;
			}
		}
		
		private string _rowStatus;
		[System.ComponentModel.DataAnnotations.StringLength(50)]
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
		
		private bool? _showInMenu;
		public virtual bool? ShowInMenu
		{
			get
			{
				return this._showInMenu;
			}
			set
			{
				this._showInMenu = value;
			}
		}
		
	}
}
#pragma warning restore 1591
