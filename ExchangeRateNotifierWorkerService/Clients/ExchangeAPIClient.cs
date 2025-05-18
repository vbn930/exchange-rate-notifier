using System.Text.Json;

using ExchangeRateNotifierWorkerService.Services;
using ExchangeRateNotifierWorkerService.Dtos;

namespace ExchangeRateNotifierWorkerService.Clients;

public class ExchangeAPIClient
{
    private readonly APIRequestWrapper _apiRequestWrapper;
    private readonly string _token;
    private readonly string _apiRequestUrl;
    private readonly List<ExchangeRateData> _dataStack;

    public ExchangeAPIClient(
        APIRequestWrapper apiRequestWrapper,
        string token
    )
    {
        _apiRequestWrapper = apiRequestWrapper;
        _token = token;
        _apiRequestUrl = $"https://openexchangerates.org/api/latest.json?app_id={token}&base=USD&prettyprint=true&show_alternative=false";
        _dataStack = new List<ExchangeRateData>();
    }

    private async Task<string> RequestExchangeRateDataAsync()
    {
        string? res = await _apiRequestWrapper.GetAsync(_apiRequestUrl);
        if (res == null) { throw new ArgumentNullException("Response is null"); }

        return res;
    }

    public async Task<ExchangeRateData> GetExchangeRateDataAsync()
    {
        string res = await RequestExchangeRateDataAsync();
        ExchangeRateData? data = JsonSerializer.Deserialize<ExchangeRateData>(res);
        if (data != null)
        {
            Console.WriteLine($"Timestamp: {data.Timestamp}");
            Console.WriteLine($"Rate: 1USD->{data.Rates["KRW"]}₩");

            _dataStack.Add(data);
            return data;
        }
        else
        {
            Console.WriteLine("API Result is NULL");
            throw new ArgumentException("Failed to get exchange data");
        }
    }

    public async Task SaveDataStackIntoDB()
    {
        //TODO: DB에 data stack 저장
        await Task.Delay(0);
        ClearDataStack();
    }

    public void ClearDataStack()
    {
        _dataStack.Clear();
    }
}