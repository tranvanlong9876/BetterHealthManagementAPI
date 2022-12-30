using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BetterHealthManagementContext context;
        private DbSet<T> _entities;

        public Repository(BetterHealthManagementContext context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> Get(string id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<string> Insert(T entity)
        {
            await _entities.AddAsync(entity);
            await Update();
            return entity.GetType().GetProperty("Id").GetValue(entity).ToString();
        }

        public async Task Update()
        {
            await context.SaveChangesAsync();
        }
    }
}
