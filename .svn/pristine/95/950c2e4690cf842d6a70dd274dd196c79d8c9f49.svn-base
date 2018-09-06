using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Service.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.BaseInfoRepository.Dictionary;
using SmartEP.DomainModel;

namespace SmartEP.Service.Frame
{
    /// <summary>
    /// 名称：DictionaryService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 字典信息服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DictionaryService
    {
        //框架代码项
        CodeMainItemRepository g_Repository = new CodeMainItemRepository();
        //框架数据字典（支持多级扩展，我们主要使用到的点位区域类型，市-区/县级市-市区/乡镇）
        CodeDictionaryRepository g_CodeDicRepository = new CodeDictionaryRepository();
        /// <summary>
        /// 根据字典类型、字典名获取字典项列表
        /// </summary>
        /// <param name="dicOfAMS">自动监控字典名</param>
        /// <returns></returns>
        public IQueryable<V_CodeMainItemEntity> RetrieveList(DictionaryType dicType, string codeName)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            return g_Repository.Retrieve(V => V.TypeName == dicTypename.ToString() && V.CodeName == codeName).OrderByDescending(V => V.SortNumber);
        }

        /// <summary>
        /// 根据字典类型、字典名获取字典项列表
        /// </summary>
        /// <param name="dicOfAMS">自动监控字典名</param>
        /// <returns></returns>
        public IQueryable<V_CodeMainItemEntity> RetrieveListByCodeNames(DictionaryType dicType, string[] codeNames)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            return g_Repository.Retrieve(V => V.TypeName == dicTypename.ToString() && codeNames.Contains(V.CodeName)).OrderByDescending(V => V.SortNumber);
        }
        /// <summary>
        /// 根据字典类型、字典名获取字典项列表
        /// </summary>
        /// <param name="typeName">自动监控字典名</param>
        /// <param name="codeName">代码项名</param>
        /// <returns></returns>
        public IQueryable<V_CodeMainItemEntity> RetrieveList(string typeName, string codeName)
        {
            return g_Repository.Retrieve(V => V.TypeName == typeName && V.CodeName == codeName).OrderByDescending(V => V.SortNumber);
        }

        /// <summary>
        /// （多级扩展字典）根据字典类型、字典名获取字典项列表
        /// </summary>
        /// <param name="CodeDictionaryName">字典类型</param>
        /// <param name="codeName">字典名称</param>
        /// <returns></returns>
        public IQueryable<V_CodeDictionaryEntity> RetrieveCodeDictionaryList(string CodeDictionaryName, string codeName)
        {
            return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == CodeDictionaryName && V.CodeName == codeName).OrderByDescending(V => V.SortNumber);
        }

        /// <summary>
        /// 获取城市均值类型，例如苏州市区、吴江区、昆山市、太仓市、常熟市
        /// </summary>
        /// <returns></returns>
        public IQueryable<V_CodeDictionaryEntity> RetrieveCityList()
        {
            return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == "空气点位区域类型" && V.CodeName == "空气点位区域类型").OrderByDescending(V => V.SortNumber);
            //return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == "空气点位区域类型" && V.CodeName == "无锡大市").OrderByDescending(V => V.SortNumber);
        }

        /// <summary>
        /// 获取城市名称获取区域列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<V_CodeDictionaryEntity> RetrieveCityRegionList(string cityName)
        {
            return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == "空气点位区域类型" && V.CodeName == cityName).OrderByDescending(V => V.SortNumber);
        }
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<V_CodeDictionaryEntity> RetrieveCityRegion(CityType cityType)
        {
            //获取城市均值字典名称
            string mainGuid = SmartEP.Core.Enums.EnumMapping.GetDesc(cityType).Split(':')[1];
            return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == "空气点位区域类型" && (V.MainGuid == mainGuid || V.CodeName == "空气点位区域类型")).OrderByDescending(V => V.SortNumber);
        }

        /// <summary>
        /// 根据多级字典的ItemGuid获取字典名称
        /// </summary>
        /// <returns></returns>
        public string GetCodeDictionaryTextByValue(string itemGuid)
        {
            V_CodeDictionaryEntity entity = g_CodeDicRepository.RetrieveFirstOrDefault(V => V.ItemGuid == itemGuid);

            return entity == null ? "" : entity.ItemText;
        }

        /// <summary>
        /// 根据城市均值类型，获取区域
        /// </summary>
        /// <param name="cityType"></param>
        /// <returns></returns>
        public IQueryable<V_CodeDictionaryEntity> RetrieveRegionList(CityType cityType)
        {
            //获取城市均值字典名称
            string mainGuid = SmartEP.Core.Enums.EnumMapping.GetDesc(cityType).Split(':')[1];
            return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == "空气点位区域类型" && V.MainGuid == mainGuid).OrderByDescending(V => V.SortNumber);
        }

        /// <summary>
        /// 根据字典类型，字典名、字典项名称获取字典项的值
        /// </summary>
        /// <param name="dicType">字典类型，例如：DictionaryType.自动监控</param>
        /// <param name="codeName">字典名，例如：行政区划</param>
        /// <param name="itemText">字典项，例如：吴中区</param>
        /// <returns></returns>
        public string GetValueByText(DictionaryType dicType, string codeName, string itemText)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            V_CodeMainItemEntity entity = g_Repository.RetrieveFirstOrDefault(V => V.TypeName == dicTypename.ToString() && V.CodeName == codeName && V.ItemText == itemText);
            return entity != null ? entity.ItemGuid : string.Empty;
        }

        /// <summary>
        /// 根据字典类型，字典名、字典项编码获取字典项的值
        /// </summary>
        /// <param name="dicType">字典类型，例如：DictionaryType.自动监控</param>
        /// <param name="codeName">字典名，例如：数据类型</param>
        /// <param name="itemCode">字典项代码，例如：RealTime</param>
        /// <returns></returns>
        public string GetValueByCode(DictionaryType dicType, string codeName, string itemCode)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            V_CodeMainItemEntity entity = g_Repository.RetrieveFirstOrDefault(V => V.TypeName == dicTypename.ToString() && V.CodeName == codeName && V.ItemValue == itemCode);
            return entity != null ? entity.ItemGuid : string.Empty;
        }

        /// <summary>
        /// 根据字典类型，字典名、字典项的值获取字典项名称
        /// </summary>
        /// <param name="dicType">字典类型，例如：DictionaryType.自动监控</param>
        /// <param name="codeName">字典名，例如：行政区划</param>
        /// <param name="itemValue">字典值Guid</param>
        /// <returns></returns>
        public string GetTextByValue(DictionaryType dicType, string codeName, string itemValue)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            V_CodeMainItemEntity entity = g_Repository.RetrieveFirstOrDefault(V => V.TypeName == dicTypename.ToString() && V.CodeName == codeName && V.ItemValue == itemValue);
            return entity != null ? entity.ItemText : string.Empty;
        }
        /// <summary>
        /// 根据字典类型，字典名、字典项名称的值获取字典项
        /// </summary>
        /// <param name="dicType">字典类型，例如：DictionaryType.自动监控</param>
        /// <param name="codeName">字典名，例如：行政区划</param>
        /// <param name="itemText">字典值Guid</param>
        /// <returns></returns>
        public string GetValueByValue(DictionaryType dicType, string codeName, string itemText)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            V_CodeMainItemEntity entity = g_Repository.RetrieveFirstOrDefault(V => V.TypeName == dicTypename.ToString() && V.CodeName == codeName && V.ItemText == itemText);
            return entity != null ? entity.ItemValue : string.Empty;
        }
        /// <summary>
        /// 根据字典类型，字典名、字典项的Guid获取字典项名称
        /// </summary>
        /// <param name="dicType">字典类型，例如：DictionaryType.自动监控</param>
        /// <param name="codeName">字典名，例如：行政区划</param>
        /// <param name="itemGuid">字典值Guid</param>
        /// <returns></returns>
        public string GetTextByGuid(DictionaryType dicType, string codeName, string itemGuid)
        {
            string dicTypename = EnumMapping.GetDictionaryName(dicType);
            V_CodeMainItemEntity entity = g_Repository.RetrieveFirstOrDefault(V => V.TypeName == dicTypename.ToString() && V.CodeName == codeName && V.ItemGuid == itemGuid);
            return entity != null ? entity.ItemText : string.Empty;
        }

        /// <summary>
        /// （多级扩展字典）根据字典类型、字典名获取字典项列表
        /// </summary>
        /// <param name="CodeDictionaryName">字典类型</param>
        /// <returns></returns>
        public IQueryable<V_CodeDictionaryEntity> RetrieveCodeDictionaryList(string CodeDictionaryName)
        {
            return g_CodeDicRepository.Retrieve(V => V.CodeDictionaryName == CodeDictionaryName).OrderByDescending(V => V.SortNumber);
        }

    }
}
