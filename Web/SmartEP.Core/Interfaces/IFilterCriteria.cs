namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 过滤条件实体的高层接口标记，主要用于实现IFilterCriteria接口派生类间的数据转换
    /// </summary>
    /*
     public class AvailableCarsFilter : IFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
     */
    interface IFilterCriteria
    {
    }
}