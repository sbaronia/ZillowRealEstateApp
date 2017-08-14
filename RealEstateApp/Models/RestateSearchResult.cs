using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateApp.Models
{
    public class LinkPage
    {
        public string HomeDetails { set; get; }
        public string ChartData { set; get; }
        public string MapHome { set; get; }
        public string SimilarSales { set; get; }
    }

    public class Address
    {
        public string Street { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Zipcode { set; get; }
        public Decimal Latitude { set; get; }
        public Decimal Longitude { set; get; }

    }

    public class EstimateData {
        public string EstimatePrice { set; get; }
        public string LastUpateDate { set; get; }
        public string ThirtyDayChange { set; get; }
        public string HighValuation { set; get; }
        public string LowValuation { set; get; }
        public string PercentileValue { set; get; }
    };

    public class RealEstateNeighborhood
    {
        public string HomeValueIndex { set; get; }
        public string HomeValueIndex1YrChange { set; get; }
        public string LinkToRegionOverview { set; get; }
        public string LinkForSaleByOwner { set; get; }
        public string LinkForSaleHome { set; get; }
    };

    public enum ErrorCodes
    {
        NONE,
        SERVER_ERROR,
        MISSING_ADDRESS,
        MISSING_CITY_STATE,
        MISSING_ZIP,
        INVALID_ADDRESS,
        SERVICE_NO_AVALIABLE,
        NO_RESULTS,
        NO_SERVICE,
        LIMIT_REACHED
    }

    public class RealEstateSearchResults
    {
        public ErrorCodes Error;
        public List<RealEstateSearchResult> Results;
    }

    public class RealEstateSearchResult
    {
        public Address Address { set; get; }
        public LinkPage Links { set; get; }
        public EstimateData Estimate { set; get; }
        public EstimateData RentalEstimate { set; get; }
        public List<RealEstateNeighborhood> NeighborHoods { set; get; }
    }
}
