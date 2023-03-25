using BackendDevTest;
using BackendDevTest.Interfaces;
using BackendDevTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();
// Configure Etherscan API client
services.AddHttpClient<IEtherscanClient, EtherscanClient>(client =>
{
    client.BaseAddress = new Uri("https://api.etherscan.io/api");
});
services.AddSingleton(configuration["EtherscanApiKey"]);


// Configure database context
services.AddDbContext<BackenddevtestContext>(options =>
    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                               Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")));

// Configure indexing service
services.AddTransient<IndexingService>();

// Configure logging
services.AddLogging(configure =>
{
    configure.AddConsole();
    //configure.AddFile("indexing.log");
});

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Get indexing service instance
var indexingService = serviceProvider.GetService<IndexingService>();

// Start indexing
await indexingService.IndexBlocksAndTransactionsAsync(12100001, 12100002);


//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Serilog;
//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//var builder = new HostBuilder()
//               .ConfigureAppConfiguration((hostingContext, config) =>
//               {
//                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//                   config.AddEnvironmentVariables();
//               })
//               .ConfigureServices((hostingContext, services) =>
//               {
//                   services.AddLogging(loggingBuilder =>
//                   {
//                       loggingBuilder.ClearProviders();
//                       loggingBuilder.AddSerilog(new LoggerConfiguration()
//                           .WriteTo.Console()
//                           .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
//                           .CreateLogger());
//                   });

//                   services.AddHttpClient<IEtherscanClient, EtherscanClient>(client =>
//                   {
//                       client.BaseAddress = new Uri("https://api.etherscan.io/api");
//                   }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//                   {
//                       AutomaticDecompression = System.Net.DecompressionMethods.GZip
//                   }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(5)))
//                     .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)))
//                     .ConfigureHttpClient((sp, client) =>
//                     {
//                         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                         var apiKey = hostingContext.Configuration.GetValue<string>("EtherscanApiKey");
//                         client.DefaultRequestHeaders.Add("Api-Key", apiKey);
//                     });

//                   services.AddTransient<IIndexer, Indexer>();
//               });

//await builder.RunConsoleAsync();
