using BackendDevTest.Interfaces;
using BackendDevTest.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace BackendDevTest
{
    public class EtherscanClient : IEtherscanClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public EtherscanClient(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;

        }

        public async Task<EthBlockResult> GetBlockByNumberAsync(string blockNumber)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?module=proxy&action=eth_getBlockByNumber&tag={blockNumber}&boolean=true&apikey={_apiKey}");
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into Block object
            var block = JsonConvert.DeserializeObject<EthBlockResponse>(content);

            return block.Result;
        }

        public async Task<int> GetBlockTransactionCountByNumberAsync(string blockNumber)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?module=proxy&action=eth_getBlockTransactionCountByNumber&tag={blockNumber}&apikey={_apiKey}");
            var content = await response.Content.ReadAsStringAsync();

            // Convert hex string to int
            var count = Convert.ToInt32(JsonConvert.DeserializeObject<BlockNumberResponse>(content).result, 16);

            return count;
        }

        public async Task<TransactionResult> GetTransactionByBlockNumberAndIndexAsync(string blockNumber, int index)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?module=proxy&action=eth_getTransactionByBlockNumberAndIndex&tag={blockNumber}&index={index}&apikey={_apiKey}");
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into Transaction object
            var transaction = JsonConvert.DeserializeObject<TransactionResponse>(content);

            return transaction.result;
        }
    }
}
