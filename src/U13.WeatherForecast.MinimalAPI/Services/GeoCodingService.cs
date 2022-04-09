using System.Net;
using System.Text.Json;
using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class GeoCodingService : IGeoCodingService
    {
        const string GET_GEO_CODING_BY_ADDRESS_URL = "https://geocoding.geo.census.gov/geocoder/locations/onelineaddress?address={0}&benchmark=2020&format=json";
        private readonly HttpClient httpClient;
        public GeoCodingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<GeoCodingResult> GetGeoCodingByAddress(string address)
        {
            GeoCodingResult result = default;
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var response = await httpClient.GetAsync(string.Format(GET_GEO_CODING_BY_ADDRESS_URL, address));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<GeoCodingResult>(content, options);
            }
            return result;
        }
    }
}
