﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IServices.ISysServices;
using Models;
using Models.Infrastructure;
using Common;

namespace Services.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly IDbSet<T> _dbset;
        private readonly IUserInfo _userInfo;

        protected RepositoryBase(IDatabaseFactory databaseFactory, IUserInfo userInfo)
        {
            _dataContext = databaseFactory.Get();
            _userInfo = userInfo;
            _dbset = _dataContext.Set<T>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            var dbSetBase = entity as IDbSetBase;

            if (dbSetBase != null)
            {
                dbSetBase.CreatedBy = _userInfo.UserId;
                dbSetBase.CreatedDate = DateTime.Now.ToString();

                entity = dbSetBase as T;
            }

            //var ienterprise = entity as IEnterprise;

            //if (ienterprise != null)
            //{
            //    ienterprise.EnterpriseId = _userInfo.EnterpriseId;
            //    entity = ienterprise as T;
            //}
            _dbset.Add(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);

            var dbSetBase = entity as IDbSetBase;

            if (dbSetBase != null)
            {
                var databaseValues = _dataContext.Entry(dbSetBase).GetDatabaseValues();

                dbSetBase.CreatedBy = databaseValues.GetValue<string>("CreatedBy");
                dbSetBase.CreatedDate = databaseValues.GetValue<string>("CreatedDate");

                dbSetBase.UpdatedDate = DateTime.Now.ToString();
                dbSetBase.UpdatedBy = _userInfo.UserId;

                entity = dbSetBase as T;
            }

            //var ienterprise = entity as IEnterprise;

            //if (ienterprise != null)
            //{
            //    var databaseValues = _dataContext.Entry(ienterprise).GetDatabaseValues();

            //    var entId = databaseValues.GetValue<string>("EnterpriseId");

            //    if (!string.IsNullOrEmpty(entId))
            //    {
            //        if (entId == _userInfo.EnterpriseId)
            //        {
            //            ienterprise.EnterpriseId = entId;
            //        }
            //        else
            //        {
            //            throw new Exception();
            //        }

            //        entity = ienterprise as T;
            //    }
            //}

            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 添加或者更新
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <param name="entity">实体</param>
        public virtual void Save(object id, T entity)
        {
            if (id != null)
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remove">物理删除标记 默认false</param>
        public virtual void Delete(object id, bool remove = false)
        {
            var item = GetById(id);
            Delete(item, remove);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <param name="remove">物理删除标记 默认false</param>
        public virtual void Delete(T item, bool remove = false)
        {
            var dbSetBase = item as IDbSetBase;

            //var iEnterprise = item as IEnterprise;

            //if (iEnterprise?.EnterpriseId == _userInfo.EnterpriseId || iEnterprise == null)//有权操作
            //{
            //    if (!remove && dbSetBase != null)//标记删除
            //        dbSetBase.Deleted = true;
            //    else
            //        _dbset.Remove(item);
            //}

            if (!remove && dbSetBase != null)//标记删除
                dbSetBase.Deleted = true;
            else
                _dbset.Remove(item);
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="where"></param>
        /// <param name="remove">物理删除标记 默认false</param>
        public virtual void Delete(Expression<Func<T, bool>> where, bool remove = false)
        {
            foreach (var item in GetAll(where))
            {
                Delete(item, remove);
            }
        }

        /// <summary>
        /// 获取单个记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(object id)
        {
            var item = _dbset.Find(id);

            //var iEnterprise = item as IEnterprise;

            //if (iEnterprise != null && iEnterprise.EnterpriseId != _userInfo.EnterpriseId) return null;

            return item;
        }

        /// <summary>
        /// 获取符合条件的用户所在企业数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> where)
        {
            return GetAll().Where(where);
        }

        /// <summary>
        /// 获取用户所在企业数据
        /// </summary>
        /// <param name="containsDeleted">包含已删除数据</param>
        /// <param name="allEnt"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(bool containsDeleted = false, bool allEnt = false)
        {
            var model = _dbset as IQueryable<T>;

            //if (!allEnt && typeof(IEnterprise).IsAssignableFrom(typeof(T)))
            //{
            //    model = model.Where("EnterpriseId=\"" + _userInfo.EnterpriseId + "\"");
            //}

            if (typeof(IDbSetBase).IsAssignableFrom(typeof(T)))
            {
                if (!containsDeleted)
                {
                    model = model.Where("Deleted=false");
                }

                model = model.OrderBy("CreatedDate desc");
            }

            if (typeof(IUserDictionary).IsAssignableFrom(typeof(T)))
            {
                model = model.OrderBy("SystemId");
            }

            return model;
        }

        public int Commit()
        {
            return _dataContext.Commit();
        }

        public Task<int> CommitAsync()
        {
            return _dataContext.CommitAsync();
        }
        /// <summary>
        /// 获取指定SystemId实例的Id
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public virtual string GetIdBySystemId(string systemId)
        {
            var model = _dbset as IQueryable<T>;

            var param = Expression.Parameter(typeof(T), "c");

            if (typeof(IUserDictionary).IsAssignableFrom(typeof(T)))
            {
                Expression left = Expression.Property(param, "SystemId");
                Expression right = Expression.Constant(systemId);
                Expression filter = Expression.Equal(left, right);

                var end = Expression.Lambda<Func<T, bool>>(filter, new[] { param });


                var entity = model.FirstOrDefault(end) as IUserDictionary;
                if (entity != null)
                    return entity.Id;
            }

            return null;
        }
        /// <summary>
        /// 获取指定Id实例的SystemId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetSystemIdById(object id)
        {
            var entity = GetById(id) as IUserDictionary;
            if (entity != null)
                return entity.SystemId;
            return null;
        }
        /// <summary>
        /// 获取指定Id实例的Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetNameById(object id)
        {
            var systemId = GetSystemIdById(id);
            if(systemId.Length == 6)
            {
                var firstId = GetIdBySystemId(systemId.Substring(0, 3));
                var firstentity = GetById(firstId) as IUserDictionary;
                if(firstentity != null)
                {
                    var entity = GetById(id) as IUserDictionary;
                    if (entity != null)
                        return firstentity.Name + "-" + entity.Name;
                }
            }
            else if(systemId.Length == 3)
            {
                var entity = GetById(id) as IUserDictionary;
                if (entity != null)
                    return entity.Name;
            }
            return null;
        }

    }

}