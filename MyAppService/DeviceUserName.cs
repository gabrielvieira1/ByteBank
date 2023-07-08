﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace MyAppService
{
  public sealed class DeviceUserName : IBackgroundTask
  {
    private BackgroundTaskDeferral backgroundTaskDeferral;
    private AppServiceConnection appServiceconnection;

    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(
      IntPtr hWnd,
      [param: MarshalAs(UnmanagedType.LPWStr)] string lpText,
      [param: MarshalAs(UnmanagedType.LPWStr)] string lpCaption,
      UInt32 utype);


    [DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
    private static extern bool GetUserNameW(
      [param: MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpBuffer,
      ref UInt32 pcbBuffer);

    public void Run(IBackgroundTaskInstance taskInstance)
    {
      // Get a deferral so that the service isn't terminated.
      this.backgroundTaskDeferral = taskInstance.GetDeferral();

      // Associate a cancellation handler with the background task.
      taskInstance.Canceled += OnTaskCanceled;

      // Retrieve the app service connection and set up a listener for incoming app service requests.
      var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
      appServiceconnection = details.AppServiceConnection;
      appServiceconnection.RequestReceived += OnRequestReceived;
    }

    private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {
      var messageDeferral = args.GetDeferral();

      ValueSet message = args.Request.Message;
      ValueSet returnData = new ValueSet();

      //int result = MessageBoxW(IntPtr.Zero, "Hello World", "This is window title", 0);

      StringBuilder sb = new StringBuilder(100);
      UInt32 size = 100;

      GetUserNameW(sb, ref size);

      returnData.Add("Result", sb.ToString());
      returnData.Add("Status", "OK");

      await args.Request.SendResponseAsync(returnData);

      messageDeferral.Complete();
    }

    // Correção

    //private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    //{
    //  var messageDeferral = args.GetDeferral();

    //  ValueSet message = args.Request.Message;
    //  ValueSet returnData = new ValueSet();

    //  StringBuilder sb = new StringBuilder(100);
    //  UInt32 size = 100;

    //  try
    //  {

    //    GetUserNameW(sb, ref size);

    //    returnData.Add("Result", sb.ToString());
    //    returnData.Add("Status", "OK");
    //  }
    //  catch (Exception ex)
    //  {
    //    returnData.Add("Error", ex.Message);
    //    returnData.Add("Status", "Error");
    //  }
    //  finally
    //  {
    //    // Liberar explicitamente o StringBuilder
    //    sb.Clear();
    //    sb = null;

    //    messageDeferral.Complete();
    //  }

    //  await args.Request.SendResponseAsync(returnData);
    //}


    private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
      if (this.backgroundTaskDeferral != null)
      {
        // Complete the service deferral.
        this.backgroundTaskDeferral.Complete();
      }
    }
  }
}
