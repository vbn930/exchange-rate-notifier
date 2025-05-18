using ExchangeRateNotifierWorkerService;
using ExchangeRateNotifierWorkerService.Services;
using ExchangeRateNotifierWorkerService.Dtos;
using ExchangeRateNotifierWorkerService.Clients;

using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

//Test code for api request
const string API_KEY = "c511651c1c924ddc9b621f261b3ee649";
string url = $"https://openexchangerates.org/api/latest.json?app_id={API_KEY}&base=USD&prettyprint=true&show_alternative=false";

var httpClient = new HttpClient();
var apiRequestWrapper = new APIRequestWrapper(httpClient);
var exchangeApiClient = new ExchangeAPIClient(apiRequestWrapper, API_KEY);

var data = await exchangeApiClient.GetExchangeRateDataAsync();

if (data != null)
{
    Console.WriteLine($"Timestamp: {data.Timestamp}");
    Console.WriteLine($"Rate: 1USD->{data.Rates["KRW"]}₩");
}
else
{
    Console.WriteLine("API Result is NULL");
}
//test code ends

var host = builder.Build();
host.Run();
