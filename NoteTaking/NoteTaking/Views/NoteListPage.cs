using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTaking
{
	public class NoteListPage : ContentPage
	{

		ListView listView;

		public NoteListPage ()
		{
			Title = "Note";

			NavigationPage.SetHasNavigationBar (this, true);

			listView = new ListView {
				RowHeight = 40,
				ItemTemplate = new DataTemplate (typeof(NoteCell))
			};

			listView.ItemSelected += (sender, e) => {
				var note = (Note)e.SelectedItem;
				var notePage = new NotePage();
				Navigation.PushAsync(notePage);
			};

			var layout = new StackLayout ();
			layout.Children.Add (listView);
			layout.VerticalOptions = LayoutOptions.FillAndExpand;
			Content = layout;

			ToolbarItem tbi = null;

			if (Device.OS == TargetPlatform.iOS) {
				tbi = new ToolbarItem ("+", null, () => {
					var note = new Note ();
					var notePage = new NotePage ();
					notePage.BindingContext = note;
					Navigation.PushAsync(notePage);
				}, 0, 0);
			}

			if (Device.OS == TargetPlatform.Android) {
				tbi = new ToolbarItem ("+", "plus", () => {
					var note = new Note ();
					var notePage = new NotePage ();
					notePage.BindingContext = note;
					Navigation.PushAsync (notePage);
				}, 0, 0);
			}

			ToolbarItems.Add (tbi);

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			listView.ItemsSource = App.Database.GetItems ();
		}

	}
}

