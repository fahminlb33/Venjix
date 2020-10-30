using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Venjix.Infrastructure.DTO;

namespace Venjix.Infrastructure.Services
{
    public interface ITelegramService
    {
        Task SendMessage(string message);
        Task VerifyAndSaveBot(string token);
    }

    public class TelegramService : ITelegramService
    {
        private readonly ILogger<TelegramService> _logger;
        private readonly IWritableOptions<VenjixOptions> _options;
        private static HttpClient _httpClient = new HttpClient();

        private const string VerifyMessage = "VENJIX";

        public TelegramService(ILogger<TelegramService> logger, IWritableOptions<VenjixOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task VerifyAndSaveBot(string token)
        {
            try
            {
                var uri = $"https://api.telegram.org/bot{token}/getUpdates";
                var result = await _httpClient.GetAsync(uri);
                result.EnsureSuccessStatusCode();

                var response = await result.Content.ReadAsStreamAsync();
                var body = await JsonDocument.ParseAsync(response);
                if (!body.RootElement.GetProperty("ok").GetBoolean())
                {
                    _logger.LogWarning("The provided Telegram token is invalid.");
                    return;
                }

                var updates = body.RootElement.GetProperty("result");
                var message = updates.EnumerateArray().FirstOrDefault(x => x.GetProperty("message").GetProperty("text").GetString().Contains(VerifyMessage));
                var chatId = message.GetProperty("message").GetProperty("chat").GetProperty("id").GetInt32();

                await _options.Update(options =>
                {
                    options.IsTelegramTokenValid = true;
                    options.TelegramToken = token;
                    options.TelegramChatId = chatId;
                });
                _logger.LogInformation("Telegram Bot is activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when verifying Telegram token.");
                await _options.Update(options =>
                {
                    options.IsTelegramTokenValid = false;
                    options.TelegramToken = token;
                    options.TelegramChatId = 0;
                });
            }
        }

        public async Task SendMessage(string message)
        {
            try
            {
                var uri = $"https://api.telegram.org/bot{_options.Value.TelegramToken}/sendMessage";
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("chat_id", _options.Value.TelegramChatId.ToString()),
                    new KeyValuePair<string, string>("text", message)
                });

                var result = await _httpClient.PostAsync(uri, body);
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when sending Telegram message.");
            }
        }

    }
}
