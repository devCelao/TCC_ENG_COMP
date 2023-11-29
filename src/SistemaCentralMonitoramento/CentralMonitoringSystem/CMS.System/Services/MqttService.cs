using CMS.System.Configuration;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace CMS.System.Services
{
    public class MqttService
    {
        private readonly MqttBroker mqttBroker;
        private ManagedMqttClientOptions optionsBuilder;
        IManagedMqttClient _mqttClient = new MqttFactory().CreateManagedMqttClient();
        public bool IsConnected => _mqttClient.IsConnected;

        public MqttService(IOptions<AppSettingsModel> settings)
        {
            this.mqttBroker = settings.Value.mqttBroker;
        }

        public void SetHandlers(Func<MqttApplicationMessageReceivedEventArgs, Task> messageReceived
            )
        {
            SetUpBroker();

            // Set up handlers
            _mqttClient.ConnectedAsync += _mqttClient_ConnectedAsync;
            _mqttClient.DisconnectedAsync += _mqttClient_DisconnectedAsync;
            _mqttClient.ConnectingFailedAsync += _mqttClient_ConnectingFailedAsync;
            _mqttClient.ApplicationMessageReceivedAsync += messageReceived;
        }

        private void SetUpBroker()
        {
            optionsBuilder = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithTcpServer(mqttBroker.Server, mqttBroker.Port) // Replace with your MQTT server address and port
                .WithClientId(mqttBroker.ClientId) // Replace with a client identifier
                .WithCredentials(mqttBroker.Username, mqttBroker.Password)
                .WithWillTopic($"channels/{mqttBroker.ChannelId}/publish/fields/")
                .Build())
            .Build();
        }

        public void PublishMessage(string message)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttBroker.Server, mqttBroker.Port) // Replace with your MQTT server address and port
                .WithClientId(mqttBroker.ClientId) // Replace with a client identifier
                .WithCredentials(mqttBroker.Username, mqttBroker.Password)
                .WithWillTopic($"channels/{mqttBroker.ChannelId}/publish/fields/")
                .Build();

                mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"channels/{mqttBroker.ChannelId}/publish/fields/field1")
                .WithPayload(message)
                .Build();

                var resposta = mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                mqttClient.DisconnectAsync();

                Console.WriteLine("MQTT application message is published.");
            }
        }


        public void TryConnect()
        {
            if (IsConnected) return;

            _mqttClient.StartAsync(optionsBuilder);
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }

        private async Task _mqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            await _mqttClient.SubscribeAsync($"channels/{mqttBroker.ChannelId}/subscribe");
            //throw new NotImplementedException();
        }

        Task _mqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            TryConnect();
            return Task.CompletedTask;
        }
        Task _mqttClient_ConnectingFailedAsync(ConnectingFailedEventArgs arg)
        {
            TryConnect();
            return Task.CompletedTask;
        }
    }
}
