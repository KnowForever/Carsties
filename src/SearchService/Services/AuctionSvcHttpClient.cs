using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionSvcHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Item>> GetItemsForSearchDb()
        {
            string AUCTIONS_SVC_ENDPOINT = $"{_config["AuctionServiceUrl"]}/api/auctions";

            var lastUpdated = await DB.Find<Item, string>()
                .Sort(i => i.Descending(_ => _.UpdatedAt))
                .Project(i => i.UpdatedAt.ToString())
                .ExecuteFirstAsync();

            return await _httpClient.GetFromJsonAsync<List<Item>>($"{AUCTIONS_SVC_ENDPOINT}?date={lastUpdated}");
        }
    }
}
