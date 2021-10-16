using BethanyPieShopHRM.Shared;
using System.Collections.Generic;

namespace BethanysPieShopHRM.Api.Models
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountries();
        Country GetCountryById(int countryId);
    }
}
