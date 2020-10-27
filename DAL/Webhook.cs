namespace Venjix.DAL
{
    public class Webhook
    {
        public int WebhookId { get; set; }
        public string Name { get; set; }
        public string UriFormat { get; set; }
        public string BodyFormat { get; set; }
        public bool JsonBody { get; set; }
        public string Method { get; set; }
    }
}
