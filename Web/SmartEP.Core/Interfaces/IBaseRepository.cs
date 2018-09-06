namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    public interface IBaseRepository : IDisposable
    {
        #region Methods

        void Add(object entity);

        void ClearChanges();

        void Delete(object entity);

        void SaveChanges();

        void Update(object entity);

        #endregion Methods
    }
}