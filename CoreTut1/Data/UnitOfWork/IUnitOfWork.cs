using Data.Dtos;
using Data.Model;
using Data.Repository;
using System;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        PhotographyContext DbContext { get; }
        IRepository<T> Repository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void ChangeConfigurations(EfConfig config);
    }
}
