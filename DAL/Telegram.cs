using System.ComponentModel.DataAnnotations;

namespace Venjix.DAL
{
    public class Telegram
    {
        [Key]
        public int TelegramId { get; set; }
        public string MessageFormat { get; set; }
    }
}
