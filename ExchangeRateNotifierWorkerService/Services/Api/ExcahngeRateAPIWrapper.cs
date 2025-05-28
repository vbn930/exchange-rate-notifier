using ExchangeRateNotifierWorkerService;
using ExchangeRateNotifierWorkerService.Services;
using ExchangeRateNotifierWorkerService.Dtos;
using ExchangeRateNotifierWorkerService.Clients;
using ExchangeRateNotifierWorkerService.Utils;
using ExchangeRateClientService.Dtos;

using System.Text;
using System.Text.Json;

namespace ExchangeRateNotifierWorkerService.Services;

class ExchangeRateAPIWrapper
{
    private readonly APIRequestWrapper _apiRequestWrapper;
    private readonly Logger _logger;
    private readonly List<ConvertedExchangeRateData> _cache;

    public ExchangeRateAPIWrapper(IConfiguration configuration, APIRequestWrapper apiRequestWrapper)
    {
        if (null == configuration)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        _apiRequestWrapper = apiRequestWrapper;

        string serviceName = configuration["Logging:ServiceName"];
        _logger = new Logger(serviceName);
        _cache = new List<ConvertedExchangeRateData>();
    }

    public async Task<ConvertedExchangeRateData> GetExchangeRateFromClient()
    {
        using (var log = _logger.StartMethod(nameof(GetExchangeRateFromClient)))
        {
            string endpoint = "";
            log.SetAttribute("API Endpoint", endpoint);
            
            var res = await _apiRequestWrapper.GetAsync(endpoint);
            if (string.IsNullOrEmpty(res))
            {
                throw new ArgumentException("Failed to get data from Exchange Rate Client");
            }

            var data = JsonSerializer.Deserialize<ConvertedExchangeRateData>(res);

            if (data == null)
            {
                throw new ArgumentException("Failed to serialize the respons data");
            }
            _cache.Add(data);

            return data;
        }
    }
}