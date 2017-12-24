using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AnotherFuckingTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await AzureIoTHub.ReceiveCloudToDeviceMessageAsync();
                    Debug.WriteLine(message);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { textBlock.Text += message; });
                }
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Task.Run(async () => { await AzureIoTHub.SendDeviceToCloudMessageAsync(); });
            }

            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=group8;AccountKey=FTPP2o14jNuGOI+YizAdfeWNQWXA4ult7M4ngYx6k8R0Hsxj/EeE1uASuQancc2dvvJksI7uf/jpx8QWgipu6Q==;EndpointSuffix=core.windows.net");
                        DeviceStatus deviceStatus = new DeviceStatus("DeviceCalvin", "0" );
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Test");

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(deviceStatus);

            // Execute the insert operation.
           await table.ExecuteAsync(insertOperation);
        }
    }
}
public class DeviceStatus : TableEntity
{
    public DeviceStatus(string device, string status)
    {
        this.PartitionKey = device;
        this.RowKey = status;
    }
    public DeviceStatus() { }
}