using Data.Dtos;
using Data.Model;
using Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        protected PhotographyContext DbContext;

        public Repository(IUnitOfWork uow)
        {
            if (_context == null)
            {
                DbContext = uow.DbContext;
                _context = DbContext;
            }

            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> list)
        {
            _dbSet.AddRange(list);
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void ChangeState(T entity, EntityState state)
        {
            _context.Entry(entity).State = state;
        }

        public DataTable DataTableQuery(string query)
        {
            using (var conn = new SqlConnection(DbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (var da = new SqlDataAdapter())
                    {
                        using (var dt = new DataTable())
                        {
                            da.SelectCommand = cmd;
                            da.Fill(dt);
                            da.Dispose();

                            return dt;
                        }
                    }
                }
            }
        }

        public async Task<DataTable> DataTableQueryAsync(string query)
        {
            var dt = new DataTable();
            var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            var reader = await connection.CreateCommand().ExecuteReaderAsync();

            dt.Load(reader);

            return dt;
        }

        public void Delete(T entity)
        {
            var dbEntityEntry = _context.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = FindById(id, new EfConfig());

            if (entity == null)
            {
                return;
            }
            else
            {
                if (entity.GetType().GetProperty("IsDeleted") != null)
                {
                    T _entity = entity;
                    _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);
                    Update(_entity);
                }
                else
                {
                    Delete(entity);
                }
            }
        }

        public void DeleteRange(IEnumerable<T> list)
        {
            _dbSet.RemoveRange(list);
        }

        public void ExecQuery(string query)
        {
            DbContext.Database.ExecuteSqlCommand(query);
        }

        public int ExecQuery(string query, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand("EXEC " + query, parameters);
        }

        public async Task<int> ExecQueryAsync(string query, params object[] parameters)
        {
            return await DbContext.Database.ExecuteSqlCommandAsync("EXEC " + query, parameters);
        }

        public T FindById(int id, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            return _dbSet.Find(id);
        }

        public T FindById(string id, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            return _dbSet.Find(id);
        }

        public async Task<T> FindByIdAsync(int id, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FindByIdAsync(string id, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            return await _dbSet.FindAsync(id);
        }

        public IList<dynamic> GetQueryResult(string query)
        {
            return _dbSet.FromSql<dynamic>(query).ToList();
        }

        public async Task<IList<dynamic>> GetQueryResultAsync(string query)
        {
            return await _dbSet.FromSql<dynamic>(query).ToListAsync();
        }

        public T GetSingle(Expression<Func<T, bool>> expression, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            if (hasFlag("<IsDeleted>"))
            {
                var parameters = expression.Parameters;
                var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(parameters[0], "IsDeleted"), Expression.Constant(false));
                var originalBody = expression.Body;
                var fullExpr = Expression.And(originalBody, checkNotDeleted);
                var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, parameters);

                return _dbSet.Where(lambda).SingleOrDefault();
            }
            else
            {
                return _dbSet.Where(expression).SingleOrDefault();
            }
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            if (hasFlag("<IsDeleted>"))
            {
                var parameters = expression.Parameters;
                var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(parameters[0], "IsDeleted"), Expression.Constant(false));
                var originalBody = expression.Body;
                var fullExpr = Expression.And(originalBody, checkNotDeleted);
                var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, parameters);

                return await _dbSet.Where(lambda).SingleOrDefaultAsync();
            }
            else
            {
                return await _dbSet.Where(expression).SingleOrDefaultAsync();
            }
        }

        public IQueryable<T> Query(EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            if (hasFlag("<IsDeleted>"))
            {
                var argument = Expression.Parameter(typeof(T));
                var left = Expression.Property(argument, "IsDeleted");
                var right = Expression.Constant(false);
                var predicate = Expression.Lambda<Func<T, bool>>(Expression.Equal(left, right), new[] { argument });

                return _dbSet.Where(predicate);
            }
            else
            {
                return _dbSet;
            }
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            if (hasFlag("<IsDeleted>"))
            {
                var parameters = expression.Parameters;
                var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(parameters[0], "IsDeleted"), Expression.Constant(false));
                var originalBody = expression.Body;
                var fullExpr = Expression.And(originalBody, checkNotDeleted);
                var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, parameters);

                return _dbSet.Where(lambda);
            }
            else
            {
                return _dbSet.Where(expression);
            }
        }

        public IQueryable<T> QueryNoTracking(Expression<Func<T, bool>> expression, EfConfig config)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            _context.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;

            if (hasFlag("<IsDeleted>"))
            {
                var parameters = expression.Parameters;
                var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(parameters[0], "IsDeleted"), Expression.Constant(false));
                var originalBody = expression.Body;
                var fullExpr = Expression.And(originalBody, checkNotDeleted);
                var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, parameters);

                return _dbSet.AsNoTracking().Where(lambda);
            }
            else
            {
                return _dbSet.AsNoTracking().Where(expression);
            }
        }

        public void QuickUpdate(T original, T updated)
        {
            _dbSet.Attach(original);
            _context.Entry(original).CurrentValues.SetValues(updated);
            _context.Entry(original).State = EntityState.Modified;
        }

        public IList<T> SqlQuery(string query)
        {
            return _dbSet.FromSql(query).ToList();
        }

        public async Task<IList<T>> SqlQueryAsync(string query)
        {
            return await _dbSet.FromSql(query).ToListAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public bool hasFlag(string field)
        {
            var hasFlag = false;
            var genericTypeArguments = _dbSet.GetType().GenericTypeArguments;

            if (genericTypeArguments.Any())
            {
                var fields = ((System.Reflection.TypeInfo)(_dbSet.GetType().GenericTypeArguments.FirstOrDefault())).DeclaredFields;

                hasFlag = fields.Any(x => x.Name.Contains(field));
            }

            return hasFlag;
        }
    }
}
