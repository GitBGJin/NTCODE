namespace SmartEP.Service.Core.Config
{
    using System;

    using SmartEP.Utilities.DataTypes.ExtensionMethods;

    /////<summary>
    /////Hashing algorithms we can use
    /////</summary>
    //public enum HashingAlgorithm
    //{
    //    ///<summary>native String.hashCode() - fast (cached) but not compatible with other clients</summary>
    //    Native = 0,
    //    ///<summary>original compatibility hashing alg (works with other clients)</summary>
    //    OldCompatibleHash = 1,
    //    ///<summary>new CRC32 based compatibility hashing algorithm (fast and works with other clients)</summary>
    //    NewCompatibleHash = 2,
    //    ///<summary>Consistent Hashing algorithm</summary>
    //    KetamaHash = 3
    //}

    /// <summary>
    /// MemCached配置信息类文件
    /// </summary>
    [Serializable]
    public class MemCachedConfigInfo : IConfigInfo
    {
        #region Fields

        private bool _applyMemCached = true;
        private bool _failOver = true;
        private int _intConnections = 3;
        private int _maintenanceSleep = 30;
        private int _maxConnections = 5;
        private int _minConnections = 3;
        private bool _nagle = true;
        private string _poolName = "SmartEPMemCached";
        private string _serverList = "127.0.0.1:11211";
        private int _socketConnectTimeout = 1000    ;
        private int _socketTimeout = 5000;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 是否应用MemCached
        /// </summary>
        public bool ApplyMemCached
        {
            get
            {
                return _applyMemCached;
            }
            set
            {
                _applyMemCached = value;
            }
        }

        /// <summary>
        /// 失效转移，即服务器失效后，由其它服务器接管其工作,详情参见http://baike.baidu.com/view/1084309.htm
        /// </summary>
        public bool FailOver
        {
            get
            {
                return _failOver;
            }
            set
            {
                _failOver = value;
            }
        }

        /// <summary>
        /// 初始化链接数
        /// </summary>
        public int IntConnections
        {
            get
            {
                return _intConnections > 0 ? _intConnections : 3;
            }
            set
            {
                _intConnections = value;
            }
        }

        /// <summary>
        /// 维护线程休息时间
        /// </summary>
        public int MaintenanceSleep
        {
            get
            {
                return _maintenanceSleep > 0 ? _maintenanceSleep : 30;
            }
            set
            {
                _maintenanceSleep = value;
            }
        }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnections
        {
            get
            {
                return _maxConnections > 0 ?_maxConnections : 5;
            }
            set
            {
                _maxConnections = value;
            }
        }

        /// <summary>
        /// 最少链接数
        /// </summary>
        public int MinConnections
        {
            get
            {
                return _minConnections > 0 ? _minConnections : 3;
            }
            set
            {
                _minConnections = value;
            }
        }

        /// <summary>
        /// 是否用nagle算法启动socket
        /// </summary>
        public bool Nagle
        {
            get
            {
                return _nagle;
            }
            set
            {
                _nagle = value;
            }
        }

        /// <summary>
        /// 链接池名称
        /// </summary>
        public string PoolName
        {
            get
            {
                return _poolName.IsNullOrEmpty() ? "SmartEPMemCached" : _poolName;
            }
            set
            {
                _poolName = value;
            }
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string ServerList
        {
            get
            {
                return _serverList;
            }
            set
            {
                _serverList = value;
            }
        }

        /// <summary>
        /// Socket链接超时时间
        /// </summary>
        public int SocketConnectTimeout
        {
            get
            {
                return _socketConnectTimeout > 1000 ? _socketConnectTimeout : 1000;
            }
            set
            {
                _socketConnectTimeout = value;
            }
        }

        /// <summary>
        /// socket超时时间
        /// </summary>
        public int SocketTimeout
        {
            get
            {
                return _socketTimeout > 1000 ? _maintenanceSleep : 3000;
            }
            set
            {
                _socketTimeout = value;
            }
        }

        #endregion Properties
    }
}