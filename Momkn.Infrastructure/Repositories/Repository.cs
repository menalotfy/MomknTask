using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Interfaces;
using Momkn.Core.Interfaces.MainInterface;
using Momkn.Infrastructure.Data;
using Momkn.Core.Enitities;


namespace Momkn.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MomknDbContext _dbContext;

        public Repository(MomknDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById(int id, IList<string> includeProperties = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>().Where(t => t.ID == id && t.IsDeleted != true);
            if (includeProperties != null)
            {
                foreach (var item in includeProperties)
                {
                    queryable = queryable.Include(item);
                }
            }
           
           return queryable.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().Where(a => a.IsDeleted != true).AsEnumerable();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate, IList<string> includeProperties)
        {
            IQueryable<T> queryable=  _dbContext.Set<T>().Where(predicate);
            foreach(var item in includeProperties)
            {
                queryable = queryable.Include(item);
            }
            return queryable.AsEnumerable();
        }

        public T Add(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        public void Add(IEnumerable<T> entity)
        {
            entity.All(a => { a.CreatedDate = DateTime.Now; return true; });
           
            _dbContext.Set<T>().AddRange(entity);
            _dbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void Update(IEnumerable<T> entity)
        {
            foreach (var item in entity)
            {
                item.ModifiedDate = DateTime.Now;
                _dbContext.Entry(item).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.IsDeleted = true;
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void BulkDelete(List<T> entity)
        {
            entity.All(a => { a.ModifiedDate = DateTime.Now; a.IsDeleted = true; return true; });
            _dbContext.Set<T>().UpdateRange(entity);
            _dbContext.SaveChanges();
        }
        public void HardDelete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
        public void BulkHardDelete(List<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
            _dbContext.SaveChanges();
        }
        public void BulkInsert(List<T> entity)
        {
            entity.All(a => { a.CreatedDate = DateTime.Now; return true; });
            _dbContext.Set<T>().AddRange(entity);
            _dbContext.SaveChanges();
        }
        public bool Exists(int id)
        {
            return (GetById(id) != null);
        }
    }
}
