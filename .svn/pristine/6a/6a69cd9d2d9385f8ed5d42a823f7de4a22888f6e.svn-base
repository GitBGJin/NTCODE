namespace SmartEP.Service.Core.Config
{
    using System;
    using System.IO;
    using System.Text;
    using System.Timers;
    using System.Web;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// 论坛基本设置管理类
    /// </summary>
    public class GenericConfigFileManager<T>
        where T : IConfigInfo
    {
        #region Fields

        /// <summary>
        /// 文件所在路径变量
        /// </summary>
        //private static string configFilePath;
        /// 配置对象
        private static T configInfo;

        /// 时间定时器
        private static Timer refreshConfigTimer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 配置文件管理类构造函数
        /// </summary>
        static GenericConfigFileManager()
        {
            ////参数初始化
            //isRefresh = refreshOrNot;
            //timeInterval = interval;
            //refreshConfigTimer = new System.Timers.Timer(3000000);
            //refreshConfigTimer.AutoReset = true;
            //refreshConfigTimer.Enabled = true;
            //refreshConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Interval);
            //refreshConfigTimer.Start();
            //实例化T类型
            configInfo = GetConfigInstance();
        }

        #endregion Constructors

        #region Properties

        public static Timer RefreshConfigTimer
        {
            get
            {
                return refreshConfigTimer;
            }
            set
            {
                refreshConfigTimer = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <returns></returns>
        public static T GetConfigInstance()
        {
            if (configInfo == null)
                Reset();
            return configInfo;
        }

        /// <summary>
        /// 重新读取配置信息
        /// </summary>
        /// <returns></returns>
        public static T Reset()
        {
            configInfo = new GenericConfigFileOperation<T>().ConfigInfo;
            return configInfo;
        }

        /// <summary>
        /// 保存(序列化)指定路径下的配置文件
        /// </summary>
        /// <returns></returns>
        public static bool Save(T ConfigInfo)
        {
            return new GenericConfigFileOperation<T>().SaveConfig();
        }

        /// 定时检查配置文件是否修改
        public static void Timer_Interval(object sender, System.Timers.ElapsedEventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// 读取最新配置信息
        /// </summary>
        /// <returns></returns>
        public static T Update()
        {
            return Reset();
        }

        #endregion Methods

        #region Other

        ///// 时间间隔
        //private static int timeInterval;
        ///// 是否启用刷新
        //private static bool isRefresh;

        #endregion Other
    }
}