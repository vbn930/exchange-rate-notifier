using ExchangeRateNotifierWorkerService.Utils;

namespace ExchangeRateNotifierWorkerService;

public class Worker : BackgroundService
{
    private readonly Logger _logger;

    public Worker(IConfiguration configuration)
    {
        if (null == configuration)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        string serviceName = configuration["Logging:ServiceName"];
        _logger = new Logger(serviceName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
