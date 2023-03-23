using BackendDevTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDevTest.Interfaces
{
    public interface IEtherscanClient
    {
        Task<Block> GetBlockByNumberAsync(string blockNumber);
        Task<int> GetBlockTransactionCountByNumberAsync(string blockNumber);
        Task<Transaction> GetTransactionByBlockNumberAndIndexAsync(string blockNumber, int index);
    }
}
