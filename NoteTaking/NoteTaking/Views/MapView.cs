using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Geolocation;

namespace NoteTaking
{
	public class MapView : ContentPage
	{
		PhotoListData pld;
		Geolocator locator;
		Xamarin.Geolocation.Position position = new Xamarin.Geolocation.Position();
		MyLocator location;
		public interface ICurrentLocation
		{
			MyLocator SetCurrentLocation ();
		}

		public MyLocator SetCurrentLocation()
		{
			GetPosition ();
			location = new MyLocator ()
			{ 
				Latitude = position.Latitude,
				Longitude = position.Longitude
			};
			return location;
		}
		async void GetPosition()
		{
			try 
			{
				locator = new Geolocator{DesiredAccuracy =50};
				if ( locator.IsListening != true )
					locator.StartListening(minTime: 1000, minDistance: 0);
				position = await locator.GetPositionAsync (timeout: 20000);
			}

			catch ( Exception e) 
			{

			}
		}
		public MapView (){
			var map = new Map (
				         MapSpan.FromCenterAndRadius (
					         new Xamarin.Forms.Maps.Position (location.Latitude, location.Longitude), 
					         Distance.FromMiles (0.3))) {
				IsShowingUser = true,
				HeightRequest = 100, 
				WidthRequest = 960, 
				VerticalOptions = LayoutOptions.FillAndExpand 
			};
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = new Thickness (20), 
				Children = { map }
			};
		}
	}
}

