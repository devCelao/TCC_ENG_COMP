namespace CMS.System.Services.Handlers.Models
{
    public class MqttReceiveMessage
    {
        public string channel_id { get; set; }
        public string created_at { get; set; }
        public string entry_id { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string elevation { get; set; }
        public string status { get; set; }
    }

    public class Device
    {
        public string channel_id { get; set; }
        public string created_at { get; set; }
        public string entry_id { get; set; }
        public string command { get; set; }
        public string device_id { get; set; }
        public string group { get; set; }
        public string frequency { get; set; }
        public string air_humidity { get; set; }
        public string air_temperature { get; set; }
        public string soil_humidity { get; set; }
        public string field8_empt { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string elevation { get; set; }
        public string status { get; set; }

        public Device converteInDevice(MqttReceiveMessage msg)
        {
            return
                new Device
                {
                    channel_id = msg.channel_id,
                    created_at = msg.created_at,
                    entry_id = msg.entry_id,
                    command = msg.field1,
                    device_id = msg.field2,
                    group = msg.field3,
                    frequency = msg.field4,
                    air_humidity = msg.field5,
                    air_temperature = msg.field6,
                    soil_humidity = msg.field7,
                    field8_empt = msg.field8,
                    latitude = msg.latitude,
                    longitude = msg.longitude,
                    elevation = msg.elevation,
                    status = msg.status,
                };
        }
    }
}
