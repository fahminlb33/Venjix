namespace Venjix.Infrastructure.DTO
{
    public class VenjixOptions
    {
        public const string SectionName = "Venjix";

        public string TelegramToken { get; set; }
        public int TelegramChatId { get; set; }
        public bool IsTelegramTokenValid { get; set; }

        public int TriggerCooldown { get; set; }
    }
}