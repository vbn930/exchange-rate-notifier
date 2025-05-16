using ExchangeRateNotifierWorkerService;
using ExchangeRateNotifierWorkerService.Services;
using ExchangeRateNotifierWorkerService.Dtos;

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

Task<string> resTask = apiRequestWrapper.GetAsync(url); // URL만 전달
string res = await resTask; // await 키워드 추가
Console.WriteLine($"Result: {res}"); // 결과 앞에 "Result: " 추가

ExchangeRateData? data = JsonSerializer.Deserialize<ExchangeRateData>(res);
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
