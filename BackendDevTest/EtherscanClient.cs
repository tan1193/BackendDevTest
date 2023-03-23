using BackendDevTest.Interfaces;
using BackendDevTest.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDevTest
{
    public class EtherscanClient : IEtherscanClient
    {
        private readonly HttpClient _httpClient;
     
   
        public EtherscanClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
           
           
        }
      
    public async Task<Block> GetBlockByNumberAsync(string blockNumber)
        {
            var apiKey = "K58YN3UKZR27URMPJNK3BRGJFPKM9C4JAD";
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?module=proxy&action=eth_getBlockByNumber&tag={blockNumber}&boolean=true&apikey={apiKey}");
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into Block object
            var block = JsonConvert.DeserializeObject<Block>(content);

            return block;
        }

        public async Task<int> GetBlockTransactionCountByNumberAsync(string blockNumber)
        {
            var apiKey = "K58YN3UKZR27URMPJNK3BRGJFPKM9C4JAD";
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?module=proxy&action=eth_getBlockTransactionCountByNumber&tag={blockNumber}&apikey={apiKey}");
            var content = await response.Content.ReadAsStringAsync();

            // Convert hex string to int
            var count = int.Parse(content, NumberStyles.HexNumber);

            return count;
        }

        public async Task<Transaction> GetTransactionByBlockNumberAndIndexAsync(string blockNumber, int index)
        {
            var apiKey = "K58YN3UKZR27URMPJNK3BRGJFPKM9C4JAD";
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?module=proxy&action=eth_getTransactionByBlockNumberAndIndex&tag={blockNumber}&index={index}&apikey={apiKey}");
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into Transaction object
            var transaction = JsonConvert.DeserializeObject<Transaction>(content);

            return transaction;
        }
    }
}
