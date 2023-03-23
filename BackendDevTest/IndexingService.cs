using BackendDevTest.Interfaces;
using BackendDevTest.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDevTest
{
    public class IndexingService
    {
        private readonly IEtherscanClient _etherscanClient;
        private readonly BackenddevtestContext _context;
        private readonly ILogger<IndexingService> _logger;

        public IndexingService(IEtherscanClient etherscanClient, BackenddevtestContext context, ILogger<IndexingService> logger)
        {
            _etherscanClient = etherscanClient;
            _context = context;
            _logger = logger;
        }

        public async Task IndexBlocksAndTransactionsAsync(int startBlockNumber, int endBlockNumber)
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = startBlockNumber; i <= endBlockNumber; i++)
            {
                try
                {
                    var blockNumber = "0x" + i.ToString("X");

                    var block = await _etherscanClient.GetBlockByNumberAsync(blockNumber);

                    if (block != null)
                    {
                        var blockRecord = new Block
                        {
                            BlockNumber = block.BlockNumber,
                            Hash = block.Hash,
                            BlockId = block.BlockId,
                            BlockReward = block.BlockReward,
                            GasLimit = block.GasLimit,
                            GasUsed = block.GasUsed,
                            Miner = block.Miner,
                            ParentHash = block.ParentHash
                        };

                        _context.Blocks.Add(blockRecord);

                        var count = await _etherscanClient.GetBlockTransactionCountByNumberAsync(blockNumber);

                        if (count > 0)
                        {
                            for (int j = 0; j < count; j++)
                            {
                                var transaction = await _etherscanClient.GetTransactionByBlockNumberAndIndexAsync(blockNumber, j);

                                if (transaction != null)
                                {
                                    var transactionRecord = new Transaction
                                    {
                                        BlockId = blockRecord.BlockId,
                                        Hash = transaction.Hash,
                                        FromAddress = transaction.FromAddress,
                                        Gas = transaction.Gas,
                                        GasPrice = transaction.GasPrice,
                                        TransactionId = transaction.TransactionId,
                                        ToAddress = transaction.ToAddress,
                                        TransactionIndex = transaction.TransactionIndex,
                                        Value = transaction.Value
                                    };

                                    _context.Transactions.Add(transactionRecord);
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    Console.WriteLine($"Block {i} indexed");
                    _logger.LogInformation($"Indexed block {blockNumber}");
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, $"Error indexing block {i}");
                }
               
            }

            stopwatch.Stop();
            _logger.LogInformation($"Indexing completed in {stopwatch.Elapsed.TotalSeconds} seconds.");
            Console.WriteLine($"Indexing completed in {stopwatch.Elapsed}");
        }
    }
}
