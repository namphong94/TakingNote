using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Reflection;
using Camera_Test.Media;
using Xamarin.Forms.Maps;
using Xamarin.Geolocation;

namespace NoteTaking
{
	public class NotePage : ContentPage
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
		public NotePage ()
		{
			this.SetBinding (ContentPage.TitleProperty, "Title");

			NavigationPage.SetHasNavigationBar (this, true);

			var nameLabel = new Label { Text="Title" };
			var nameEntry = new Entry ();
			nameEntry.SetBinding (Entry.TextProperty,"Title");


			var notesLabel = new Label {Text = "Text" };
			var notesEntry = new Entry ();
			notesEntry.SetBinding (Entry.TextProperty,"Text");


			MyLocator location = DependencyService.Get<ICurrentLocation> ().SetCurrentLocation ();

			var locationLabel = new Label { Text="Location" };
			var map = new Map ( 
				MapSpan.FromCenterAndRadius( 
					new Xamarin.Forms.Maps.Position (location.Latitude,location.Longitude), 
					Distance.FromMiles(0.3))){
				IsShowingUser = true,
				HeightRequest = 100, 
				WidthRequest = 960, 
				VerticalOptions = LayoutOptions.FillAndExpand 
			};






			var saveButton = new Button {Text = "Save" };
			saveButton.Clicked += (sender, e) =>
			{
				var note = (Note)BindingContext;
				App.Database.SaveItem(note);
				Navigation.PopAsync();
			};

			var deleteButton = new Button {Text = "Delete" };
			deleteButton.Clicked += (sender, e) => 
			{
				var note = (Note)BindingContext;
				App.Database.DeleteItem(note.NoteID);
				Navigation.PopAsync();
			};

			var cancelButton = new Button { Text = "Cancel" };
			cancelButton.Clicked += (sender, e) => 
			{
				var note = (Note)BindingContext;
				Navigation.PopAsync();
			};


			var Photo  = new Button {Text = "Photos" };
			Photo.Clicked += photo_clicked;

			Content = new StackLayout 
			{
				VerticalOptions= LayoutOptions.StartAndExpand,
				Padding= new Thickness (20), 
				Children= {nameLabel, nameEntry, notesLabel, notesEntry, locationLabel, map, saveButton, deleteButton,Photo,cancelButton
				}
			};
		}
		async void photo_clicked(object sender, EventArgs e)
		{
			await doPhotoAction (pld);
		}

		async private Task doPhotoAction(PhotoListData pld)
		{
#if __ANDROID__
			var action = await DisplayActionSheet("Select Source","Cancel",null,"Camera","Photo Library");
			if (action == "Camera")
			{
				doCameraPhoto(pld);
			}
			else if (action == "Photo Library")
			{
				doPhotoLibrary(pld);
			}
#else

			var action = await DisplayActionSheet("Select Source", "Cancel", null, "Camera", "Photo Library");
			if (action == "Camera")
			{
			doCameraPhoto(pld);
			}
			else if (action == "Photo Library")
			{
			doPhotoLibrary(pld);
			}
			//doPhotoLibrary(pld);
#endif
		}

		async void doCameraPhoto(PhotoListData pld)
		{
			#if __ANDROID__
			MediaPicker picker = new MediaPicker(Forms.Context);
			#else
			MediaPicker picker = new MediaPicker();
			#endif

			if (picker.IsCameraAvailable == false)
			{
				var page = new ContentPage();
				var result = page.DisplayAlert("Warning", "Camera is not available", "OK");

				return;
			}
			else
			{
				try
				{
					var resultfile = await picker.TakePhoto(null);
					#if __ANDROID__
					showDrawingView(pld);
					#else
					showDrawingView(pld);
					#endif
				}
				catch (Exception ex)
				{
				}
			}
		}

		async void showDrawingView(PhotoListData pld)
		{
			/*var pv = new DrawingPhotoView();
			pv.PhotoList = pld;
			await Navigation.PushAsync(pv);
			pld = pv.PhotoList;*/
		}

		async void doPhotoLibrary(PhotoListData pld)
		{
			#if __ANDROID__
			MediaPicker picker = new MediaPicker(Forms.Context);
			#else
			MediaPicker picker = new MediaPicker();
			#endif

			if (picker.IsPhotoGalleryAvailable == false)
			{
				var page = new ContentPage();
				var result = page.DisplayAlert("Warning", "Photo is not available", "OK");

				return;
			}
			else
			{
				try
				{
					var resultfile = await picker.PickPhoto();
					#if __ANDROID__
					//showDrawingView(pld);
					#else
					//showDrawingView(pld);
					#endif
				}
				catch (Exception e)
				{
				}
			}
		}

	}
		
	public class PhotoListData : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		List<string> _photoList;
		int _count;
		string _countItem;
		string _firstPhotoPath;

		// Init Function
		public PhotoListData()
		{
			_photoList = new List<string>();
			_count = 0;
			_countItem = "ADD PHOTO";
			_firstPhotoPath = "takePhoto_icon.png";
		}

		public void AddPhotoItem(string photoPath, bool _comesFromSubmit)
		{
			if (_count == 0)
				FirstPhotoPath = photoPath;

			_photoList.Add(photoPath);
			_count = _photoList.Count;
			CountItemString = _comesFromSubmit ? String.Format("ADD MORE PHOTOS ({0} added)", _count) : String.Format("VIEW PHOTOS ({0})", _count);
		}

		// Data Fields that Change
		public string FirstPhotoPath
		{
			set
			{
				_firstPhotoPath = value;

				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("FirstPhotoPath"));
				}
			}

			get
			{
				return _firstPhotoPath;
			}
		}

		public string CountItemString
		{
			set
			{
				_countItem = value;

				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("CountItemString"));
				}
			}

			get
			{
				return _countItem;
			}
		}

		public List<string> PhotoList
		{
			set
			{
				_photoList = value;

				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("PhotoList"));
				}
			}

			get
			{
				return _photoList;
			}
		}
	}

}

