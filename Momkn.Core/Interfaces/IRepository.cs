using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Enitities;

namespace Momkn.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id, IList<string> includeProperties = null);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate, IList<string> includeProperties);
        T Add(T entity);
        void Add(IEnumerable<T> entity);
        void Update(T entity);
        void Update(IEnumerable<T> entity);

        void Delete(T entity);
        void BulkDelete(List<T> entity);
        void HardDelete(T entity);
        void BulkHardDelete(List<T> entity);

        bool Exists(int id);
        void BulkInsert(List<T> entity);

    }
}
