using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
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
        Task<bool> InsertRange(List<T> entityList);
        Task<bool> Update();

        Task<bool> Remove(T entity);
        Task<List<TView>> GetAll<TView>();
        Task<PagedResult<TView>> GetAllPaging<TView>(PagingRequestBase requestBase);
        Task<PagedResult<TView>> PagingExistingQuery<TView>(IQueryable<T> query, int pageIndex, int pageItems);
        TOut TransferBetweenTwoModels<TIn, TOut>(TIn model);

        void TransferBetweenTwoModels<TIn, TOut>(ref TOut dbmodel, TIn model);

    }
}
