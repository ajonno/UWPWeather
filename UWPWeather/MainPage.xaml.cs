using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json.Linq;
using PubNubMessaging.Core;
using Parse;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPWeather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public readonly Pubnub _pubnub = new Pubnub("pub-c-f0da0918-8663-43a8-8e4b-2202c96931b6", "sub-c-8dc31cfa-30a0-11e3-a365-02ee2ddab7fe");

		public MainPage()
        {
			this.InitializeComponent();

			// Subscribe to the demo_tutorial channel
			_pubnub.Subscribe<string>(
			   "awards",
			   DisplaySubscribeReturnMessage,
			   DisplaySubscribeConnectStatusMessage,
			   DisplayErrorMessage);


		}

		private void DisplayErrorMessage(PubnubClientError obj)
		{
			Debug.WriteLine($"DisplayErrorMessage {obj}");
		}

		void DisplaySubscribeReturnMessage(string result)
		{
			Debug.WriteLine("SUBSCRIBE REGULAR CALLBACK:");
			Debug.WriteLine(result);
			JArray adata = JArray.Parse(result);
			Debug.WriteLine(adata.First);

		}

		private void DisplaySubscribeConnectStatusMessage(string obj)
		{
			Debug.WriteLine("SUBSCRIBE CONNECT STATUS CALLBACK:");
			Debug.WriteLine(obj);
		}


		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			var position = await LocationManager.GetPositionAsync();

			ResultTextBlock.Text = string.Empty;
			var myWeather = await OpenWeatherMapProxy.GetWeather(position.Coordinate.Latitude, position.Coordinate.Longitude);
			string icon = string.Format("ms-appx:///Assets/{0}.png", myWeather.weather[0].icon);
			ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
			ResultTextBlock.Text = myWeather.name + " - " + (int)myWeather.main.temp + " - " + myWeather.weather[0].description;

			IDictionary<string, object> dict = new Dictionary<string, object>() { { "category", "Best Picture" } };

			ParseClient.Initialize("mCf0nyFhZibuMu2LAXXOOmE1Cuz5vPz7PN82HSRJ", "rRqpccXYcAaL81bUUzl21cE3HDhECZZ5uC4Y1OZK");
			var cloudFunctionTask = await ParseCloud.CallFunctionAsync<string>("averageStars", dict);
			Debug.WriteLine(cloudFunctionTask);


		}
	}
}
