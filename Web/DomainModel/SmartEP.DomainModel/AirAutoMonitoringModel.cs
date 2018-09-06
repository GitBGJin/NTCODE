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
using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.Core.Interfaces;

namespace SmartEP.DomainModel.AirAutoMonitoring	
{
	public partial class AirAutoMonitoringModel : OpenAccessContext, IAirAutoMonitoringModelUnitOfWork, ICustomOrmContext
	{
		private static string connectionStringName = @"AMS_AirAutoMonitorConnection";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("AirAutoMonitoringModel.rlinq");
		
		public AirAutoMonitoringModel()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public AirAutoMonitoringModel(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public AirAutoMonitoringModel(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public AirAutoMonitoringModel(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public AirAutoMonitoringModel(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<OriginalPacketRequestEntity> OriginalPacketRequestEntities 
		{
			get
			{
				return this.GetAll<OriginalPacketRequestEntity>();
			}
		}
		
		public IQueryable<OriginalPacketReceiveEntity> OriginalPacketReceiveEntities 
		{
			get
			{
				return this.GetAll<OriginalPacketReceiveEntity>();
			}
		}
		
		public IQueryable<OriginalPacketBackupEntity> OriginalPacketBackupEntities 
		{
			get
			{
				return this.GetAll<OriginalPacketBackupEntity>();
			}
		}
		
		public IQueryable<InstrumentDataBy60BufferEntity> InstrumentDataBy60BufferEntities 
		{
			get
			{
				return this.GetAll<InstrumentDataBy60BufferEntity>();
			}
		}
		
		public IQueryable<InstrumentDataBy60Entity> InstrumentDataBy60Entities 
		{
			get
			{
				return this.GetAll<InstrumentDataBy60Entity>();
			}
		}
		
		public IQueryable<InfectantByRTEntity> InfectantByRTEntities 
		{
			get
			{
				return this.GetAll<InfectantByRTEntity>();
			}
		}
		
		public IQueryable<InfectantBy60BufferEntity> InfectantBy60BufferEntities 
		{
			get
			{
				return this.GetAll<InfectantBy60BufferEntity>();
			}
		}
		
		public IQueryable<InfectantBy60Entity> InfectantBy60Entities 
		{
			get
			{
				return this.GetAll<InfectantBy60Entity>();
			}
		}
		
		public IQueryable<InfectantBy5BufferEntity> InfectantBy5BufferEntities 
		{
			get
			{
				return this.GetAll<InfectantBy5BufferEntity>();
			}
		}
		
		public IQueryable<InfectantBy5Entity> InfectantBy5Entities 
		{
			get
			{
				return this.GetAll<InfectantBy5Entity>();
			}
		}
		
		public IQueryable<InfectantBy30BufferEntity> InfectantBy30BufferEntities 
		{
			get
			{
				return this.GetAll<InfectantBy30BufferEntity>();
			}
		}
		
		public IQueryable<InfectantBy30Entity> InfectantBy30Entities 
		{
			get
			{
				return this.GetAll<InfectantBy30Entity>();
			}
		}
		
		public IQueryable<InfectantBy10BufferEntity> InfectantBy10BufferEntities 
		{
			get
			{
				return this.GetAll<InfectantBy10BufferEntity>();
			}
		}
		
		public IQueryable<InfectantBy10Entity> InfectantBy10Entities 
		{
			get
			{
				return this.GetAll<InfectantBy10Entity>();
			}
		}
		
		public IQueryable<InfectantBy1Entity> InfectantBy1Entities 
		{
			get
			{
				return this.GetAll<InfectantBy1Entity>();
			}
		}
		
		public IQueryable<AirInstrumentStateEntity> AirInstrumentStateEntities 
		{
			get
			{
				return this.GetAll<AirInstrumentStateEntity>();
			}
		}
		
		public IQueryable<AirCalibrationDetailEntity> AirCalibrationDetailEntities 
		{
			get
			{
				return this.GetAll<AirCalibrationDetailEntity>();
			}
		}
		
		public IQueryable<AirCalibrationEntity> AirCalibrationEntities 
		{
			get
			{
				return this.GetAll<AirCalibrationEntity>();
			}
		}
		
		public IQueryable<OriRegionDayAQIReportEntity> OriRegionDayAQIReportEntities 
		{
			get
			{
				return this.GetAll<OriRegionDayAQIReportEntity>();
			}
		}
		
		public IQueryable<OriRegionHourAQIEntity> OriRegionHourAQIEntities 
		{
			get
			{
				return this.GetAll<OriRegionHourAQIEntity>();
			}
		}
		
		public IQueryable<OriHourAQIEntity> OriHourAQIEntities 
		{
			get
			{
				return this.GetAll<OriHourAQIEntity>();
			}
		}
		
		public IQueryable<OriDayAQIEntity> OriDayAQIEntities 
		{
			get
			{
				return this.GetAll<OriDayAQIEntity>();
			}
		}
		
		public IQueryable<InfectantBy1BufferEntity> InfectantBy1BufferEntities 
		{
			get
			{
				return this.GetAll<InfectantBy1BufferEntity>();
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
		/// Allows you to customize the BackendConfiguration of AirAutoMonitoringModel.
		/// </summary>
		/// <param name="config">The BackendConfiguration of AirAutoMonitoringModel.</param>
		static partial void CustomizeBackendConfiguration(ref BackendConfiguration config);
		
	}
	
	public interface IAirAutoMonitoringModelUnitOfWork : IUnitOfWork
	{
		IQueryable<OriginalPacketRequestEntity> OriginalPacketRequestEntities
		{
			get;
		}
		IQueryable<OriginalPacketReceiveEntity> OriginalPacketReceiveEntities
		{
			get;
		}
		IQueryable<OriginalPacketBackupEntity> OriginalPacketBackupEntities
		{
			get;
		}
		IQueryable<InstrumentDataBy60BufferEntity> InstrumentDataBy60BufferEntities
		{
			get;
		}
		IQueryable<InstrumentDataBy60Entity> InstrumentDataBy60Entities
		{
			get;
		}
		IQueryable<InfectantByRTEntity> InfectantByRTEntities
		{
			get;
		}
		IQueryable<InfectantBy60BufferEntity> InfectantBy60BufferEntities
		{
			get;
		}
		IQueryable<InfectantBy60Entity> InfectantBy60Entities
		{
			get;
		}
		IQueryable<InfectantBy5BufferEntity> InfectantBy5BufferEntities
		{
			get;
		}
		IQueryable<InfectantBy5Entity> InfectantBy5Entities
		{
			get;
		}
		IQueryable<InfectantBy30BufferEntity> InfectantBy30BufferEntities
		{
			get;
		}
		IQueryable<InfectantBy30Entity> InfectantBy30Entities
		{
			get;
		}
		IQueryable<InfectantBy10BufferEntity> InfectantBy10BufferEntities
		{
			get;
		}
		IQueryable<InfectantBy10Entity> InfectantBy10Entities
		{
			get;
		}
		IQueryable<InfectantBy1Entity> InfectantBy1Entities
		{
			get;
		}
		IQueryable<AirInstrumentStateEntity> AirInstrumentStateEntities
		{
			get;
		}
		IQueryable<AirCalibrationDetailEntity> AirCalibrationDetailEntities
		{
			get;
		}
		IQueryable<AirCalibrationEntity> AirCalibrationEntities
		{
			get;
		}
		IQueryable<OriRegionDayAQIReportEntity> OriRegionDayAQIReportEntities
		{
			get;
		}
		IQueryable<OriRegionHourAQIEntity> OriRegionHourAQIEntities
		{
			get;
		}
		IQueryable<OriHourAQIEntity> OriHourAQIEntities
		{
			get;
		}
		IQueryable<OriDayAQIEntity> OriDayAQIEntities
		{
			get;
		}
		IQueryable<InfectantBy1BufferEntity> InfectantBy1BufferEntities
		{
			get;
		}
	}
}
#pragma warning restore 1591
