using BethanyPieShopHRM.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BethanyPieShopHRM.App.Services
{
    public interface ICountryDataService
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<Country> GetCountryById(int countryId);
    }
}
