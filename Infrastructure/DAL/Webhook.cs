using System.ComponentModel.DataAnnotations;

namespace Venjix.Infrastructure.DAL
{
    public class Webhook
    {
        [Key]
        public int WebhookId { get; set; }
        public string Name { get; set; }
        public string UriFormat { get; set; }
        public string BodyFormat { get; set; }
        public bool JsonBody { get; set; }
        public string Method { get; set; }
    }
}
