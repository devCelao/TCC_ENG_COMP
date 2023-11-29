using CMS.System.Data.Models;
using CMS.System.Data.Repositories;
using CMS.System.Services.Handlers.Models;
using MQTTnet.Client;
using Newtonsoft.Json;
using System.Text;

namespace CMS.System.Services.Handlers
{
    public class MqttIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MqttService _broker;
        public MqttIntegrationHandler(IServiceProvider serviceProvider
                                    , MqttService broker
            )
        {
            _serviceProvider = serviceProvider;
            _broker = broker;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _broker.SetHandlers(_mqttClient_MessageReceived);
            _broker.TryConnect();
            return Task.CompletedTask;
        }

        private async Task AddMedicao(Device device)
        {
            using (var scope = _serviceProvider.CreateScope())
            {

                var repository = scope.ServiceProvider.GetRequiredService<IDispositivoRepository>();

                var dispositivoCadastrado = await repository.GetOnlyOneDevice(device.device_id);

                if (dispositivoCadastrado.ID_DEVICE is null)
                {
                    dispositivoCadastrado = await AddDispositivo(device.device_id);
                }

                var medicao = new MedicaoMQTT
                {
                    ID_DEVICE = device.device_id,
                    DT_PUBLICACAO = DateTime.Now,
                    VAL_HMD_AMB = float.Parse(device.air_humidity.Replace(".",",")),
                    VAL_TMP_AMB = float.Parse(device.air_temperature.Replace(".", ",")),
                    VAL_HMD_SOL = float.Parse(device.soil_humidity.Replace(".", ","))
                };

                await repository.AddMedicao(device.device_id, new DispositivoMedicao(medicao));
            }
        }

        private async Task<Dispositivo> GetDispositivosById(string device_id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IDispositivoRepository>();

                return await repository.GetOnlyOneDevice(device_id);
            }
        }

        private async Task<Dispositivo> AddDispositivo(string ID_DEVICE)
        {
            var dispositivo = new Dispositivo 
            {
                ID_DEVICE = ID_DEVICE,
                DT_CADASTRO = DateTime.Now,
                DT_ALTERACAO = DateTime.Now,
                FREQUENCY = 60,
                ID_GROUP = 0,
                IND_ATIVO = 1,
                IND_ATIVO_HMD_AMB = 1,
                IND_ATIVO_HMD_SOL = 1,
                IND_ATIVO_TMP_AMB = 1,
                COD_USUARIO_ALTERACAO = "Inicializado pelo Sistema"
            };

            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IDispositivoRepository>();

                await repository.AddDispositivo(dispositivo);

            }

            return dispositivo;
        }

        public async Task _mqttClient_MessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var content = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var device = converteClasse(content);
            string payload = String.Empty;

            if (device != null)
            {
                // Sync Profile
                if (device.command == "104")
                {
                    var dispositivoCadastrado = await GetDispositivosById(device.device_id);

                    if(dispositivoCadastrado.ID_DEVICE is null)
                    {
                        dispositivoCadastrado = await AddDispositivo(device.device_id);
                    }

                    payload = $"{404}{device.device_id}.{dispositivoCadastrado.FREQUENCY}";
                    _broker.PublishMessage(payload);
                    Thread.Sleep(15000);


                    payload = $"{407}{device.device_id}.{dispositivoCadastrado.ID_GROUP}";
                    _broker.PublishMessage(payload);
                    Thread.Sleep(15000);


                    payload = $"{410}{device.device_id}.{dispositivoCadastrado.IND_ATIVO_HMD_SOL}";
                    _broker.PublishMessage(payload);
                    Thread.Sleep(15000);


                    payload = $"{413}{device.device_id}.{dispositivoCadastrado.IND_ATIVO_TMP_AMB}";
                    _broker.PublishMessage(payload);
                    Thread.Sleep(15000);


                    payload = $"{416}{device.device_id}.{dispositivoCadastrado.IND_ATIVO_HMD_AMB}";
                    _broker.PublishMessage(payload);
                    Thread.Sleep(15000);
                }

                if (device.command == "201")
                {
                    await AddMedicao(device);
                }
            }

            Console.WriteLine($"Mensagem recebida no tópico '{e.ApplicationMessage.Topic}': {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        }

        private Device converteClasse(string? content)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<MqttReceiveMessage>(content);
                return new Device().converteInDevice(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao desserializar JSON: " + ex.Message);
                return null;
            }
        }
    }
}
