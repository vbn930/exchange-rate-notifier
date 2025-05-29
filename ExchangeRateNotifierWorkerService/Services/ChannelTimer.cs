using ExchangeRateNotifierWorkerService.Utils;
using ExchangeRateNotifierWorkerService.Dtos;
using ExchangeRateNotifierWorkerService.Services;
using System.Reflection.Metadata.Ecma335;

namespace ExchangeRateNotifierWorkerService.Services;

public class ChannelTimerService
{
    private readonly APIRequestWrapper _apiRequestWrapper;
    private readonly Logger _logger;
    private readonly Dictionary<ulong, System.Timers.Timer> _channelTimers;
    private readonly List<ChannelTimerSetting> _channelTimerSettings;

    public ChannelTimerService(IConfiguration configuration, APIRequestWrapper apiRequestWrapper)
    {
        if (null == configuration)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        string serviceName = configuration["Logging:ServiceName"];
        _logger = new Logger(serviceName);

        _apiRequestWrapper = apiRequestWrapper;

        _channelTimers = new Dictionary<ulong, System.Timers.Timer>();
        _channelTimerSettings = new List<ChannelTimerSetting>();

        //TODO: get timer settings from CosmosDB
    }

    public async Task AddChannelTimerAsync(ulong channelId, int intervalMinutes)
    {
        using (var log = _logger.StartMethod(nameof(AddChannelTimerAsync)))
        {
            log.SetAttribute("Channel Id", channelId);
            log.SetAttribute("Interval Minutes", intervalMinutes);
            if (_channelTimers.ContainsKey(channelId))
            {
                log.SetAttribute("Is Alrady Exist", true);
                return;
            }

            log.SetAttribute("Is Alrady Exist", false);

            var channelSetting = new ChannelTimerSetting(channelId, intervalMinutes);
            var channelTimer = new System.Timers.Timer();

            channelTimer.Interval = intervalMinutes * 60 * 1000;
            channelTimer.AutoReset = true;
            channelTimer.Elapsed += async (sender, e) =>
            {
                //TODO: Send discord message
            };

            channelTimer.Start();
            _channelTimers.Add(channelSetting.ChannelId, channelTimer);

            //TODO: Save channel setting in CosmosDB

            _channelTimerSettings.Add(channelSetting);
        }
    }

    public async Task<bool> DeleteChannelTimerAysnc(ulong channelId)
    {
        if (_channelTimers.TryGetValue(channelId, out System.Timers.Timer existingTimer))
        {
            existingTimer.Stop();
            existingTimer.Dispose();
            _channelTimers.Remove(channelId);
        }
        else
        {
            return false;
        }

        var existingSettingInList = _channelTimerSettings.FirstOrDefault(s => s.ChannelId == channelId);
        if (existingSettingInList != null)
        {
            _channelTimerSettings.Remove(existingSettingInList);
        }

        await Task.Delay(0);

        return true;
    }

    public async Task UpdateChannelTimerAsync(ChannelTimerSetting channelTimerSetting)
    {
        await DeleteChannelTimerAysnc(channelTimerSetting.ChannelId);

        //Set new timer with new setting
        await AddChannelTimerAsync(channelTimerSetting.ChannelId, channelTimerSetting.IntervalMinutes);

        //TODO: update setting in CosmosDB

        _channelTimerSettings.Add(channelTimerSetting);
    }
}