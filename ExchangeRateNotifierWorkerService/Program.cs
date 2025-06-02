using ExchangeRateNotifierWorkerService;
using ExchangeRateNotifierWorkerService.Services;
using ExchangeRateNotifierWorkerService.Dtos;
using ExchangeRateNotifierWorkerService.Clients;

using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
