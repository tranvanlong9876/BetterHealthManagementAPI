using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CountryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CountryServices
{
    public interface ICountryService
    {
        public Task<List<ViewCountryModel>> GetAllCountries(string name);

        public Task<ViewCountryModel> GetCountry(string id);
    }
}
