using Data.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> list);
        void Update(T entity);
        void QuickUpdate(T original, T updated);
        void Delete(T entity);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> list);
        void Attach(T entity);
        void ChangeState(T entity, EntityState state);
        T FindById(int id, EfConfig configs);
        Task<T> FindByIdAsync(int id, EfConfig configs);
        T FindById(string id, EfConfig configs);
        Task<T> FindByIdAsync(string id, EfConfig configs);
        T GetSingle(Expression<Func<T, bool>> expression, EfConfig configs);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, EfConfig configs);
        IQueryable<T> Query(EfConfig configs);
        IQueryable<T> Query(Expression<Func<T, bool>> expression, EfConfig configs);
        IQueryable<T> QueryNoTracking(Expression<Func<T, bool>> expression, EfConfig configs);
        
        // Pure Sql Stuff
        void ExecQuery(string query);
        int ExecQuery(string query, params object[] parameters);
        Task<int> ExecQueryAsync(string query, params object[] parameters);
        IList<T> SqlQuery(string query);
        Task<IList<T>> SqlQueryAsync(string query);
        IList<dynamic> GetQueryResult(string query);
        Task<IList<dynamic>> GetQueryResultAsync(string query);
        DataTable DataTableQuery(string query);
        Task<DataTable> DataTableQueryAsync(string query);
    }
}
