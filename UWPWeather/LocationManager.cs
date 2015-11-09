using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace UWPWeather
{
	public class LocationManager
	{
		public static async Task<Geoposition> GetPositionAsync()
		{
			var accessStatus = await Geolocator.RequestAccessAsync();

			if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();

			var geoLocator = new Geolocator { DesiredAccuracy = 0 };

			var position = await geoLocator.GetGeopositionAsync();

			return position;
		}
	}
}
