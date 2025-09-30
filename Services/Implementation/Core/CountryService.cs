using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ERP_System_Project.Services.Implementation.Core
{
    public class CountryService : GenericService<Country>, ICountryService
    {
        private readonly IMemoryCache _cache;
        private const string CountriesCacheKey = "CountriesCache";

        public CountryService(IUnitOfWork uow, IMemoryCache memoryCache) : base(uow)
        {
            _cache = memoryCache;
        }

        public new async Task<IEnumerable<Country>> GetAllWithCacheAsync()
        {
            if (_cache.TryGetValue(CountriesCacheKey, out IEnumerable<Country> countries))
            {
                return countries;
            }

            countries = await _repository.GetAllAsync();
            _cache.Set(CountriesCacheKey, countries, TimeSpan.FromMinutes(5));

            return countries;
        }

        public async Task<IEnumerable<Country>> SearchByNameAsync(string name)
        {
            var query = _repository.GetAllAsIQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.ToUpper().StartsWith(name.ToUpper()));
            }

            return await query.ToListAsync();
        }
    }
}
