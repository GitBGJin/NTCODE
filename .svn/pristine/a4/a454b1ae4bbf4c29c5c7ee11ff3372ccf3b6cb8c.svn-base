#region Header

/*
        Copyright (C) EQMS Group
        All rights reserved
        guid1:  f4fa154d-74d9-4102-8625-507b92823f1d
        guid2:  6c797071-cd2a-4297-9aa7-6701edcf2bfa
        guid3:  269fc6de-fc34-49b5-88db-ce88a11adb5e
        guid4:  238f4343-80b5-44a3-818d-4548ecff7934
        guid5:  1e744308-9d75-40aa-ae42-bbf486c256f7
        CLR版本:            	4.0.30319.225
        新建项输入的名称: 	    GeneralConfigFileManager
        机器名称:            	BD32
        注册组织名:         	Microsoft
        命名空间名称:      	    Discuz.Config
        文件名:             	GeneralConfigFileManager
        当前系统时间:      	    2012-08-23 11:11:39
        用户所在的域:      	    BD32
        当前登录用户名:   	    Administrator
        创建年份:           	2012

        created by 邵晓军 at  2012-08-23 11:11:39
*/

#endregion Header

namespace SmartEP.Service.Core.Config
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    using SmartEP.Utilities.DataTypes.ExtensionMethods;
    using SmartEP.Utilities.FileFormats.XML;
    using SmartEP.Utilities.IO.ExtensionMethods;
    using SmartEP.Utilities.Web.WebBrowserHelper;

    class GenericConfigFileOperation<T> : IConfigFileOperation<T>
        where T : IConfigInfo
    {
        #region Fields

        /// <summary>
        /// 文件所在路径变量(读取配置方式，无需构造)
        /// </summary>
        //private string configFilePath;
        /// <summary>
        /// 临时配置对象变量
        /// </summary>
        private static T configInfo;

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime modificationDate;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 初始化文件修改时间和T对象实例
        /// </summary>
        /// <param name="filepath"></param>
        public GenericConfigFileOperation()
        {
            //读取文件修改时间
            modificationDate = new FileInfo(ConfigFilePath).LastWriteTime;
            //实例化T对象
            configInfo = LoadConfig();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 根据泛型T类型实例类名，从系统全局配置文件[Configs/customConfigFile.config]中取得对于配置文件节点路径
        /// </summary>
        public static string ConfigFilePath
        {
            get
            {
                //后期需要修改
                //不需要读取配置信息，系统初始化时全局配置信息已缓存，可以读取缓存内容
                var customConfigFilePath = ConfigurationManager.AppSettings["CustomConfigFilePath"];

                //是否存在自定义的全局配置根文件
                if (!File.Exists(WebBrowserHelper.GetMapPath(customConfigFilePath)))
                    throw new ArgumentNullException("发生错误: 1、节点配置错误；2、虚拟目录或网站根目录下没有正确的自定义的全局配置根文件 [Configs/customConfigFile.config]");
                //实例T的类型
                var constanceClassName = typeof(T).FullName;
                //谓词定位节点，并取得对应Value
                var configNode = "/CustomConfigFile/add[@type='" + constanceClassName + "']/@filePath";
                //T实例对于使用的配置文件（读取太频繁，可以使用缓存）
                var configFilePath = WebBrowserHelper.GetMapPath(XMLProcess.Read(customConfigFilePath, configNode));
                //是否存在对于的配置文件
                if (configFilePath.IsNullOrEmpty() || !File.Exists(configFilePath))
                    throw new ArgumentNullException("发生错误: 1、节点配置错误；2、虚拟目录或网站根目录下没有正确的config文件");

                return configFilePath;
            }
        }

        public T ConfigInfo
        {
            get
            {
                return configInfo;
            }
            set
            {
                configInfo = value;
            }
        }

        public DateTime ModificationDate
        {
            get
            {
                return modificationDate;
            }
            set
            {
                modificationDate = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 根据文件日期判断文件是否修改
        /// </summary>
        /// <returns></returns>
        public static bool IsModifyConfig()
        {
            return DateTime.Compare(new FileInfo(ConfigFilePath).LastWriteTime, modificationDate) == 0 ? false : true;
        }

        /// <summary>
        /// 读取XML配置实例化T对象
        /// </summary>
        /// <param name="filePath">指定的配置文件所在的路径(包括文件名)</param>
        /// <returns></returns>
        public T LoadConfig()
        {
            lock (lockHelper)
            {
                return new FileInfo(ConfigFilePath).XMLToObject<T>();
            }
        }

        /// <summary>
        /// 保存(序列化)指定路径下的配置文件
        /// </summary>
        /// <param name="configFilePath">指定的配置文件所在的路径(包括文件名)</param>
        /// <param name="configinfo">被保存(序列化)的对象</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfig()
        {
            return SerializationExtensions.ToXML(configInfo, ConfigFilePath);
        }

        #endregion Methods
    }
}