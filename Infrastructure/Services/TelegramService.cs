using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                var chatId = await GetChatId(token);
                var (callName, username) = await GetBotName(token);
                _options.Update(options =>
                {
                    options.IsTelegramTokenValid = true;
                    options.TelegramToken = token;
                    options.TelegramChatId = chatId;
                    options.TelegramBotCallName = callName;
                    options.TelegramBotUsername = username;
                });
                _logger.LogInformation("Telegram Bot is activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when verifying Telegram token.");
                _options.Update(options =>
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

        private async Task<int> GetChatId(string token)
        {
            var uri = $"https://api.telegram.org/bot{token}/getUpdates?limit=5";
            var result = await _httpClient.GetAsync(uri);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsStringAsync();
            var body = JToken.Parse(response);
            var message = body["result"].FirstOrDefault(x => x["message"]["text"].Value<string>().Contains(VerifyMessage));
            return message["message"]["chat"]["id"].Value<int>();
        }

        private async Task<(string callName, string username)> GetBotName(string token)
        {
            var uri = $"https://api.telegram.org/bot{token}/getMe";
            var result = await _httpClient.GetAsync(uri);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsStringAsync();
            var body = JToken.Parse(response);
            var user = body["result"];
            return (user["first_name"].Value<string>(), user["username"].Value<string>());
        }
    }
}
