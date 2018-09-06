namespace SmartEP.Service.Core.Cached
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    using SmartEP.Service.Core.Common;
    using SmartEP.Service.Core.Config;
    using SmartEP.Service.Core.Enums;
    using SmartEP.Utilities.DataTypes.ExtensionMethods;
    using SmartEP.Core.Generic;
    using SmartEP.Utilities.Reflection.ExtensionMethods;
    using SmartEP.Utilities.Web.WebBrowserHelper;

    /// <summary>
    /// 缓存类
    /// 进行全局缓存控制管理
    /// </summary>
    public class CachedContext
    {
        #region Fields

        //配置缓存方式，多中缓存方式以";"分割
        private static string strInitialCachedMode = GenericConfigFileManager<GlobalConfigInfo>.GetConfigInstance().CachedMode;
        private static string[] arrInitialCachedMode = strInitialCachedMode.SplitString(";");
        private static volatile CachedContext cachedContextInstance = null;
        private static ICacheStrategy cachedStrategy;

        //存放多个单例
        private static Dictionary<string, ICacheStrategy> dictionaryStrategy = new Dictionary<string, ICacheStrategy>();
        private static object lockHelper = new object();

        //缓存程序集名称
        private static string strCachedAssembleName = GenericConfigFileManager<GlobalConfigInfo>.GetConfigInstance().CachedAssemblyName;

        //缓存命名空间
        private static string strCachedNameSpace = GenericConfigFileManager<GlobalConfigInfo>.GetConfigInstance().CachedNameSpace;

        #endregion Fields

        #region Constructors

        //CachedMode cacheMode;
        /// <summary>
        /// 构造函数
        /// </summary>
        private CachedContext()
        {
            //初始化时，缓存字典中如有过期数据，先清理过期数据；确保缓存字典新鲜数据
            if(dictionaryStrategy.IsNotNull() && dictionaryStrategy.Count>0)
                dictionaryStrategy.Clear();

            //初始化默认DefaultCachedStrategy缓存
            dictionaryStrategy.Add("DefaultCachedStrategy", new DefaultCachedStrategy());

            //判断global.config对于的实例是否存在，并且实例中是否存在CacheMode信息
            if (GenericConfigFileManager<GlobalConfigInfo>.GetConfigInstance().IsNotNull() && strInitialCachedMode.IsNotNullAndNotEmpty())
            {
                try
                {
                    var dllPath = string.Concat("~/bin/", strCachedAssembleName, ".dll");
                    foreach (string strClassName in arrInitialCachedMode)
                    {
                        ////更加字符串获取对应枚举变量
                        //cacheMode = EnumMapping.ParseEnum<CachedMode>(strClassName);

                        var classType = string.Concat(strCachedNameSpace,"." ,strClassName);
                        //策略字典中不存在对象,使用类名作为Key检索使用
                        if (!dictionaryStrategy.ContainsKey(strClassName))
                        {
                            //采用反射方式产生对象实例
                            cachedStrategy = Factory.CreateInstance<ICacheStrategy>(dllPath, classType);
                            //判断缓存服务是否启动
                            if (cachedStrategy.IsConnect())
                            {
                                //缓存对象存入字典表
                                dictionaryStrategy.Add(strClassName, cachedStrategy);
                            }
                        }
                    }
                }
                catch
                {
                    throw new Exception("请检查global.config文件中节点CacheMode信息是否配置正确！");
                }
            }
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
        /// Singleton模式返回当前类的实例
        /// </summary>
        /// <returns></returns>
        public static CachedContext GetCachedContextService()
        {
            //return Singleton<CachedContext>.GetInstance();
            if (cachedContextInstance == null)
            {
                lock (lockHelper)
                {
                    if (cachedContextInstance == null)
                    {
                        cachedContextInstance = new CachedContext();
                    }
                }
            }
            return cachedContextInstance;
        }

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObject(string objId, object o,CachedMode mode=CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);

            //指定缓存中键不存在，保存相应键值对；否则不保存
            if (!IsExistObject(objId,mode))
                dictionaryStrategy[className].AddObject(objId, o);
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="o">到期时间,单位:秒</param>
        public void AddObject(string objId, object o, int expire, CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);

            //指定缓存中键不存在，保存相应键值对；否则不保存
            if (!IsExistObject(objId, mode))
                dictionaryStrategy[className].AddObject(objId, o,expire);
        }

        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        public void AddObjectWithDepend(string objId, object o, string[] dependKey, CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);

            //指定缓存中键不存在，保存相应键值对；否则不保存
            if (!IsExistObject(objId, mode))
                dictionaryStrategy[className].AddObjectWithDepend(objId, o, dependKey);
        }

        /// <summary>
        /// 添加指定ID的对象(关联指定文件组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="files"></param>
        public void AddObjectWithFileChange(string objId, object o, string[] files, CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);

            //指定缓存中键不存在，保存相应键值对；否则不保存
            if (!IsExistObject(objId, mode))
                dictionaryStrategy[className].AddObjectWithFileChange(objId, o, files);
        }

        /// <summary>
        /// 清空的有缓存数据
        /// </summary>
        public void FlushAll(CachedMode mode=CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);
            dictionaryStrategy[className].ThrowIfNull<ICacheStrategy>("缓存类型 " + className + " 不存在");

            dictionaryStrategy[className].FlushAll();
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsConnect(CachedMode mode=CachedMode.DefaultCachedStrategy)
        {
            bool bolConnect = false;
            var className = EnumMapping.GetCachedClassName(mode);

            //缓存字典中有缓存上下文
            if ( dictionaryStrategy.ContainsKey(className))
                bolConnect = dictionaryStrategy[className].IsConnect();
            return bolConnect;
        }

        /// <summary>
        /// 是否存在指定ID的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public virtual bool IsExistObject(string objId, CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);
            dictionaryStrategy[className].ThrowIfNull<ICacheStrategy>("缓存类型 " + className + " 不存在");

            if (objId.IsNullOrEmpty())
                return false;
            return dictionaryStrategy[className].RetrieveObject(objId).IsNotNull() ? true : false;
        }

        /// <summary>
        /// 是否字典中存在缓存策略
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public bool IsExitCachedStrategy(CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);
            return dictionaryStrategy[className].IsNotNull();
        }

        /// <summary>
        /// 加载指定的缓存策略
        /// </summary>
        /// <param name="ics"></param>
        public void LoadCustomCacheStrategy(CachedMode mode)
        {
            lock (lockHelper)
            {
                var className = EnumMapping.GetCachedClassName(mode);
                //加载指定的缓存策略不存在,使用默认缓存策略
                if (dictionaryStrategy[className].IsNull())
                {
                    cachedStrategy = dictionaryStrategy["DefaultCachedStrategy"];
                }
                else
                    cachedStrategy = dictionaryStrategy[className];
            }
        }

        /// <summary>
        /// 清除并加载默认的缓存策略
        /// </summary>
        public void LoadDefaultCacheStrategy()
        {
            lock (lockHelper)
            {
                if (dictionaryStrategy.IsNotNull())
                    dictionaryStrategy.Clear();
                dictionaryStrategy.Add("DefaultCachedStrategy", new DefaultCachedStrategy());
            }
        }

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        public void RemoveObject(string objId, CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);
            //指定缓存中存在键，则删除该键值对
            if (IsExistObject(objId, mode))
                dictionaryStrategy[className].RemoveObject(objId);
        }

        /// <summary>
        /// 重置缓存上下文
        /// </summary>
        public CachedContext ResetCacheStrategy()
        {
            lock (lockHelper)
            {
                cachedContextInstance = new CachedContext();
            }
            return cachedContextInstance;
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public object RetrieveObject(string objId, CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            var className = EnumMapping.GetCachedClassName(mode);
            dictionaryStrategy[className].ThrowIfNull<ICacheStrategy>("缓存类型 " + className + " 不存在");
            return dictionaryStrategy[className].RetrieveObject(objId);
        }

        #endregion Methods
    }
}