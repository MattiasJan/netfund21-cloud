using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace IotDevice_Console
{
    public class Program
    {
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=netfund21-iothub.azure-devices.net;DeviceId=ConsoleDevice;SharedAccessKey=Xsd5hFddWQZncFrKs3+sc5ooInFQHeWQPjXioNXR1Cg=");
        private static readonly Random rnd = new Random();

        public static async Task Main(string[] args)
        {
            await SendMessageAsync();
        }


        private static async Task SendMessageAsync()
        {
            while (true)
            {
                var data = new
                {
                    Temperature = rnd.Next(10, 20),
                    Humidity = rnd.Next(30, 40)
                };

                var json = JsonConvert.SerializeObject(data);
                var message = new Message(Encoding.UTF8.GetBytes(json));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine($"Message sent to Azure IotHub ({json})");
                await Task.Delay(10 * 1000);
            }
        }
    }
}
