namespace U13.WeatherForecast.MinimalAPI.Models.GeoCoding
{
    public class GeoCodingResult
    {
        public Result Result { get; set; }
    }

    public class Benchmark
    {
        public string Id { get; set; }
        public string BenchmarkName { get; set; }
        public string BenchmarkDescription { get; set; }
        public bool IsDefault { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Input
    {
        public Benchmark Benchmark { get; set; }
        public Address Address { get; set; }
    }

    public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class TigerLine
    {
        public string TigerLineId { get; set; }
        public string Side { get; set; }
    }

    public class AddressComponents
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string PreQualifier { get; set; }
        public string PreDirection { get; set; }
        public string PreType { get; set; }
        public string StreetName { get; set; }
        public string SuffixType { get; set; }
        public string SuffixDirection { get; set; }
        public string SuffixQualifier { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class AddressMatch
    {
        public string MatchedAddress { get; set; }
        public Coordinates Coordinates { get; set; }
        public TigerLine TigerLine { get; set; }
        public AddressComponents AddressComponents { get; set; }
    }

    public class Result
    {
        public Input Input { get; set; }
        public List<AddressMatch> AddressMatches { get; set; }
    }

}
