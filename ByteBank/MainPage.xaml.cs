using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ByteBank
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private AppServiceConnection deviceService;

    public MainPage()
    {
      this.InitializeComponent();
    }

    private async void btnClick_Click(object sender, RoutedEventArgs e)
    {
      // Add the connection.
      if (this.deviceService == null)
      {
        this.deviceService = new AppServiceConnection();

        // Here, we use the app service name defined in the app service 
        // provider's Package.appxmanifest file in the <Extension> section.
        this.deviceService.AppServiceName = "com.microsoft.deviceInfo";

        // Use Windows.ApplicationModel.Package.Current.Id.FamilyName 
        // within the app service provider to get this value.
        this.deviceService.PackageFamilyName = "1e0301ff-f819-4eb6-a97f-8bda5de6c1cb_03w1eqkrn0fpt"; // Package.Current.Id.FamilyName;

        var status = await this.deviceService.OpenAsync();

        if (status != AppServiceConnectionStatus.Success)
        {
          textBox.Text = "Failed to connect";
          this.deviceService = null;
          return;
        }
      }

      // Call the service.
      int idx = int.Parse(textBox.Text);
      //string folderPath = textBox.Text;
      var message = new ValueSet();
      message.Add("Command", "Item");
      message.Add("ID", idx);
      //message.Add("FolderPath", folderPath);
      AppServiceResponse response = await this.deviceService.SendMessageAsync(message);
      string result = "";

      if (response.Status == AppServiceResponseStatus.Success)
      {
        ////Get the data that the service sent to us.
        if (response.Message["Status"] as string == "OK")
        {
          result = response.Message["Result"] as string;
        }
        //await Launcher.LaunchUriAsync(new Uri("calculator://"));
      }

      textBox.Text = result;
    }
  }
}
