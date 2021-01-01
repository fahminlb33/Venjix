﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
        private readonly IVenjixOptionsService _optionsService;
        private static HttpClient _httpClient = new HttpClient();

        private const string VerifyMessage = "VENJIX";

        public TelegramService(ILogger<TelegramService> logger, IVenjixOptionsService optionsService)
        {
            _logger = logger;
            _optionsService = optionsService;
        }

        public async Task VerifyAndSaveBot(string token)
        {
            try
            {
                var chatId = await GetChatId(token);
                var (callName, username) = await GetBotName(token);

                _optionsService.Options.IsTelegramTokenValid = true;
                _optionsService.Options.TelegramToken = token;
                _optionsService.Options.TelegramChatId = chatId;
                _optionsService.Options.TelegramBotCallName = callName;
                _optionsService.Options.TelegramBotUsername = username;
                await _optionsService.Save();

                _logger.LogInformation("Telegram Bot is activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when verifying Telegram token.");
                _optionsService.Options.IsTelegramTokenValid = false;
                _optionsService.Options.TelegramToken = token;
                _optionsService.Options.TelegramChatId = 0;
                await _optionsService.Save();
            }
        }

        public async Task SendMessage(string message)
        {
            try
            {
                var uri = $"https://api.telegram.org/bot{_optionsService.Options.TelegramToken}/sendMessage";
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("chat_id", _optionsService.Options.TelegramChatId.ToString()),
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
            var id = message?["message"]?["chat"]?["id"]?.Value<int>();

            if (!id.HasValue)
            {
                throw new Exception("No recent chat to bot.");
            }

            return id.Value;
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
