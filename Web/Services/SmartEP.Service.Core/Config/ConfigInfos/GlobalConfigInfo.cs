namespace SmartEP.Service.Core.Config
{
    using System;
    using System.Configuration;

    /// <summary>
    /// 系统全局设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public partial class GlobalConfigInfo : IConfigInfo
    {
        #region Fields

        //是否允许匿名用户: 1 是 0 否
        private int allowAnonymous = 0;

        //应用程序默认访问文件
        private string applicationLoginURL = "login.aspx";

        //应用程序名称
        private string applicationName = "环境质量";

        //应用程序默认门户地址
        private string applicationPortalURL = "portal.aspx";

        //缓存程序集名称
        private string cachedAssemblyName = "SmartEP.Service.Core";

        //使用缓存策略, DefaultCacheStrategy、MemCachedStrategy、RedisStrategy、CookieStrategy、SessionStrategy等
        private string cachedMode = "MemCachedStrategy";

        //缓存命名空间
        private string cachedNameSpace = "SmartEP.Service.Core.Cached";

        //自定义配置文件路径[Configs/customConfigFile.config]
        private string customConfigFile = ConfigurationManager.AppSettings["CustomConfigFilePath"];

        //用户单位名称
        private string customInstitutionName = "";

        //默认主题样式
        private string defaultTheme = "";

        //全局配置文件路径
        private string globalConfig = "Configs/global.config";

        //网站备案信息
        private string icp = "";

        //IP访问列表
        private string ipAccess = "";

        //IP禁止访问列表
        private string ipDenyAccess = "";

        //Jquery库地址
        private string jqueryURL = "javascript/jquery.js";

        //是否显示商业授权链接
        private int licensed = 1;

        //Logo图片地址
        private string logoPicPath = "";

        //在线列表是否隐藏游客: 1 是 0 否
        private int onlineListHiddenAnonymous = 1;

        //多久无动作视为离线（分钟）
        private int onlineTimeout = 30;

        //ORM提供者包括 1、OpenAccesssRepositoryStrategy 2、NHibernateRepositoryStrategy 3、AdoRepositoryStrategy 4、EnterpriseLibraryRepositoryStrategy
        private string ormProvider = "OpenAccesssRepositoryStrategy;AdoRepositoryStrategy";

        //ORM提供者程序集名称
        private string ormProviderAssemblyName = "SmartEP.Service.Core";

        //ORM提供者命名空间
        private string ormProviderNameSpace = "SmartEP.Service.Core.ORMProvider";

        //密码模式, 0为默认(32位md5), 1为动网兼容模式(16位md5)
        private int passwordMode = 0;

        //暂停应用程序
        private int pauseApplication = 0;

        //是否记录操作日志: 1 是 0 否
        private int recordOperationLog = 1;

        //是否记录访问日志: 1 是 0 否
        private int recordVisitLog = 1;

        //是否启用用户登录安全提问
        private int secques = 0;

        //发送欢迎短消息
        private int showWelcomeMsg = 1;

        //系统标题图片地址
        private string titlePicPath = "";

        //虚拟目录
        private string virtualPath = "";

        //url访问地址
        private string webURL = "";

        //欢迎短消息内容
        private string welcomeMsgContent = "欢迎登录系统!";

        #endregion Fields

        #region Properties

        //是否允许匿名用户: 1 是 0 否
        public int AllowAnonymous
        {
            get; set;
        }

        //应用程序默认访问文件
        public string ApplicationLoginURL
        {
            get; set;
        }

        //应用程序名称
        public string ApplicationName
        {
            get; set;
        }

        //应用程序默认门户地址
        public string ApplicationPortalURL
        {
            get; set;
        }

        //缓存程序集名称
        public string CachedAssemblyName
        {
            get; set;
        }

        //使用缓存策略, default为微软自身Cache、Memcache、Redis、Cookie、Session等
        public string CachedMode
        {
            get; set;
        }

        //缓存命名空间
        public string CachedNameSpace
        {
            get; set;
        }

        //自定义配置文件路径[Configs/customConfigFile.config]
        public string CustomConfigFile
        {
            get; set;
        }

        //用户单位名称
        public string CustomInstitutionName
        {
            get; set;
        }

        //默认主题样式
        public string DefaultTheme
        {
            get; set;
        }

        //全局配置文件路径
        public string GlobalConfig
        {
            get; set;
        }

        //网站备案信息
        public string Icp
        {
            get; set;
        }

        //IP访问列表
        public string IpAccess
        {
            get; set;
        }

        //IP禁止访问列表
        public string IpDenyAccess
        {
            get; set;
        }

        //Jquery库地址
        public string JqueryURL
        {
            get; set;
        }

        //是否显示商业授权链接
        public int Licensed
        {
            get; set;
        }

        //Logo图片地址
        public string LogoPicPath
        {
            get; set;
        }

        //在线列表是否隐藏游客: 1 是 0 否
        public int OnlineListHiddenAnonymous
        {
            get; set;
        }

        //多久无动作视为离线（分钟）
        public int OnlineTimeout
        {
            get; set;
        }

        //ORM提供者包括 OpenAccesssRepositoryStrategy、NHibernatesRepositoryStrategy、AdoRepositoryStrategy、EnterpriseLibrarysRepositoryStrategy
        public string OrmProvider
        {
            get; set;
        }

        //ORM提供者程序集名称
        public string OrmProviderAssemblyName
        {
            get; set;
        }

        //ORM提供者命名空间
        public string OrmProviderNameSpace
        {
            get; set;
        }

        //密码模式, 0为默认(32位md5), 1为动网兼容模式(16位md5)
        public int PasswordMode
        {
            get; set;
        }

        //暂停应用程序
        public int PauseApplication
        {
            get; set;
        }

        //是否记录操作日志: 1 是 0 否
        public int RecordOperationLog
        {
            get; set;
        }

        //是否记录访问日志: 1 是 0 否
        public int RecordVisitLog
        {
            get; set;
        }

        //是否启用用户登录安全提问
        public int Secques
        {
            get; set;
        }

        //发送欢迎短消息
        public int ShowWelcomeMsg
        {
            get; set;
        }

        //系统标题图片地址
        public string TitlePicPath
        {
            get; set;
        }

        //虚拟目录
        public string VirtualPath
        {
            get; set;
        }

        //url访问地址
        public string WebURL
        {
            get; set;
        }

        //欢迎短消息内容
        public string WelcomeMsgContent
        {
            get; set;
        }

        #endregion Properties
    }
}