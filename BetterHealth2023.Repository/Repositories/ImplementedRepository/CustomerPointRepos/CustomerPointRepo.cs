using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos
{
    public class CustomerPointRepo : Repository<CustomerPoint>, ICustomerPointRepo
    {
        public CustomerPointRepo(BetterHealthManagementContext context) : base(context)
        {

        }

        public CustomerPointRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<int?> GetCustomerPointBasedOnCustomerId(string customerId)
        {
            int? plusPoint = await context.CustomerPoints.Where(x => x.CustomerId.Equals(customerId) && x.IsPlus).SumAsync(x => x.Point);
            int? minusPoint = await context.CustomerPoints.Where(x => x.CustomerId.Equals(customerId) && !x.IsPlus).SumAsync(x => x.Point);

            if(plusPoint == null && minusPoint == null)
            {
                return null;
            }

            plusPoint = plusPoint == null ? 0 : plusPoint;
            minusPoint = minusPoint == null ? 0 : minusPoint;

            return plusPoint - minusPoint;
        }

        public async Task<int?> GetCustomerPointBasedOnPhoneNumber(string phoneNumber)
        {
            var customerId = await context.Customers.Where(x => x.PhoneNo.Equals(phoneNumber)).Select(x => x.Id).SingleOrDefaultAsync();

            if (customerId == null) return null;

            return await GetCustomerPointBasedOnCustomerId(customerId);
        }
    }
}
