namespace SmartEP.Service.Core.Cached
{
    using System.Collections;

    using SmartEP.Service.Core.Config;
    using SmartEP.Utilities.DataTypes.ExtensionMethods;
    using SmartEP.Utilities.Web.WebBrowserHelper;

    /// <summary>
    /// MemCache管理操作类
    /// </summary>
    public sealed class MemCachedManager
    {
        #region Fields

        private static MemcachedClient mc = null;
        private static MemCachedConfigInfo memCachedConfigInfo = GenericConfigFileManager<MemCachedConfigInfo>.GetConfigInstance();
        private static SockIOPool pool = null;
        private static string[] serverList = null;

        #endregion Fields

        #region Constructors

        static MemCachedManager()
        {
            CreateManager();
        }

        #endregion Constructors

        #region Enumerations

        /// <summary>
        /// Stats命令行参数
        /// </summary>
        public enum Stats
        {
            /// <summary>
            /// stats : 显示服务器信息, 统计数据等
            /// </summary>
            Default = 0,
            /// <summary>
            /// stats reset : 清空统计数据
            /// </summary>
            Reset = 1,
            /// <summary>
            /// stats malloc : 显示内存分配数据
            /// </summary>
            Malloc = 2,
            /// <summary>
            /// stats maps : 显示"/proc/self/maps"数据
            /// </summary>
            Maps =3,
            /// <summary>
            /// stats sizes
            /// </summary>
            Sizes = 4,
            /// <summary>
            /// stats slabs : 显示各个slab的信息,包括chunk的大小,数目,使用情况等
            /// </summary>
            Slabs = 5,
            /// <summary>
            /// stats items : 显示各个slab中item的数目和最老item的年龄(最后一次访问距离现在的秒数)
            /// </summary>
            Items = 6,
            /// <summary>
            /// stats cachedump slab_id limit_num : 显示某个slab中的前 limit_num 个 key 列表
            /// </summary>
            CachedDump =7,
            /// <summary>
            /// stats detail [on|off|dump] : 设置或者显示详细操作记录   on:打开详细操作记录  off:关闭详细操作记录 dump: 显示详细操作记录(每一个键值get,set,hit,del的次数)
            /// </summary>
            Detail = 8
        }

        #endregion Enumerations

        #region Properties

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static MemcachedClient CacheClient
        {
            get
            {
                if (mc == null)
                    CreateManager();

                return mc;
            }
        }

        /// <summary>
        /// 缓存服务器地址列表
        /// </summary>
        public static string[] ServerList
        {
            set
            {
                if (value != null)
                    serverList = value;
            }
            get { return serverList; }
        }

        #endregion Properties

        #region Methods

        public static void Dispose()
        {
            if (memCachedConfigInfo.ApplyMemCached && pool != null)
                pool.Shutdown();
        }

        /// <summary>
        /// 获取有效的服务器地址
        /// </summary>
        /// <returns>有效的服务器地</returns>
        public static string[] GetConnectedSocketHost()
        {
            SockIO sock = null;
            string connectedHost = null;
            foreach (string hostName in serverList)
            {
                if (!hostName.IsNullOrEmpty())
                {
                    try
                    {
                        sock = SockIOPool.GetInstance(memCachedConfigInfo.PoolName).GetConnection(hostName);
                        if (sock != null)
                        {
                            connectedHost = hostName.MergeString(connectedHost);
                        }
                    }
                    finally
                    {
                        if (sock != null)
                            sock.Close();
                    }
                }
            }
            return connectedHost.SplitString(",");
        }

        /// <summary>
        /// 获取当前缓存键值所存储在的服务器
        /// </summary>
        /// <param name="key">当前缓存键</param>
        /// <returns>当前缓存键值所存储在的服务器</returns>
        public static string GetSocketHost(string key)
        {
            string hostName = "";
            SockIO sock = null;
            try
            {
                sock = SockIOPool.GetInstance(memCachedConfigInfo.PoolName).GetSock(key);
                if (sock != null)
                {
                    hostName = sock.Host;
                }
            }
            finally
            {
                if (sock != null)
                    sock.Close();
            }
            return hostName;
        }

        /// <summary>
        /// 获取服务器端缓存的数据信息
        /// </summary>
        /// <returns>返回信息</returns>
        public static ArrayList GetStats()
        {
            ArrayList arrayList = new ArrayList();
            foreach (string server in serverList)
            {
                arrayList.Add(server);
            }
            return GetStats(arrayList, Stats.Default, null);
        }

        /// <summary>
        /// 获取服务器端缓存的数据信息
        /// </summary>
        /// <param name="serverArrayList">要访问的服务列表</param>
        /// <returns>返回信息</returns>
        public static ArrayList GetStats(ArrayList serverArrayList, Stats statsCommand, string param)
        {
            ArrayList statsArray = new ArrayList();
            param = param.IsNullOrEmpty() ? "" : param.Trim().ToLower();

            string commandstr = "stats";
            //转换stats命令参数
            switch (statsCommand)
            {
                case Stats.Reset: { commandstr = "stats reset"; break; }
                case Stats.Malloc: { commandstr = "stats malloc"; break; }
                case Stats.Maps: { commandstr = "stats maps"; break; }
                case Stats.Sizes: { commandstr = "stats sizes"; break; }
                case Stats.Slabs: { commandstr = "stats slabs"; break; }
                case Stats.Items: { commandstr = "stats"; break; }
                case Stats.CachedDump:
                {
                    string[] statsparams = StringExtensions.SplitString(param, " ");
                    if(statsparams.Length == 2)
                        if (ValueTypeExtensions.IsNumericArray(statsparams))
                            commandstr = "stats cachedump  " + param;

                    break;
                }
                case Stats.Detail:
                    {
                        if(string.Equals(param, "on") || string.Equals(param, "off") || string.Equals(param, "dump"))
                            commandstr = "stats detail " + param.Trim();

                        break;
                    }
                default: { commandstr = "stats"; break; }
            }
            //加载返回值
            Hashtable stats = MemCachedManager.CacheClient.Stats(serverArrayList, commandstr);
            foreach (string key in stats.Keys)
            {
                statsArray.Add(key);
                Hashtable values = (Hashtable)stats[key];
                foreach (string key2 in values.Keys)
                {
                    statsArray.Add(key2 + ":" + values[key2]);
                }
            }
            return statsArray;
        }

        private static void CreateManager()
        {
            serverList = memCachedConfigInfo.ServerList.SplitString("\r\n");

            pool = SockIOPool.GetInstance(memCachedConfigInfo.PoolName);
            pool.SetServers(serverList);
            pool.InitConnections = memCachedConfigInfo.IntConnections;//初始化链接数
            pool.MinConnections = memCachedConfigInfo.MinConnections;//最少链接数
            pool.MaxConnections = memCachedConfigInfo.MaxConnections;//最大连接数
            pool.SocketConnectTimeout = memCachedConfigInfo.SocketConnectTimeout;//Socket链接超时时间
            pool.SocketTimeout = memCachedConfigInfo.SocketTimeout;// Socket超时时间
            pool.MaintenanceSleep = memCachedConfigInfo.MaintenanceSleep;//维护线程休息时间
            pool.Failover = memCachedConfigInfo.FailOver; //失效转移(一种备份操作模式)
            pool.Nagle = memCachedConfigInfo.Nagle;//是否用nagle算法启动socket
            pool.HashingAlgorithm = HashingAlgorithm.NewCompatibleHash;
            pool.Initialize();

            mc = new MemcachedClient();
            mc.PoolName = memCachedConfigInfo.PoolName;
            mc.EnableCompression = false;
        }

        #endregion Methods
    }

    /// <summary>
    /// MemCache缓存策略类
    /// </summary>
    public class MemCachedStrategy : ICacheStrategy
    {
        #region Constructors

        static MemCachedStrategy()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 到期时间
        /// </summary>
        public int TimeOut
        {
            set; get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObject(string objId, object o)
        {
            AddObject(objId,o,TimeOut);
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="o">到期时间,单位:秒</param>
        public void AddObject(string objId, object o, int expire)
        {
            RemoveObject(objId);
            if (expire > 0)
                MemCachedManager.CacheClient.Set(objId, o, System.DateTime.Now.AddMinutes(expire));
            else
                MemCachedManager.CacheClient.Set(objId, o);
        }

        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        public void AddObjectWithDepend(string objId, object o, string[] dependKey)
        {
            ;
        }

        /// <summary>
        /// 添加指定ID的对象(关联指定文件组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="files"></param>
        public void AddObjectWithFileChange(string objId, object o, string[] files)
        {
            ;
        }

        /// <summary>
        /// 清空的有缓存数据
        /// </summary>
        public void FlushAll()
        {
            MemCachedManager.CacheClient.FlushAll();
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsConnect()
        {
            return MemCachedManager.GetConnectedSocketHost().Length > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在一个指定的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public virtual bool IsExistObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return false;
            }
            return MemCachedManager.CacheClient.Get(objId).IsNull() ? false : true;
        }

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        public void RemoveObject(string objId)
        {
            if (MemCachedManager.CacheClient.KeyExists(objId))
                MemCachedManager.CacheClient.Delete(objId);
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public object RetrieveObject(string objId)
        {
            return MemCachedManager.CacheClient.Get(objId);
        }

        #endregion Methods
    }
}