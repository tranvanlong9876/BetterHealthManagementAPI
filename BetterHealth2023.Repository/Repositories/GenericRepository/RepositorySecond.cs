using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository
{
    public class RepositorySecond<T1, T2> : IRepositorySecond<T1, T2> where T1 : class
                                                                      where T2 : class
    {
        protected readonly BetterHealthManagementContext context;
        private DbSet<T1> _entities;
        private readonly IMapper mapper;

        public RepositorySecond(BetterHealthManagementContext context, IMapper mapper)
        {
            this.context = context;
            _entities = context.Set<T1>();
            this.mapper = mapper;
        }
        public async Task<List<T2>> GetAll()
        {
            List<T1> results = await _entities.ToListAsync();
            return results.Select(model => mapper.Map<T2>(model)).ToList();
        }

        public async Task<PagedResult<T2>> GetAllPaging(PagingRequestBase requestBase)
        {
            int totalRow = await _entities.CountAsync();
            List<T1> results = await _entities.Skip((requestBase.pageIndex - 1) * requestBase.pageItems)
                                              .Take(requestBase.pageItems)
                                              .ToListAsync();
            List<T2> convertResults = results.Select(model => mapper.Map<T2>(model)).ToList();
            var pagedResult = new PagedResult<T2>(convertResults, totalRow, requestBase.pageIndex, requestBase.pageItems);
            return pagedResult;
        }

        public async Task<T1> GetClassDBModel(string id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T2> GetClassViewModel(string id)
        {
            T1 results = await _entities.FindAsync(id);
            T2 convertResults = mapper.Map<T2>(results);
            return convertResults; 
        }

        public async Task<bool> Insert(T1 entity)
        {
            await _entities.AddAsync(entity);
            await Update();
            return true;
        }

        public async Task<bool> Update()
        {
            await context.SaveChangesAsync();
            return true;
        }
    }
}
