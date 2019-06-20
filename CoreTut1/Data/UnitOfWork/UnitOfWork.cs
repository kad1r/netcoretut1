using Data.Dtos;
using Data.Model;
using Data.Repository;
using System;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected string _connectionString;
        private UnitOfWork uow;
        public PhotographyContext _context;

        public UnitOfWork(PhotographyContext context)
        {
            //Database.SetInitializer<PhotographyContext>(null);

            if (context == null)
            {
                context = new PhotographyContext();
                _context = context;
                uow = new UnitOfWork(_context);
            }

            _context = context;
        }

        public PhotographyContext DbContext
        {
            get
            {
                if (_context == null)
                {
                    _context = new PhotographyContext();
                }

                return _context;
            }
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (uow == null)
            {
                uow = new UnitOfWork(_context);
            }

            return new Repository<T>(uow);
        }

        public void ChangeConfigurations(EfConfig config)
        {
            DbContext.ChangeTracker.AutoDetectChangesEnabled = config.AutoDetectChanges;
            DbContext.ChangeTracker.LazyLoadingEnabled = config.LazyLoading;
        }

        public int SaveChanges()
        {
            try
            {
                #region WRITE LOG

                //var changes = _context.ChangeTracker.Entries()
                //    .Select(x => new UowChangeLog
                //    {
                //        OriginalValues = x.State != EntityState.Added ? x.OriginalValues.PropertyNames.ToDictionary(c => c, c => x.OriginalValues[c]) : null,
                //        CurrentValues = x.CurrentValues.PropertyNames.ToDictionary(c => c, c => x.CurrentValues[c]),
                //        State = x.State,
                //        DateInserted = DateTime.Now
                //    })
                //    .FirstOrDefault();

                //WriteUOWLogs(changes);

                #endregion WRITE LOG

                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // catch DbEntityValidationException errors
                var msg = ex.Message;
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                #region WRITE LOG

                //var changes = _context.ChangeTracker.Entries()
                //    .Select(x => new UowChangeLog
                //    {
                //        OriginalValues = x.State != EntityState.Added ? x.OriginalValues.PropertyNames.ToDictionary(c => c, c => x.OriginalValues[c]) : null,
                //        CurrentValues = x.CurrentValues.PropertyNames.ToDictionary(c => c, c => x.CurrentValues[c]),
                //        State = x.State,
                //        DateInserted = DateTime.Now
                //    })
                //    .FirstOrDefault();

                //WriteUOWLogs(changes);

                #endregion WRITE LOG

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // catch DbEntityValidationException errors
                var msg = ex.Message;
                throw;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
