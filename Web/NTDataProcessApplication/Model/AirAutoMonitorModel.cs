﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ContextGenerator.ttinclude code generation file.
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
using Model;

namespace Model	
{
	public partial class AirAutoMonitorModel : OpenAccessContext, IAirAutoMonitorModelUnitOfWork
	{
		private static string connectionStringName = @"AMS_AirAutoMonitorConnection";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("AirAutoMonitorModel.rlinq");
		
		public AirAutoMonitorModel()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public AirAutoMonitorModel(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public AirAutoMonitorModel(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public AirAutoMonitorModel(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public AirAutoMonitorModel(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<TB_InfectantBy60Buffer> TB_InfectantBy60Buffers 
		{
			get
			{
				return this.GetAll<TB_InfectantBy60Buffer>();
			}
		}
		
		public IQueryable<TB_InfectantBy5Buffer> TB_InfectantBy5Buffers 
		{
			get
			{
				return this.GetAll<TB_InfectantBy5Buffer>();
			}
		}
		
		public IQueryable<TB_InfectantBy1Buffer> TB_InfectantBy1Buffers 
		{
			get
			{
				return this.GetAll<TB_InfectantBy1Buffer>();
			}
		}
		
		public IQueryable<TB_OriHourAQI> TB_OriHourAQIs 
		{
			get
			{
				return this.GetAll<TB_OriHourAQI>();
			}
		}
		
		public IQueryable<TB_InfectantByMonth> TB_InfectantByMonths 
		{
			get
			{
				return this.GetAll<TB_InfectantByMonth>();
			}
		}
		
		public IQueryable<TB_InfectantByDay> TB_InfectantByDays 
		{
			get
			{
				return this.GetAll<TB_InfectantByDay>();
			}
		}
		
		public IQueryable<TB_OriDayAQI> TB_OriDayAQIs 
		{
			get
			{
				return this.GetAll<TB_OriDayAQI>();
			}
		}
		
		public IQueryable<TB_InfectantBy60> TB_InfectantBy60 
		{
			get
			{
				return this.GetAll<TB_InfectantBy60>();
			}
		}
		
		public IQueryable<TB_InfectantBy5> TB_InfectantBy5 
		{
			get
			{
				return this.GetAll<TB_InfectantBy5>();
			}
		}
		
		public IQueryable<TB_InfectantBy1> TB_InfectantBy1 
		{
			get
			{
				return this.GetAll<TB_InfectantBy1>();
			}
		}
		
		public IQueryable<TB_OriRegionHourAQI> TB_OriRegionHourAQIs 
		{
			get
			{
				return this.GetAll<TB_OriRegionHourAQI>();
			}
		}
		
		public IQueryable<TB_OriRegionDayAQIReport> TB_OriRegionDayAQIReports 
		{
			get
			{
				return this.GetAll<TB_OriRegionDayAQIReport>();
			}
		}
		
		public IQueryable<TB_RealAQINearby> TB_RealAQINearbies 
		{
			get
			{
				return this.GetAll<TB_RealAQINearby>();
			}
		}
		
		public static BackendConfiguration GetBackendConfiguration()
		{
			BackendConfiguration backend = new BackendConfiguration();
			backend.Backend = "MsSql";
			backend.ProviderName = "System.Data.SqlClient";
		
			CustomizeBackendConfiguration(ref backend);
		
			return backend;
		}
		
		/// <summary>
		/// Allows you to customize the BackendConfiguration of AirAutoMonitorModel.
		/// </summary>
		/// <param name="config">The BackendConfiguration of AirAutoMonitorModel.</param>
		static partial void CustomizeBackendConfiguration(ref BackendConfiguration config);
		
	}
	
	public interface IAirAutoMonitorModelUnitOfWork : IUnitOfWork
	{
		IQueryable<TB_InfectantBy60Buffer> TB_InfectantBy60Buffers
		{
			get;
		}
		IQueryable<TB_InfectantBy5Buffer> TB_InfectantBy5Buffers
		{
			get;
		}
		IQueryable<TB_InfectantBy1Buffer> TB_InfectantBy1Buffers
		{
			get;
		}
		IQueryable<TB_OriHourAQI> TB_OriHourAQIs
		{
			get;
		}
		IQueryable<TB_InfectantByMonth> TB_InfectantByMonths
		{
			get;
		}
		IQueryable<TB_InfectantByDay> TB_InfectantByDays
		{
			get;
		}
		IQueryable<TB_OriDayAQI> TB_OriDayAQIs
		{
			get;
		}
		IQueryable<TB_InfectantBy60> TB_InfectantBy60
		{
			get;
		}
		IQueryable<TB_InfectantBy5> TB_InfectantBy5
		{
			get;
		}
		IQueryable<TB_InfectantBy1> TB_InfectantBy1
		{
			get;
		}
		IQueryable<TB_OriRegionHourAQI> TB_OriRegionHourAQIs
		{
			get;
		}
		IQueryable<TB_OriRegionDayAQIReport> TB_OriRegionDayAQIReports
		{
			get;
		}
		IQueryable<TB_RealAQINearby> TB_RealAQINearbies
		{
			get;
		}
	}
}
#pragma warning restore 1591
