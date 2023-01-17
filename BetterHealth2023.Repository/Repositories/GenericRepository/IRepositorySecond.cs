using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository
{
    public interface IRepositorySecond<T1,T2> where T1 : class
                                              where T2 : class
    {
        Task<T1?> GetClassDBModel(string id);
        Task<T2?> GetClassViewModel(string id);
        Task<bool> Insert(T1 entity);
        Task<bool> Update();
        Task<List<T2>?> GetAll();
        Task<PagedResult<T2>?> GetAllPaging(PagingRequestBase requestBase);
    }
}
