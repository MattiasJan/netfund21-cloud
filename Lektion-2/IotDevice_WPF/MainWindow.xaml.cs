using Dapper;
using IotDevice_WPF.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IotDevice_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeviceModel deviceModel;
        private Random rnd;
        private DeviceClient deviceClient;

        private string hostName = "netfund21-iothub.azure-devices.net";
        private string apiUrl = "https://netfund21-functionapp.azurewebsites.net/api/devices";
        private string sql = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HansMattin-Lassei\Documents\Utbildning\NETFUND21\netfund21-cloud\Lektion-2\IotDevice_WPF\Data\sql_db.mdf;Integrated Security=True;Connect Timeout=30";
        private bool isConfigured = false;

        public MainWindow()
        {
            InitializeComponent();

            InitializeDevice();

            if (!isConfigured)
            {
                btnConnect.IsEnabled = true;
                btnConnect.Content = "Connect";
            }
            else
            {
                ConnectAndSendAsync().GetAwaiter();
            }

        }

        // STEP 1.
        private void InitializeDevice()
        {
            rnd = new();
            deviceModel = new();

            using (IDbConnection conn = new SqlConnection(sql))
            {
                var data = conn.Query<DeviceModel>("SELECT * FROM Device");

                if(data.Any())
                {
                    foreach(var device in data)
                    {
                        deviceModel.Id = device.Id;
                        deviceModel.SensorType = device.SensorType;
                        deviceModel.SharedAccessKey = device.SharedAccessKey;             
                    }

                    isConfigured = true;
                }

            }
        }


        // STEP 2.
        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            using (IDbConnection conn = new SqlConnection(sql))
            {
                deviceModel.Id = Guid.NewGuid().ToString();
                deviceModel.SensorType = "Simulated Temperature Sensor";

                using (var http = new HttpClient())
                {
                    var result = await http.PostAsJsonAsync(apiUrl, deviceModel);
                    deviceModel.SharedAccessKey = await result.Content.ReadAsStringAsync();
                }

                await conn.ExecuteAsync("INSERT INTO Device VALUES (@Id, @SensorType, @SharedAccessKey)", deviceModel);
            }

            await ConnectAndSendAsync();
        }


        // STEP 3.
        private async Task ConnectAndSendAsync()
        {
            tblockDeviceId.Text = deviceModel.Id;
            btnConnect.IsEnabled = false;
            btnConnect.Content = "Connected";

            deviceClient = DeviceClient.CreateFromConnectionString($"HostName={hostName};DeviceId={deviceModel.Id};SharedAccessKey={deviceModel.SharedAccessKey}");
            await UpdateSensorTypeAsync(deviceModel.SensorType);
            await SendMessageAsync();
        }

        private async Task UpdateSensorTypeAsync(string sensorType)
        {
            TwinCollection reportedProperty = new TwinCollection();
            reportedProperty["sensorType"] = sensorType;

            await deviceClient.UpdateReportedPropertiesAsync(reportedProperty);
        }


        private async Task SendMessageAsync()
        {
            while (true)
            {
                var message = new DeviceMessageModel
                {
                    Temperature = rnd.Next(20, 30),
                    Humidity = rnd.Next(30, 40)
                };

                await deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));
                tblockInfo.Text = $"Message sent {DateTime.Now}";

                await Task.Delay(10 * 1000);
            }
        }

    }
}
