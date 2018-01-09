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
        List<AzureDevices> list =new List<AzureDevices>();
        public MainPage()
        {
            this.InitializeComponent();
            
          /*  Task.Run(async () =>
            {
                while (true)
                {
                    var message = await AzureIoTHub.ReceiveCloudToDeviceMessageAsync();
                    Debug.WriteLine(message);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { textBlock.Text += message; });
                }
            });*/
            
        }

        /*private async void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Task.Run(async () => { await AzureIoTHub.SendDeviceToCloudMessageAsync(); });
            }

            // Retrieve the storage account from the connection string.
            var myAzureClass = new MyAzureClass();
            var table = myAzureClass.GetCloudTable();

            AzureDevices deviceStatus = new AzureDevices();
            deviceStatus.DeviceId = "DeviceCalvin";
            deviceStatus.Status = false;
            deviceStatus.AssignRowKey();
            deviceStatus.AssignPartitionKey();
            
            AzureDevices deviceRetrieve =await AzureDevices.RetrieveRecord(table, "DeviceCalvin");
            if (deviceRetrieve == null)
            {
                TableOperation tableOperation = TableOperation.Insert(deviceStatus);
                await table.ExecuteAsync(tableOperation);
                Debug.WriteLine("Record inserted");
            }
            else
            {
                Debug.WriteLine("Record exists");
            }
        }
        */
        private async void AddDeviceBtn_Click(object sender, RoutedEventArgs e)
        {
            MyAzureClass myAzureClass = new MyAzureClass();
            var AddedDevice = await myAzureClass.AddDeviceToCloud(AddDeviceBox.Text, StatusCheck.IsChecked);
            list.Add(AddedDevice);
            DeviceList.ItemsSource = null;
            DeviceList.ItemsSource = list;
        }

        private async void GetDevicesButton_Click(object sender, RoutedEventArgs e)
        {
            MyAzureClass myAzureClass = new MyAzureClass();
            list = await myAzureClass.GetDevices();
            foreach(AzureDevices device in list)
            {
                Debug.WriteLine("Device ID: " + device.DeviceId);           
            }
            DeviceList.ItemsSource = list;
        }

        private void DeleteDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            //int Index = DeviceList.SelectedIndex;
            AzureDevices SelectedDevice = (AzureDevices)DeviceList.SelectedItems[0];
            MyAzureClass myAzureClass = new MyAzureClass();            
            myAzureClass.DeleteRecordinTable(SelectedDevice.DeviceId);

            var index = DeviceList.Items.IndexOf(DeviceList.SelectedItem);
            list.RemoveAt(index);
            DeviceList.ItemsSource = null;
            DeviceList.ItemsSource = list;
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



