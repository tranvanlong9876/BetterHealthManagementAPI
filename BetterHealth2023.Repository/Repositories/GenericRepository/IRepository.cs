using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(string id);
        Task<TView> GetViewModel<TView>(string id);
        Task<bool> Insert(T entity);
        Task<bool> Update();
        Task<List<TView>> GetAll<TView>();
        Task<PagedResult<TView>> GetAllPaging<TView>(PagingRequestBase requestBase);
        Task<PagedResult<TView>> PagingExistingQuery<TView>(IQueryable<T> query, int pageIndex, int pageItems);


    }
}
