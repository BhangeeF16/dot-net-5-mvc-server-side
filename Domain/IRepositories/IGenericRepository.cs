#region Imports

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Domain.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<T> GetByString(string id);
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);

        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);
    }
}