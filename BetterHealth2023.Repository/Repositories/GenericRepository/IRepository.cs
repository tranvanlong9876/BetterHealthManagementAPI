using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Get(string id);
        Task<string> Insert(T entity);
        Task Update();
        Task<List<T>?> GetAll();

    }
}
