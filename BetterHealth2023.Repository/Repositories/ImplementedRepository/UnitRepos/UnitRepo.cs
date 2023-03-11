using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UnitRepos
{
    public class UnitRepo : Repository<Unit>, IUnitRepo
    {
        public UnitRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> CheckExistUnitName(string unitName)
        {
            Unit unitModel = await context.Units.Where(x => x.UnitName.ToLower().Trim().Equals(unitName.ToLower().Trim())).FirstOrDefaultAsync();
            if (unitModel != null) return true;
            return false;
        }

        public async Task<bool> CheckExistUnitNameUpdate(string id, string unitName)
        {
            Unit unitModel = await context.Units.Where(x => (x.UnitName.ToLower().Trim().Equals(unitName.ToLower().Trim())) && (x.Id.Trim() != id.Trim())).FirstOrDefaultAsync();
            if (unitModel != null) return true;
            return false;
        }

        public async Task<PagedResult<ViewUnitModel>> GetAll(GetUnitPagingModel pagingModel)
        {
            var query = from unit in context.Units
                        select unit;

            if (!string.IsNullOrEmpty(pagingModel.UnitName))
            {
                query = query.Where(x => x.UnitName.Contains(pagingModel.UnitName.Trim()));
            }

            if (pagingModel.isCountable.HasValue)
            {
                query = query.Where(x => x.IsCountable.Equals(pagingModel.isCountable.Value));
            }

            int totalRow = await query.CountAsync();

            var UnitModelList = await query.Skip((pagingModel.pageIndex - 1) * pagingModel.pageItems)
                .Take(pagingModel.pageItems)
                .Select(selector => new ViewUnitModel()
                {
                    Id = selector.Id,
                    UnitName = selector.UnitName,
                    IsCountable = selector.IsCountable,
                    CreatedDate = selector.CreatedDate,
                    Status = selector.Status
                }).ToListAsync();
            var pageResult = new PagedResult<ViewUnitModel>(UnitModelList, totalRow, pagingModel.pageIndex, pagingModel.pageItems);
            return pageResult;
        }
        
    }
}
