using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CountryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CountryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CountryServices
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepo _countryRepo;
        public CountryService(ICountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public async Task<List<ViewCountryModel>> GetAllCountries(string name)
        {
            return await _countryRepo.GetCountrys(name);
        }

        public async Task<ViewCountryModel> GetCountry(string id)
        {
            return await _countryRepo.GetCountry(id);
        }
    }
}
