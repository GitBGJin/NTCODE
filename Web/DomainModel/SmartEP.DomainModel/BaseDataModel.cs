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
using SmartEP.DomainModel.BaseData;
using SmartEP.Core.Interfaces;

namespace SmartEP.DomainModel.BaseData	
{
    public partial class BaseDataModel : OpenAccessContext, IBaseDataModelUnitOfWork, ICustomOrmContext
	{
		private static string connectionStringName = @"AMS_BaseDataConnection";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("BaseDataModel.rlinq");
		
		public BaseDataModel()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public BaseDataModel(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public BaseDataModel(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public BaseDataModel(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public BaseDataModel(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<ValidDataSettingEntity> ValidDataSettingEntities 
		{
			get
			{
				return this.GetAll<ValidDataSettingEntity>();
			}
		}
		
		public IQueryable<RepeatLimitSettingEntity> RepeatLimitSettingEntities 
		{
			get
			{
				return this.GetAll<RepeatLimitSettingEntity>();
			}
		}
		
		public IQueryable<OutlierSettingEntity> OutlierSettingEntities 
		{
			get
			{
				return this.GetAll<OutlierSettingEntity>();
			}
		}
		
		public IQueryable<OfflineSettingEntity> OfflineSettingEntities 
		{
			get
			{
				return this.GetAll<OfflineSettingEntity>();
			}
		}
		
		public IQueryable<NegativeLimitSettingEntity> NegativeLimitSettingEntities 
		{
			get
			{
				return this.GetAll<NegativeLimitSettingEntity>();
			}
		}
		
		public IQueryable<ExcessiveSettingEntity> ExcessiveSettingEntities 
		{
			get
			{
				return this.GetAll<ExcessiveSettingEntity>();
			}
		}
		
		public IQueryable<BreakSettingEntity> BreakSettingEntities 
		{
			get
			{
				return this.GetAll<BreakSettingEntity>();
			}
		}
		
		public IQueryable<NotifyTypeEntity> NotifyTypeEntities 
		{
			get
			{
				return this.GetAll<NotifyTypeEntity>();
			}
		}
		
		public IQueryable<NotifyStrategyEntity> NotifyStrategyEntities 
		{
			get
			{
				return this.GetAll<NotifyStrategyEntity>();
			}
		}
		
		public IQueryable<NotifySendEntity> NotifySendEntities 
		{
			get
			{
				return this.GetAll<NotifySendEntity>();
			}
		}
		
		public IQueryable<NotifyNumberEntity> NotifyNumberEntities 
		{
			get
			{
				return this.GetAll<NotifyNumberEntity>();
			}
		}
		
		public IQueryable<NotifyGradeEntity> NotifyGradeEntities 
		{
			get
			{
				return this.GetAll<NotifyGradeEntity>();
			}
		}
		
		public IQueryable<NotifyAddressEntity> NotifyAddressEntities 
		{
			get
			{
				return this.GetAll<NotifyAddressEntity>();
			}
		}
		
		public IQueryable<GradeEntity> GradeEntities 
		{
			get
			{
				return this.GetAll<GradeEntity>();
			}
		}
		
		public IQueryable<CreatAlarmEntity> CreatAlarmEntities 
		{
			get
			{
				return this.GetAll<CreatAlarmEntity>();
			}
		}
		
		public IQueryable<AlarmInfoEntity> AlarmInfoEntities 
		{
			get
			{
				return this.GetAll<AlarmInfoEntity>();
			}
		}
		
		public IQueryable<ValidDataSolutionDetailEntity> ValidDataSolutionDetailEntities 
		{
			get
			{
				return this.GetAll<ValidDataSolutionDetailEntity>();
			}
		}
		
		public IQueryable<SubjectionRelationEntity> SubjectionRelationEntities 
		{
			get
			{
				return this.GetAll<SubjectionRelationEntity>();
			}
		}
		
		public IQueryable<RuleSolutionEntity> RuleSolutionEntities 
		{
			get
			{
				return this.GetAll<RuleSolutionEntity>();
			}
		}
		
		public IQueryable<RepeatLimitSolutionDetailEntity> RepeatLimitSolutionDetailEntities 
		{
			get
			{
				return this.GetAll<RepeatLimitSolutionDetailEntity>();
			}
		}
		
		public IQueryable<ProtocolCommandEntity> ProtocolCommandEntities 
		{
			get
			{
				return this.GetAll<ProtocolCommandEntity>();
			}
		}
		
		public IQueryable<ProtocolColumnEntity> ProtocolColumnEntities 
		{
			get
			{
				return this.GetAll<ProtocolColumnEntity>();
			}
		}
		
		public IQueryable<PollutantCodeEntity> PollutantCodeEntities 
		{
			get
			{
				return this.GetAll<PollutantCodeEntity>();
			}
		}
		
		public IQueryable<OutlierSolutionDetailEntity> OutlierSolutionDetailEntities 
		{
			get
			{
				return this.GetAll<OutlierSolutionDetailEntity>();
			}
		}
		
		public IQueryable<OfflineSolutionDetailEntity> OfflineSolutionDetailEntities 
		{
			get
			{
				return this.GetAll<OfflineSolutionDetailEntity>();
			}
		}
		
		public IQueryable<NegativeLimitSolutionDetailEntity> NegativeLimitSolutionDetailEntities 
		{
			get
			{
				return this.GetAll<NegativeLimitSolutionDetailEntity>();
			}
		}
		
		public IQueryable<ExcessiveSolutionDetailEntity> ExcessiveSolutionDetailEntities 
		{
			get
			{
				return this.GetAll<ExcessiveSolutionDetailEntity>();
			}
		}
		
		public IQueryable<EQIConcentrationLimitEntity> EQIConcentrationLimitEntities 
		{
			get
			{
				return this.GetAll<EQIConcentrationLimitEntity>();
			}
		}
		
		public IQueryable<EQIEntity> EQIEntities 
		{
			get
			{
				return this.GetAll<EQIEntity>();
			}
		}
		
		public IQueryable<APIConcentrationLimitEntity> APIConcentrationLimitEntities 
		{
			get
			{
				return this.GetAll<APIConcentrationLimitEntity>();
			}
		}
		
		public IQueryable<MonitoringStationEntity> MonitoringStationEntities 
		{
			get
			{
				return this.GetAll<MonitoringStationEntity>();
			}
		}
		
		public IQueryable<MonitoringPointExtensionForEQMSWaterEntity> MonitoringPointExtensionForEQMSWaterEntities 
		{
			get
			{
				return this.GetAll<MonitoringPointExtensionForEQMSWaterEntity>();
			}
		}
		
		public IQueryable<MonitoringPointExtensionForEQMSAirEntity> MonitoringPointExtensionForEQMSAirEntities 
		{
			get
			{
				return this.GetAll<MonitoringPointExtensionForEQMSAirEntity>();
			}
		}
		
		public IQueryable<MonitoringPointEntity> MonitoringPointEntities 
		{
			get
			{
				return this.GetAll<MonitoringPointEntity>();
			}
		}
		
		public IQueryable<MonitoringInstrumentEntity> MonitoringInstrumentEntities 
		{
			get
			{
				return this.GetAll<MonitoringInstrumentEntity>();
			}
		}
		
		public IQueryable<CommunicationInfoEntity> CommunicationInfoEntities 
		{
			get
			{
				return this.GetAll<CommunicationInfoEntity>();
			}
		}
		
		public IQueryable<AcquisitionInstrumentEntity> AcquisitionInstrumentEntities 
		{
			get
			{
				return this.GetAll<AcquisitionInstrumentEntity>();
			}
		}
		
		public IQueryable<InstrumentEntity> InstrumentEntities 
		{
			get
			{
				return this.GetAll<InstrumentEntity>();
			}
		}
		
		public IQueryable<InstrumentChannelEntity> InstrumentChannelEntities 
		{
			get
			{
				return this.GetAll<InstrumentChannelEntity>();
			}
		}
		
		public IQueryable<UsedPollutantEntity> UsedPollutantEntities 
		{
			get
			{
				return this.GetAll<UsedPollutantEntity>();
			}
		}
		
		public IQueryable<PersonalizedSettingEntity> PersonalizedSettingEntities 
		{
			get
			{
				return this.GetAll<PersonalizedSettingEntity>();
			}
		}
		
		public IQueryable<CustomPollutantGroupEntity> CustomPollutantGroupEntities 
		{
			get
			{
				return this.GetAll<CustomPollutantGroupEntity>();
			}
		}
		
		public IQueryable<CustomPollutantCategoryEntity> CustomPollutantCategoryEntities 
		{
			get
			{
				return this.GetAll<CustomPollutantCategoryEntity>();
			}
		}
		
		public IQueryable<V_OfflineSettingEntity> V_OfflineSettingEntities 
		{
			get
			{
				return this.GetAll<V_OfflineSettingEntity>();
			}
		}
		
		public IQueryable<V_Point_Water_UserConfigEntity> V_Point_Water_UserConfigEntities 
		{
			get
			{
				return this.GetAll<V_Point_Water_UserConfigEntity>();
			}
		}
		
		public IQueryable<V_Point_Air_UserConfigEntity> V_Point_Air_UserConfigEntities 
		{
			get
			{
				return this.GetAll<V_Point_Air_UserConfigEntity>();
			}
		}
		
		public IQueryable<V_FactorEntity> V_FactorEntities 
		{
			get
			{
				return this.GetAll<V_FactorEntity>();
			}
		}
		
		public IQueryable<V_Factor_SiteMap_UserConfigEntity> V_Factor_SiteMap_UserConfigEntities 
		{
			get
			{
				return this.GetAll<V_Factor_SiteMap_UserConfigEntity>();
			}
		}
		
		public IQueryable<V_Factor_UserConfigEntity> V_Factor_UserConfigEntities 
		{
			get
			{
				return this.GetAll<V_Factor_UserConfigEntity>();
			}
		}
		
		public IQueryable<V_InstrumentEntity> V_InstrumentEntities 
		{
			get
			{
				return this.GetAll<V_InstrumentEntity>();
			}
		}
		
		public IQueryable<V_InstrumentChannelEntity> V_InstrumentChannelEntities 
		{
			get
			{
				return this.GetAll<V_InstrumentChannelEntity>();
			}
		}
		
		public IQueryable<V_Point_SiteMap_UserConfig_TypeEntity> V_Point_SiteMap_UserConfig_TypeEntities 
		{
			get
			{
				return this.GetAll<V_Point_SiteMap_UserConfig_TypeEntity>();
			}
		}
		
		public IQueryable<V_Point_SiteMap_UserConfig_RegionEntity> V_Point_SiteMap_UserConfig_RegionEntities 
		{
			get
			{
				return this.GetAll<V_Point_SiteMap_UserConfig_RegionEntity>();
			}
		}
		
		public IQueryable<V_Point_SiteMap_UserConfig_PropertyEntity> V_Point_SiteMap_UserConfig_PropertyEntities 
		{
			get
			{
				return this.GetAll<V_Point_SiteMap_UserConfig_PropertyEntity>();
			}
		}
		
		public IQueryable<V_Point_UserConfigEntity> V_Point_UserConfigEntities 
		{
			get
			{
				return this.GetAll<V_Point_UserConfigEntity>();
			}
		}
		
		public IQueryable<V_NotifyNumberEntity> V_NotifyNumberEntities 
		{
			get
			{
				return this.GetAll<V_NotifyNumberEntity>();
			}
		}
		
		public IQueryable<V_Point_AirEntity> V_Point_AirEntities 
		{
			get
			{
				return this.GetAll<V_Point_AirEntity>();
			}
		}
		
		public IQueryable<V_Point_InstrumentEntity> V_Point_InstrumentEntities 
		{
			get
			{
				return this.GetAll<V_Point_InstrumentEntity>();
			}
		}
		
		public IQueryable<V_Point_InstrumentChannelEntity> V_Point_InstrumentChannelEntities 
		{
			get
			{
				return this.GetAll<V_Point_InstrumentChannelEntity>();
			}
		}
		
		public IQueryable<DT_ApproveMappingEntity> DT_ApproveMappingEntities 
		{
			get
			{
				return this.GetAll<DT_ApproveMappingEntity>();
			}
		}
		
		public IQueryable<V_NotifyMaliEntity> V_NotifyMaliEntities 
		{
			get
			{
				return this.GetAll<V_NotifyMaliEntity>();
			}
		}
		
		public IQueryable<SYS_PointsFactors_MappingEntity> SYS_PointsFactors_MappingEntities 
		{
			get
			{
				return this.GetAll<SYS_PointsFactors_MappingEntity>();
			}
		}
		
		public IQueryable<SYS_Point_MappingEntity> SYS_Point_MappingEntities 
		{
			get
			{
				return this.GetAll<SYS_Point_MappingEntity>();
			}
		}
		
		public IQueryable<SYS_Factors_MappingEntity> SYS_Factors_MappingEntities 
		{
			get
			{
				return this.GetAll<SYS_Factors_MappingEntity>();
			}
		}
		
		public IQueryable<DT_ConfigIDInfoEntity> DT_ConfigIDInfoEntities 
		{
			get
			{
				return this.GetAll<DT_ConfigIDInfoEntity>();
			}
		}
		
		public IQueryable<DT_DataTypeConfigEntity> DT_DataTypeConfigEntities 
		{
			get
			{
				return this.GetAll<DT_DataTypeConfigEntity>();
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
		/// Allows you to customize the BackendConfiguration of BaseDataModel.
		/// </summary>
		/// <param name="config">The BackendConfiguration of BaseDataModel.</param>
		static partial void CustomizeBackendConfiguration(ref BackendConfiguration config);
		
	}
	
	public interface IBaseDataModelUnitOfWork : IUnitOfWork
	{
		IQueryable<ValidDataSettingEntity> ValidDataSettingEntities
		{
			get;
		}
		IQueryable<RepeatLimitSettingEntity> RepeatLimitSettingEntities
		{
			get;
		}
		IQueryable<OutlierSettingEntity> OutlierSettingEntities
		{
			get;
		}
		IQueryable<OfflineSettingEntity> OfflineSettingEntities
		{
			get;
		}
		IQueryable<NegativeLimitSettingEntity> NegativeLimitSettingEntities
		{
			get;
		}
		IQueryable<ExcessiveSettingEntity> ExcessiveSettingEntities
		{
			get;
		}
		IQueryable<BreakSettingEntity> BreakSettingEntities
		{
			get;
		}
		IQueryable<NotifyTypeEntity> NotifyTypeEntities
		{
			get;
		}
		IQueryable<NotifyStrategyEntity> NotifyStrategyEntities
		{
			get;
		}
		IQueryable<NotifySendEntity> NotifySendEntities
		{
			get;
		}
		IQueryable<NotifyNumberEntity> NotifyNumberEntities
		{
			get;
		}
		IQueryable<NotifyGradeEntity> NotifyGradeEntities
		{
			get;
		}
		IQueryable<NotifyAddressEntity> NotifyAddressEntities
		{
			get;
		}
		IQueryable<GradeEntity> GradeEntities
		{
			get;
		}
		IQueryable<CreatAlarmEntity> CreatAlarmEntities
		{
			get;
		}
		IQueryable<AlarmInfoEntity> AlarmInfoEntities
		{
			get;
		}
		IQueryable<ValidDataSolutionDetailEntity> ValidDataSolutionDetailEntities
		{
			get;
		}
		IQueryable<SubjectionRelationEntity> SubjectionRelationEntities
		{
			get;
		}
		IQueryable<RuleSolutionEntity> RuleSolutionEntities
		{
			get;
		}
		IQueryable<RepeatLimitSolutionDetailEntity> RepeatLimitSolutionDetailEntities
		{
			get;
		}
		IQueryable<ProtocolCommandEntity> ProtocolCommandEntities
		{
			get;
		}
		IQueryable<ProtocolColumnEntity> ProtocolColumnEntities
		{
			get;
		}
		IQueryable<PollutantCodeEntity> PollutantCodeEntities
		{
			get;
		}
		IQueryable<OutlierSolutionDetailEntity> OutlierSolutionDetailEntities
		{
			get;
		}
		IQueryable<OfflineSolutionDetailEntity> OfflineSolutionDetailEntities
		{
			get;
		}
		IQueryable<NegativeLimitSolutionDetailEntity> NegativeLimitSolutionDetailEntities
		{
			get;
		}
		IQueryable<ExcessiveSolutionDetailEntity> ExcessiveSolutionDetailEntities
		{
			get;
		}
		IQueryable<EQIConcentrationLimitEntity> EQIConcentrationLimitEntities
		{
			get;
		}
		IQueryable<EQIEntity> EQIEntities
		{
			get;
		}
		IQueryable<APIConcentrationLimitEntity> APIConcentrationLimitEntities
		{
			get;
		}
		IQueryable<MonitoringStationEntity> MonitoringStationEntities
		{
			get;
		}
		IQueryable<MonitoringPointExtensionForEQMSWaterEntity> MonitoringPointExtensionForEQMSWaterEntities
		{
			get;
		}
		IQueryable<MonitoringPointExtensionForEQMSAirEntity> MonitoringPointExtensionForEQMSAirEntities
		{
			get;
		}
		IQueryable<MonitoringPointEntity> MonitoringPointEntities
		{
			get;
		}
		IQueryable<MonitoringInstrumentEntity> MonitoringInstrumentEntities
		{
			get;
		}
		IQueryable<CommunicationInfoEntity> CommunicationInfoEntities
		{
			get;
		}
		IQueryable<AcquisitionInstrumentEntity> AcquisitionInstrumentEntities
		{
			get;
		}
		IQueryable<InstrumentEntity> InstrumentEntities
		{
			get;
		}
		IQueryable<InstrumentChannelEntity> InstrumentChannelEntities
		{
			get;
		}
		IQueryable<UsedPollutantEntity> UsedPollutantEntities
		{
			get;
		}
		IQueryable<PersonalizedSettingEntity> PersonalizedSettingEntities
		{
			get;
		}
		IQueryable<CustomPollutantGroupEntity> CustomPollutantGroupEntities
		{
			get;
		}
		IQueryable<CustomPollutantCategoryEntity> CustomPollutantCategoryEntities
		{
			get;
		}
		IQueryable<V_OfflineSettingEntity> V_OfflineSettingEntities
		{
			get;
		}
		IQueryable<V_Point_Water_UserConfigEntity> V_Point_Water_UserConfigEntities
		{
			get;
		}
		IQueryable<V_Point_Air_UserConfigEntity> V_Point_Air_UserConfigEntities
		{
			get;
		}
		IQueryable<V_FactorEntity> V_FactorEntities
		{
			get;
		}
		IQueryable<V_Factor_SiteMap_UserConfigEntity> V_Factor_SiteMap_UserConfigEntities
		{
			get;
		}
		IQueryable<V_Factor_UserConfigEntity> V_Factor_UserConfigEntities
		{
			get;
		}
		IQueryable<V_InstrumentEntity> V_InstrumentEntities
		{
			get;
		}
		IQueryable<V_InstrumentChannelEntity> V_InstrumentChannelEntities
		{
			get;
		}
		IQueryable<V_Point_SiteMap_UserConfig_TypeEntity> V_Point_SiteMap_UserConfig_TypeEntities
		{
			get;
		}
		IQueryable<V_Point_SiteMap_UserConfig_RegionEntity> V_Point_SiteMap_UserConfig_RegionEntities
		{
			get;
		}
		IQueryable<V_Point_SiteMap_UserConfig_PropertyEntity> V_Point_SiteMap_UserConfig_PropertyEntities
		{
			get;
		}
		IQueryable<V_Point_UserConfigEntity> V_Point_UserConfigEntities
		{
			get;
		}
		IQueryable<V_NotifyNumberEntity> V_NotifyNumberEntities
		{
			get;
		}
		IQueryable<V_Point_AirEntity> V_Point_AirEntities
		{
			get;
		}
		IQueryable<V_Point_InstrumentEntity> V_Point_InstrumentEntities
		{
			get;
		}
		IQueryable<V_Point_InstrumentChannelEntity> V_Point_InstrumentChannelEntities
		{
			get;
		}
		IQueryable<DT_ApproveMappingEntity> DT_ApproveMappingEntities
		{
			get;
		}
		IQueryable<V_NotifyMaliEntity> V_NotifyMaliEntities
		{
			get;
		}
		IQueryable<SYS_PointsFactors_MappingEntity> SYS_PointsFactors_MappingEntities
		{
			get;
		}
		IQueryable<SYS_Point_MappingEntity> SYS_Point_MappingEntities
		{
			get;
		}
		IQueryable<SYS_Factors_MappingEntity> SYS_Factors_MappingEntities
		{
			get;
		}
		IQueryable<DT_ConfigIDInfoEntity> DT_ConfigIDInfoEntities
		{
			get;
		}
		IQueryable<DT_DataTypeConfigEntity> DT_DataTypeConfigEntities
		{
			get;
		}
	}
}
#pragma warning restore 1591
