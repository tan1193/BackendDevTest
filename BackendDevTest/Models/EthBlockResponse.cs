using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDevTest.Models
{
    public class EthBlockResponse
    {
        public string Jsonrpc { get; set; }
        public int Id { get; set; }
        public EthBlockResult Result { get; set; }
    }

    public class EthBlockResult
    {
        public string BaseFeePerGas { get; set; }
        public string Difficulty { get; set; }
        public string ExtraData { get; set; }
        public string GasLimit { get; set; }
        public string GasUsed { get; set; }
        public string Hash { get; set; }
        public string LogsBloom { get; set; }
        public string Miner { get; set; }
        public string MixHash { get; set; }
        public string Nonce { get; set; }
        public string Number { get; set; }
        public string ParentHash { get; set; }
        public string ReceiptsRoot { get; set; }
        public string Sha3Uncles { get; set; }
        public string Size { get; set; }
        public string StateRoot { get; set; }
        public string Timestamp { get; set; }
        public string TotalDifficulty { get; set; }
        public TransactionResult[] Transactions { get; set; }
        public string TransactionsRoot { get; set; }
        public string[] Uncles { get; set; }
    }

    public  class BlockNumberResponse
    {
        public string jsonrpc { get; set; }
        public string id { get; set; }
        public string result { get; set; }
    }
}
