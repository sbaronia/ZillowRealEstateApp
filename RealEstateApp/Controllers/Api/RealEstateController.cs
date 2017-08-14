using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Models;
using RealEstateApp.Services;
using System.Net;

namespace RealEstateApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class RealEstateController : Controller
    {
        private readonly IRealEstateService m_service;

        public RealEstateController(IRealEstateService service)
        {
            m_service = service;
        }

        // GET api/restate?address={address}&city={city}&state={state}&zipcode={zipcode}
        [HttpGet("address/{address}/city/{city}/state/{state}")]
        public async Task<IActionResult> GetAsync(string address, string city, string state)
        {
            return await search(address, city, state, null);
        }

        [HttpGet("address/{address}/zipcode/{zipcode}")]
        public async Task<IActionResult> GetAsync(string address, string zipcode)
        {
            return await search(address, null, null, zipcode);
        }

        private async Task<IActionResult> search(string address, string city, string state, string zipcode)
        {
            address = WebUtility.UrlDecode(address);
            RealEstateSearchResults result = await m_service.Search(address, city, state, zipcode);

            return Ok(result);
        }
    }
}
