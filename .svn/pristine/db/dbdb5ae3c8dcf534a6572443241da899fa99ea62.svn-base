namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using SmartEP.Core.Generic;
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseGenericRepository<TContext, TEntity> : IBaseGenericRepository<TContext, TEntity>
        where TContext : ICustomOrmContext,new()
        where TEntity : IBaseEntityProperty
    {

        public BaseGenericRepository()
        {
            Context = Singleton<TContext>.GetInstance();
        }

        #region Properties

        public ICustomOrmContext Context;


        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Add(object obj)
        {
            this.Context.Add(obj);
            SaveChanges();
        }

        ///  <summary>
        /// 增加TEntity泛型对象
        /// </summary>
        ///  <param name="entity">List<TEntity>类型对象</param>
        public virtual void Add(TEntity entity)
        {
            this.Context.Add(entity);
            SaveChanges();
        }

        ///  <summary>
        /// 批量增加TEntity泛型对象
        /// </summary>
        ///  <param name="entities">List<TEntity>类型对象</param>
        public virtual void BatchAdd(List<TEntity> entities)
        {
            foreach (var t in entities)
            {
                this.Context.Add(t);
            }
            SaveChanges();
        }

        /// <summary>
        /// 批量删除对象
        ///  </summary>
        public virtual void BatchDelete(List<TEntity> entities)
        {
            foreach (var t in entities)
            {
                this.Context.Delete(t);
            }
            SaveChanges();
        }

        /// <summary>
        /// 批量更新对象
        ///  </summary>
        public virtual void BatchUpdate(List<TEntity> entities)
        {
            SaveChanges();
        }

        public virtual void ClearChanges()
        {
            this.Context.ClearChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Delete(object obj)
        {
            this.Context.Delete(obj);
            SaveChanges();
        }

        ///  <summary>
        /// 删除TEntity泛型对象
        /// </summary>
        ///  <param name="entity">List<TEntity>类型对象</param>
        public virtual void Delete(TEntity entity)
        {
            this.Context.Delete(entity);
            SaveChanges();
        }

        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 根据key主键判断记录是否存在
        ///  </summary>
        ///  <returns>记录数</returns>
        public abstract bool IsExist(string strKey);

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.GetAll<TEntity>().Where(predicate);
        }

        /// <summary>
        /// 返回所有对象
        ///  </summary>
        ///  <returns>List<TEntity>类型</returns>
        public virtual IQueryable<TEntity> RetrieveAll()
        {
            return this.Context.GetAll<TEntity>();
        }

        /// <summary>
        /// 返回记录总数
        ///  </summary>
        ///  <returns>记录数</returns>
        public virtual int RetrieveAllCount()
        {
            return this.Context.GetAll<TEntity>().Count();
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有符合条件的对象的记录总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int RetrieveCount(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.GetAll<TEntity>().Where(predicate).Count();
        }

        /// <summary>
        /// 根据lambda表达式要求，返回第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity RetrieveFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.GetAll<TEntity>().FirstOrDefault(predicate);
        }

        public virtual void SaveChanges()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {
            SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Update(object obj)
        {
            Update((TEntity)obj);
        }

        ///  <summary>
        /// 使用TEntity对象更新记录
        ///  </summary>
        ///  <param name="entity">待更新对象</param>
        ///  <returns></returns>
        public virtual bool Update(TEntity entity)
        {
            SaveChanges();
            return true;
        }

        #endregion Methods

        #region Other

        ///// <summary>
        ///// 保存TEntity泛型对象，并返回对象主键
        ///// </summary>
        ///// <typeparam name="TKey">
        ///// <param name="entity"></param>
        ///// <returns>对象主键</returns>
        //public abstract TKey AddAndRetKey<TKey>(TEntity entity);
        /////  <summary>
        ///// 保存TEntity泛型对象，并返回对象主键
        ///// </summary>
        /////  <param name="entity">List<TEntity>类型对象</param>
        ///// <returns>对象主键</returns>
        //public abstract TKey SaveAndRetKey<TKey>(TEntity entitiy);
        ///// <summary>
        ///// 根据key主键List批量删除对象
        /////  </summary>
        /////  <returns>是否成功</returns>
        //public abstract void BatchDelete<TKey>(List<TKey> keys);
        ///// <summary>
        ///// 根据key主键删除对象
        /////  </summary>
        /////  <returns>是否成功</returns>
        //public abstract bool Delete<TKey>(TKey key);
        /////  <summary>
        ///// 使用TEntity对象更新主键为key的记录
        /////  </summary>
        /////  <param name="key">主键</param>
        /////  <param name="entity">待更新对象</param>
        /////  <returns></returns>
        //public abstract bool Update<TKey>(TKey key, TEntity entitiy);
        /////  <summary>
        ///// 返回指定key的TEntity对象
        /////  </summary>
        /////  <param name="key">主键</param>
        /////  <returns></returns>
        //public abstract IQueryable<TEntity> Retrieve<TKey>(TKey key);
        ///// <summary>
        ///// 根据key主键判断记录是否存在
        /////  </summary>
        /////  <returns>记录数</returns>
        //public abstract bool IsExist<TKey>(TKey key);

        #endregion Other
    }
}