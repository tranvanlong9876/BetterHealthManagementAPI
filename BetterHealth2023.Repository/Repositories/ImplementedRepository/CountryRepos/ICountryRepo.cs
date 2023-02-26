using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CountryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CountryRepos
{
    public interface ICountryRepo : IRepository<Country>
    {
        public Task<List<ViewCountryModel>> GetCountrys(string name);
        public Task<ViewCountryModel> GetCountry(string id);
    }
}
