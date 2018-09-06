namespace SmartEP.Service.Core.Config
{
    using System;
    using System.Text;

    /// <summary>
    /// Discuz!NT 配置管理类接口
    /// </summary>
    public interface IConfigFileOperation<T>
    {
        #region Properties

        T ConfigInfo
        {
            get; set;
        }

        DateTime ModificationDate
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        T LoadConfig();

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <returns></returns>
        bool SaveConfig();

        #endregion Methods
    }
}