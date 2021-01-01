using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Venjix.Infrastructure.DAL;

namespace Venjix.Infrastructure.Services
{
    public interface ITriggerRunnerService
    {
        void RunTriggers(IEnumerable<Recording> recordings);
    }

    public class TriggerRunnerService : ITriggerRunnerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TriggerRunnerService> _logger;
        private readonly ITelegramService _telegram;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IVenjixOptionsService _optionsService;

        public TriggerRunnerService(ILogger<TriggerRunnerService> logger, IMemoryCache cache, ITelegramService telegram, IServiceScopeFactory scopeFactory, IVenjixOptionsService optionsService, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _cache = cache;
            _telegram = telegram;
            _scopeFactory = scopeFactory;
            _optionsService = optionsService;
            _httpClientFactory = httpClientFactory;
        }

        public void RunTriggers(IEnumerable<Recording> recordings)
        {
            Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetService<VenjixContext>();

                foreach (var record in recordings)
                {
                    var triggers = await context.Triggers.Where(x => x.SensorId == record.SensorId)
                        .Include(x => x.Webhook)
                        .Include(x => x.Sensor)
                        .ToListAsync();

                    foreach (var trigger in triggers)
                    {
                        if (!trigger.ContinuousSend && _cache.TryGetValue(trigger.Name, out object cachedValue) && (double)cachedValue == record.Value)
                            continue;                                

                        if (!ValidateCondition(trigger.Event, trigger.Value, record.Value))
                            return;

                        try
                        {
                            _cache.Set(trigger.Name, record.Value, TimeSpan.FromSeconds(_optionsService.Options.TriggerCooldown));
                            if (trigger.Target == TriggerTarget.Telegram)
                            {
                                await ExecuteTelegram(record);
                                _logger.LogDebug("Telegram action trigger executed: {0}", trigger.Name);
                            }
                            else
                            {
                                await ExecuteWebhook(record, trigger.Webhook);
                                _logger.LogDebug("Webhook action trigger executed: {0}", trigger.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to execute trigger action: {0}.", trigger.Name);
                        }
                    }

                    _logger.LogDebug("All trigger for record {0} has been executed.", record.RecordingId);
                }
            });
        }

        private bool ValidateCondition(TriggerEvent triggerEvent, double triggerValue, double recordValue)
        {
            return triggerEvent switch
            {
                TriggerEvent.NewData => true,
                TriggerEvent.LargerThan => recordValue > triggerValue,
                TriggerEvent.LargerOrEqual => recordValue >= triggerValue,
                TriggerEvent.SmallerThan => recordValue < triggerValue,
                TriggerEvent.SmallerOrEqual => recordValue <= triggerValue,
                _ => false,
            };
        }

        private async Task ExecuteWebhook(Recording record, Webhook webhook)
        {
            StringContent body = null;
            if (webhook.JsonBody)
            {
                var content = webhook.BodyFormat.Replace("{{value}}", record.Value.ToString());
                body = new StringContent(content, Encoding.UTF8, "application/json");
            }

            var client = _httpClientFactory.CreateClient();
            var uri = webhook.UriFormat.Replace("{{value}}", record.Value.ToString());
            if (webhook.Method == "GET")
            {
                (await client.GetAsync(uri)).EnsureSuccessStatusCode();
            } 
            else if (webhook.Method == "POST")
            {
                (await client.PostAsync(uri, body)).EnsureSuccessStatusCode();
            }
            else
            {
                (await client.PutAsync(uri, body)).EnsureSuccessStatusCode();
            }
        }

        private async Task ExecuteTelegram(Recording record)
        {
            await _telegram.SendMessage($"New data on {record.Sensor.DisplayName}, recorded: {record.Value}");
        }
    }
}
