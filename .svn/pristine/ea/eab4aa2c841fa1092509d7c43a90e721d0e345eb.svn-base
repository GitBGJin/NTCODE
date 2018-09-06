namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    public interface ICustomOrmContext
    {
        #region Methods

        void Add(object entity);

        void ClearChanges();

        void Delete(object entity);

        IQueryable<T> GetAll<T>();

        //T GetObjectByKey<T>(ObjectKey key);
        //T GetObjectByKey<T>(Object key);
        void SaveChanges();

        #endregion Methods
    }
}