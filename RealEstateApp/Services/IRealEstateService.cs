using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApp.Models;

namespace RealEstateApp.Services
{
    public interface IRealEstateService
    {
        Task<RealEstateSearchResults> Search(string address, string city, string state, string zipcode);
    }
}
