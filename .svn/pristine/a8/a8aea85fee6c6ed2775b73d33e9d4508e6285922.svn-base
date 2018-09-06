namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    public interface IBaseGenericRepository<TContext, TEntity> : IBaseRepository
        where TContext : ICustomOrmContext
        where TEntity : IBaseEntityProperty
    {
        #region Methods

        ///  <summary>
        /// 增加TEntity泛型对象
        /// </summary>
        ///  <param name="entity">List<TEntity>类型对象</param>
        void Add(TEntity entity);

        ///  <summary>
        /// 批量增加TEntity泛型对象
        /// </summary>
        ///  <param name="entities">List<TEntity>类型对象</param>
        void BatchAdd(List<TEntity> entities);

        /// <summary>   
        /// 批量删除对象
        ///  </summary>
        void BatchDelete(List<TEntity> entities);

        ///  <summary>
        /// 使用List<TEntity>对象批量更新记录
        ///  </summary>
        ///  <param name="entity">待更新对象</param>
        ///  <returns></returns>
        void BatchUpdate(List<TEntity> entities);

        ///  <summary>
        /// 使用TEntity对象更新记录
        ///  </summary>
        ///  <param name="entity">待更新对象</param>
        ///  <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// 批量删除对象
        ///  </summary>
        void Delete(TEntity entity);

        /// <summary>
        /// 根据key主键判断记录是否存在
        ///  </summary>
        ///  <returns>记录数</returns>
        bool IsExist(string strKey);

        /// <summary>
        /// 获取指定的记录对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 返回所有对象
        ///  </summary>
        ///  <returns>List<TEntity>类型</returns>
        IQueryable<TEntity> RetrieveAll();

        /// <summary>
        /// 返回记录总数
        ///  </summary>
        ///  <returns>记录数</returns>
        int RetrieveAllCount();

        #endregion Methods

        #region Other

        /////  <summary>
        ///// 批量增加TEntity泛型对象
        ///// </summary>
        /////  <param name="entity">List<TEntity>类型对象</param>
        ///// <returns>增肌对象主键</returns>
        //TKey AddAndRetKey<TKey>(TEntity entity);
        ///// <summary>
        ///// 根据key主键List批量删除对象
        /////  </summary>
        /////  <returns>是否成功</returns>
        //void BatchDelete<TKey>(List<TKey> keys);
        ///// <summary>
        ///// 根据key主键删除对象
        /////  </summary>
        /////  <returns>是否成功</returns>
        //bool Delete<TKey>(TKey key);
        /////  <summary>
        ///// 使用TEntity对象更新主键为key的记录
        /////  </summary>
        /////  <param name="key">主键</param>
        /////  <param name="entity">待更新对象</param>
        /////  <returns></returns>
        //bool Update<TKey>(TKey key, TEntity entity);
        /////  <summary>
        ///// 返回指定key的TEntity对象
        /////  </summary>
        /////  <param name="key">主键</param>
        /////  <returns></returns>
        //IQueryable<TEntity> Retrieve<TKey>(TKey key);
        ///// <summary>
        ///// 根据key主键判断记录是否存在
        /////  </summary>
        /////  <returns>记录数</returns>
        //bool IsExist<TKey>(TKey key);

        #endregion Other
    }
}