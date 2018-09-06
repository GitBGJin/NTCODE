namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IBaseEntityProperty
    {
        #region Properties

        DateTime? CreatDateTime
        {
            get; set;
        }

        string CreatUser
        {
            get; set;
        }

        string Description
        {
            get; set;
        }

        int? OrderByNum
        {
            get; set;
        }

        DateTime? UpdateDateTime
        {
            get; set;
        }

        string UpdateUser
        {
            get; set;
        }

        #endregion Properties
    }
}