namespace SmartEP.Service.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using SmartEP.Service.Core.Cached;
    using SmartEP.Service.Core.Enums;

    using SmartEP.Core.Interfaces;
    using SmartEP.DomainModel;
    using SmartEP.Utilities.DataTypes.ExtensionMethods;
    using SmartEP.Utilities.Reflection.ExtensionMethods;
    using SmartEP.Utilities.Web.WebBrowserHelper;

    public static class Factory
    {
        #region Methods

        /// <summary>
        /// 读取程序集中类的类型，通过反射方式获取相应的实例
        /// </summary>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            //实例T的程序集名称
            string assembleName = typeof(T).Assembly.GetName().Name;
            var dllVirtualPath = string.Concat("~/bin/", assembleName, ".dll");
            //实例T的类型
            string classType = typeof(T).FullName;

            return CreateInstance<T>(dllVirtualPath, classType);
        }

        /// <summary>
        /// 读取程序集中类的类型，通过反射方式获取相应的实例
        /// </summary>
        /// <param name="dllVirtualPath">反射程序集的虚拟路径</param>
        /// <param name="classType">实例类型</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string dllVirtualPath,string classType)
        {
            try
            {
                //采用工厂方式产生Context对象实例
                return AssemblyName.GetAssemblyName(new FileInfo(WebBrowserHelper.GetMapPath(dllVirtualPath)).FullName)
                                    .Load().GetType(classType)
                                    .CreateInstance<T>();
            }
            catch
            {
                throw new Exception("请检查创建类型是否正确！");
            }
        }

        /// <summary>
        /// 读取程序集中类的类型，通过反射方式获取相应的实例,并将实例存储在缓存中
        /// </summary>
        /// <returns></returns>
        public static T CreateInstanceAndStoreCache<T>(CachedMode mode = CachedMode.DefaultCachedStrategy)
        {
            //实例T的程序集名称
            string assembleName = typeof(T).Assembly.GetName().Name;
            var dllPath = string.Concat("~/bin/", assembleName, ".dll");
            //实例T的类型
            string classType = typeof(T).FullName;
            T tInstance;

            CachedContext cachedContext = CachedContext.GetCachedContextService();
            //读取缓存中是否存在对象实例
            if (!cachedContext.IsExistObject(classType,mode))
            {
                //缓存中不存在对象实例
                tInstance = CreateInstance<T>(dllPath, classType);
                cachedContext.AddObject(classType, tInstance,mode);
            }
            else
                tInstance = (T)cachedContext.RetrieveObject(classType,mode);

            return tInstance;
        }

        #endregion Methods
    }
}