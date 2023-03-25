using BackendDevTest.Interfaces;
using BackendDevTest.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
                            BlockNumber = Convert.ToInt32(block.Number,16),
                            Hash = block.Hash,
                            GasLimit = Convert.ToInt64(block.GasLimit, 16),
                            GasUsed = Convert.ToInt64(block.GasUsed, 16),
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
                                        Hash = transaction.hash,
                                        FromAddress = transaction.from,
                                        Gas = Convert.ToInt64(transaction.gas, 16),
                                        GasPrice = Convert.ToInt64(transaction.gasPrice, 16),
                                        ToAddress = transaction.to,
                                        TransactionIndex = Convert.ToInt32(transaction.transactionIndex, 16),
                                        Value = Convert.ToInt64(transaction.value, 16)
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
