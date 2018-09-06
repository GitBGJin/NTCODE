namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    interface IFilterContainer<T>
        where T : IFilterCriteria
    {
        #region Events

        event EventHandler<EventArgs> FilterChanged;

        #endregion Events

        #region Properties

        T Criteria
        {
            get;
        }

        #endregion Properties
    }
}