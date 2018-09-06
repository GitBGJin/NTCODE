using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.DomainModel.WaterQualityControlOperation
{
    /// <summary>
    /// 名称：ElectrodeCalibrationEntity.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-03
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 电极校准实体类
    /// 版权所有(C)：江苏远大信息股份有限公司
	[Serializable]
	public partial class ElectrodeCalibrationEntity
	{
        public ElectrodeCalibrationEntity()
		{}

		#region Model
		private int _id;
		private string _missionid;
		private string _actionid;
		private int? _pointid;
		private string _pointname;
		private string _monitoritemcode;
		private string _monitoritemtext;
		private decimal? _calibrationtemperature;
		private decimal? _calibrationconcentration;
		private decimal? _beforeconcentration;
		private decimal? _beforeparameter;
		private decimal? _afterconcentration;
		private decimal? _afterparameter;
		private string _pollingpeople;
		private DateTime? _pollingdate;
		private int? _orderbynum;
		private string _description;
		private string _creatuser;
		private DateTime? _creatdatetime;
		private string _updateuser;
        private DateTime? _updatedatetime;
        private string _taskCode;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MissionID
		{
			set{ _missionid=value;}
			get{return _missionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ActionID
		{
			set{ _actionid=value;}
			get{return _actionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PointId
		{
			set{ _pointid=value;}
			get{return _pointid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PointName
		{
			set{ _pointname=value;}
			get{return _pointname;}
		}
		/// <summary>
		/// 检查项编号
		/// </summary>
		public string MonitorItemCode
		{
			set{ _monitoritemcode=value;}
			get{return _monitoritemcode;}
		}
		/// <summary>
		/// 检查项名称
		/// </summary>
		public string MonitorItemText
		{
			set{ _monitoritemtext=value;}
			get{return _monitoritemtext;}
		}
		/// <summary>
		/// 校准液温度
		/// </summary>
		public decimal? CalibrationTemperature
		{
			set{ _calibrationtemperature=value;}
			get{return _calibrationtemperature;}
		}
		/// <summary>
		/// 校准液浓度
		/// </summary>
		public decimal? CalibrationConcentration
		{
			set{ _calibrationconcentration=value;}
			get{return _calibrationconcentration;}
		}
		/// <summary>
		/// 校准前浓度
		/// </summary>
		public decimal? BeforeConcentration
		{
			set{ _beforeconcentration=value;}
			get{return _beforeconcentration;}
		}
		/// <summary>
		/// 校准前参数
		/// </summary>
		public decimal? BeforeParameter
		{
			set{ _beforeparameter=value;}
			get{return _beforeparameter;}
		}
		/// <summary>
		/// 校准后浓度
		/// </summary>
		public decimal? AfterConcentration
		{
			set{ _afterconcentration=value;}
			get{return _afterconcentration;}
		}
		/// <summary>
		/// 校准后参数
		/// </summary>
		public decimal? AfterParameter
		{
			set{ _afterparameter=value;}
			get{return _afterparameter;}
		}
		/// <summary>
		/// 巡检人
		/// </summary>
		public string PollingPeople
		{
			set{ _pollingpeople=value;}
			get{return _pollingpeople;}
		}
		/// <summary>
		/// 巡检日期
		/// </summary>
		public DateTime? PollingDate
		{
			set{ _pollingdate=value;}
			get{return _pollingdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrderByNum
		{
			set{ _orderbynum=value;}
			get{return _orderbynum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CreatUser
		{
			set{ _creatuser=value;}
			get{return _creatuser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreatDateTime
		{
			set{ _creatdatetime=value;}
			get{return _creatdatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UpdateUser
		{
			set{ _updateuser=value;}
			get{return _updateuser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UpdateDateTime
		{
			set{ _updatedatetime=value;}
			get{return _updatedatetime;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string TaskCode
        {
            set { _taskCode = value; }
            get { return _taskCode; }
        }
		#endregion Model

	}
}

