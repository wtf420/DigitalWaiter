using Camera.MAUI;
using System.Diagnostics;
using System.Net;

namespace MauiApp1.Pages;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        ServiceHelper.ConnectionURL = "http://localhost:8080/";
        ServiceHelper.GetService<OrderService>().StartUpdating();
        await ServiceHelper.GetService<FoodService>().RefreshDataAsync();
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        camView.BarCodeOptions = new()
        {
            PossibleFormats = { ZXing.BarcodeFormat.QR_CODE }
        };
        camView.Camera = camView.Cameras.FirstOrDefault();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await camView.StopCameraAsync();
            await camView.StartCameraAsync();
        });
    }

    private void camView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await camView.StopCameraAsync();
            string[] a = ($"{args.Result[0]}").Split(" ");

            foreach (string s in a)
            {
                var str = "http://" + s;
                Uri uriResult;
                if (Uri.TryCreate(str, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    ServiceHelper.ConnectionURL = str + ":8080/";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceHelper.ConnectionURL);
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        break;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                } else
                { continue; }
            }
            ServiceHelper.GetService<OrderService>().StartUpdating();
            await ServiceHelper.GetService<FoodService>().RefreshDataAsync();
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        });
    }
}

