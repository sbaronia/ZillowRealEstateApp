using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZillowModels
{
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

    public class ZillowResponse
    {
        public searchresults searchresults  { get; set; }
    }

    public class searchresults
    {

        public searchresultsRequest request { get; set; }
        public Message message { get; set; }
        public searchresultsResponses response { get; set; }
    }

    public class searchresultsRequest
    {

        public string address { get; set; }

        public string citystatezip { get; set; }
    }

    public class Regions
    {

        public string zipcodeid { get; set; }

        public string cityid { get; set; }

        public string countyid { get; set; }

        public string stateid { get; set; }
    }

    public class AmountOptional
    {

        public Currency currency { get; set; }

        public Boolean currencyFieldSpecified { get; set; }

        [JsonProperty("@duration")]
        public string duration { get; set; }

        [JsonProperty("@deprecated")]
        public Boolean deprecated { get; set; }

        public Boolean deprecatedFieldSpecified { get; set; }

        [JsonProperty("#text")]
        public string value { get; set; }

        public AmountOptional()
        {
            this.deprecated = true;
        }

    }

    public enum Currency
    {

        /// <remarks/>
        USD,
    }

    public class Zestimate
    {

        public Amount amount { get; set; }

        [JsonProperty("last-updated")]
        public string lastupdated { get; set; }

        public AmountOptional oneWeekChange { get; set; }

        public AmountOptional valueChange { get; set; }

        public ZestimateValuationRange valuationRange { get; set; }

        public string percentile { get; set; }
    }

    public class Amount
    {

        [JsonProperty("@currency")]
        public Currency currency { get; set; }

        [JsonProperty("#text")]
        public string value { get; set; }

    }

    public class ZestimateValuationRange
    {

        public Amount low { get; set; }

        public Amount high { get; set; }
    }

    public class Address
    {

        public string street { get; set; }

        public string zipcode { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public Decimal latitude { get; set; }

        public Decimal longitude { get; set; }
    }

    public class DeprecatedType
    {

        public Boolean deprecated { get; set; }

        public string value { get; set; }

        public DeprecatedType()
        {
            this.deprecated = true;
        }
    }

    public class Links
    {

        public string homedetails { get; set; }

        public string graphsanddata { get; set; }

        public string mapthishome { get; set; }

        public string myestimator { get; set; }

        public DeprecatedType myzestimator { get; set; }

        public string comparables { get; set; }
    }

    public class Property
    {

        public uint zpid { get; set; }

        public Links links { get; set; }

        public Address address { get; set; }

        public string fIPScounty { get; set; }

        public string useCode { get; set; }

        public string taxAssessmentYear { get; set; }

        public string taxAssessment { get; set; }

        public string yearBuilt { get; set; }

        public string lotSizeSqFt { get; set; }

        public string finishedSqFt { get; set; }

        public string bathrooms { get; set; }

        public string bedrooms { get; set; }

        public string totalRooms { get; set; }

        public string lastSoldDate { get; set; }

        public Amount lastSoldPrice { get; set; }

    }

    public class SimpleProperty : Property
    {

        public Zestimate zestimate { get; set; }

        public Zestimate rentzestimate { get; set; }

        [JsonConverter(typeof(SingleValueArrayConverter<LocalRealEstateRegion>))]
        public List<LocalRealEstateRegion> localRealEstate { get; set; }

    }

    public class LocalRealEstateRegion
    {
        public Region region { get; set; }
    }

    public class Region
    {

        public string zindexValue { get; set; }

        public string zindexOneYearChange { get; set; }

        public LocalRealEstateRegionLinks links { get; set; }

        [JsonProperty("@name")]
        public string name { get; set; }

        [JsonProperty("@type")]
        public string type { get; set; }

        [JsonProperty("@id")]
        public uint id { get; set; }
    }

    /// <remarks/>
    public class LocalRealEstateRegionLinks
    {

        public string overview { get; set; }

        public string forSaleByOwner { get; set; }

        public string forSale { get; set; }

    }

    public class DetailedProperty : SimpleProperty
    {

        public Regions regions { get; set; }
    }

     public class ComparableProperty : SimpleProperty
    {

        public Decimal score { get; set; }

        public Boolean scoreFieldSpecified { get; set; }

    }

    public class Message
    {

        public string text { get; set; }

        public string code { get; set; }

        [JsonProperty("limit-warning")]
        public Boolean limitwarning { get; set; }

        public Boolean limitwarningFieldSpecified { get; set; }

        public Message()
        {
            this.limitwarning = true;
        }

     }


    public class searchresultsResponses
    {
        public searchresultsResponse results { get; set; }

    }

    public class searchresultsResponse
    {
        [JsonConverter(typeof(SingleValueArrayConverter<SimpleProperty>))]
        public List<SimpleProperty> result { get; set; }

    }
}

