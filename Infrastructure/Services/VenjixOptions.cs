namespace Venjix.Infrastructure.Services
{
    public class VenjixOptions
    {
        public bool IsTelegramTokenValid { get; set; }
        public string TelegramToken { get; set; }
        public string TelegramBotCallName { get; set; }
        public string TelegramBotUsername { get; set; }
        public int TelegramChatId { get; set; }
        public int TriggerCooldown { get; set; }
    }
}