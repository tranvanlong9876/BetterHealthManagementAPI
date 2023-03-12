using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CountryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CountryRepos
{
    public class CountryRepo : Repository<Country>, ICountryRepo
    {
        public CountryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<ViewCountryModel> GetCountry(string id)
        {
            var query = await context.Countries.Where(x=> x.Id.Equals(id.Trim())).Select(selector => new ViewCountryModel()
            {
                Id = selector.Id,
                Name = selector.Name
            }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<List<ViewCountryModel>> GetCountrys(string name)
        {
            var query = from country in context.Countries
                        select country;

            if (!String.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name.Trim()));
            }

            var data = await query.Select(selector => new ViewCountryModel()
            {
                Id = selector.Id,
                Name = selector.Name
            }).ToListAsync();

            return data;
        }
    }
}
