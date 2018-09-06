namespace SmartEP.Core.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    #region Enumerations

    [Flags]
    public enum CachedMode
    {
        DefaultCachedStrategy = 0x0,
        MemCachedStrategy = 0x1,
        RedisStrategy = 0x2,
        CookieStrategy = 0x4,
        SessionStrategy = 0x8,
        DBStrategy = 0x10,
        XMLStrategy = 0x20
    }

    /// <summary>
    /// 用于数据库ORM提供者
    /// </summary>
    [Flags]
    public enum OrmProviderMode
    {
        OpenAccessORM = 0x0,
        NHibernate = 0x1,
        ADONet = 0x2,
        EnterpriseLibrary = 0x4
    }

    /// <summary>
    /// 用于数据库ORM提供者
    /// </summary>
    [Flags]
    public enum SysLogMode
    {
        /// <summary>
        /// 正常日志
        /// </summary>
        [Description("Info")]
        Info = 0x0,
        /// <summary>
        /// 警告日志
        /// </summary>
        [Description("Warning")]
        Warning = 0x1,
        /// <summary>
        /// 错误日志
        /// </summary>
        [Description("Error")]
        Error = 0x2
    }

    #endregion Enumerations
}