namespace CMS.System.Configuration
{
    public class MqttBroker
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ChannelId { get; set; }
    }
}
