using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using ZillowModels;
using System.Xml.Serialization;
using RealEstateApp.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace RealEstateApp.Services
{

    public class ZillowRealEstateServiceImpl : IRealEstateService
    {
        private readonly ILogger<ZillowRealEstateServiceImpl> _logger;
        private static readonly string WEB_SERVICE_URL = "https://www.zillow.com/";
        private static readonly string API_KEY = "X1-ZWz1dyb53fdhjf_6jziz";
        private static Dictionary<String, ErrorCodes> errorMap;

       static ZillowRealEstateServiceImpl()
        {
            errorMap = new Dictionary<string, ErrorCodes>();
            errorMap.Add("0", ErrorCodes.NONE);
            errorMap.Add("1", ErrorCodes.SERVER_ERROR);
            errorMap.Add("2", ErrorCodes.SERVER_ERROR);
            errorMap.Add("3", ErrorCodes.SERVICE_NO_AVALIABLE);
            errorMap.Add("4", ErrorCodes.SERVICE_NO_AVALIABLE);
            errorMap.Add("500", ErrorCodes.MISSING_ADDRESS);
            errorMap.Add("501", ErrorCodes.MISSING_CITY_STATE);
            errorMap.Add("502", ErrorCodes.NO_RESULTS);
            errorMap.Add("503", ErrorCodes.INVALID_ADDRESS);
            errorMap.Add("504", ErrorCodes.NO_SERVICE);
            errorMap.Add("505", ErrorCodes.SERVER_ERROR);
            errorMap.Add("506", ErrorCodes.INVALID_ADDRESS);
            errorMap.Add("507", ErrorCodes.NO_RESULTS);
        }

        public ZillowRealEstateServiceImpl(ILogger<ZillowRealEstateServiceImpl> logger)
        {
            _logger = logger;
            Console.Out.WriteLine("Service started");
        }

        async Task<RealEstateSearchResults> IRealEstateService.Search(string address, string city, string state, string zipcode)
        {
            return await searchRestate(address, city, state, zipcode);
        }

        private async Task<RealEstateSearchResults> searchRestate(string address, string city, string state, string zipcode)
        {
            using (HttpClient client = new HttpClient())
            {
                string citystatezip = string.Empty;
                if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(state))
                    citystatezip = string.Format("{0} {1}", city, state);
                else if (!string.IsNullOrEmpty(zipcode))
                    citystatezip = zipcode;

                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                    //client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                    //client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.8));

                    var response = await client.GetAsync(String.Format("/webservice/GetSearchResults.htm?zws-id={0}&address={1}&citystatezip={2}",
                                                        API_KEY, address, citystatezip));
                    response.EnsureSuccessStatusCode();
                    string streamResult = await response.Content.ReadAsStringAsync();
                    
                    streamResult = "<searchresults>"+ streamResult.Substring(38+302);
                    streamResult = streamResult.Replace("</SearchResults:searchresults>", "</searchresults>");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(streamResult);
                    string json = JsonConvert.SerializeXmlNode(doc);
                    ZillowResponse zResponse = JsonConvert.DeserializeObject<ZillowResponse>(json);
                    Console.Out.WriteLine(json);
                    searchresults oresult = zResponse.searchresults; 
                    if (oresult != null && oresult is searchresults)
                    {
                        searchresults searchResult = oresult as searchresults;
                        Message msg = searchResult.message;
                        RealEstateSearchResults restateSearchResults = new RealEstateSearchResults();
                        if (msg != null && msg.code != null) errorMap.TryGetValue(msg.code, out restateSearchResults.Error);
                        
                        if (msg.limitwarning && "0".CompareTo(msg.code) == 0)
                        {
                            if (searchResult.response.results.result != null)
                            {
                                restateSearchResults.Results = new List<RealEstateSearchResult>();
                                foreach (SimpleProperty result in searchResult.response.results.result)
                                {

                                    RealEstateSearchResult sResult = new RealEstateSearchResult
                                    {
                                        Address = createAddress(result.address),
                                        Links = createLinks(result.links),
                                        Estimate = createEstimate(result.zestimate),
                                        RentalEstimate = createEstimate(result.rentzestimate),
                                        NeighborHoods = createNeighborHoods(result.localRealEstate)
                                    };
                                    restateSearchResults.Results.Add(sResult);
                                }
                            }
                            else
                            {
                                restateSearchResults.Error = ErrorCodes.LIMIT_REACHED;
                            }
                            
                        }
                        else
                        {
                            if (!msg.limitwarning) restateSearchResults.Error = ErrorCodes.LIMIT_REACHED;
                            
                        }
                        return restateSearchResults;
                    }
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.Out.WriteLine(httpRequestException);
                    return new RealEstateSearchResults { Error = ErrorCodes.SERVER_ERROR };
                }
                catch(Exception ex)
                {
                    Console.Out.WriteLine(ex);
                    return new RealEstateSearchResults { Error = ErrorCodes.SERVER_ERROR };
                }
            }

            return new RealEstateSearchResults { Error = ErrorCodes.SERVER_ERROR };
        }

        private List<Models.RealEstateNeighborhood> createNeighborHoods(List<ZillowModels.LocalRealEstateRegion> zlocalRealEstates)
        {
            List<Models.RealEstateNeighborhood> neighborhoods = null;
            if (zlocalRealEstates != null)
            {
                neighborhoods = new List<Models.RealEstateNeighborhood>();
                foreach (LocalRealEstateRegion region in zlocalRealEstates)
                {
                    Models.RealEstateNeighborhood neidhborhood = new Models.RealEstateNeighborhood
                    {
                        HomeValueIndex = region.region.zindexValue,
                        HomeValueIndex1YrChange = region.region.zindexOneYearChange,
                        LinkToRegionOverview = region.region.links != null ? region.region.links.overview : null,
                        LinkForSaleByOwner = region.region.links != null ? region.region.links.forSaleByOwner : null,
                        LinkForSaleHome = region.region.links != null ? region.region.links.forSale : null
                    };
                    neighborhoods.Add(neidhborhood);
                }
            }

            return neighborhoods;
        }


        private Models.Address createAddress(ZillowModels.Address zAddress)
        {
            Models.Address address = null;
            if (zAddress != null)
            {
                address = new Models.Address
                {
                    Street = zAddress.street,
                    City = zAddress.city,
                    State = zAddress.state,
                    Zipcode = zAddress.zipcode,
                    Latitude = zAddress.latitude,
                    Longitude = zAddress.longitude
                };
            }

            return address;
        }

        private Models.LinkPage createLinks(ZillowModels.Links zLink)
        {
            Models.LinkPage link = null;
            if (zLink != null)
            {
                link = new Models.LinkPage
                {
                    HomeDetails = zLink.homedetails,
                    ChartData = zLink.graphsanddata,
                    MapHome = zLink.mapthishome,
                    SimilarSales = string.Empty
                };
            }

            return link;
        }

        private Models.EstimateData createEstimate(ZillowModels.Zestimate zEstimate)
        {
            Models.EstimateData estimate = null;
            if (zEstimate != null)
            {
                estimate = new Models.EstimateData { EstimatePrice = zEstimate.amount.value, ThirtyDayChange = zEstimate.valueChange.value, HighValuation = zEstimate.valuationRange.high.value, LowValuation = zEstimate.valuationRange.low.value, PercentileValue = zEstimate.percentile, LastUpateDate = zEstimate.lastupdated };
            }
            return estimate;
        }
            
    }
}
