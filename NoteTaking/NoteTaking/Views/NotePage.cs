using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NoteTaking
{
	public class NotePage : ContentPage
	{
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
				App.Database.DeleteItem(note.ID);
				Navigation.PopAsync();
			};

			var cancelButton = new Button { Text = "Cancel" };
			cancelButton.Clicked += (sender, e) => 
			{
				var note = (Note)BindingContext;
				Navigation.PopAsync();
			};

			Content = new StackLayout 
			{
				VerticalOptions= LayoutOptions.StartAndExpand,
				Padding= new Thickness (20), 
				Children= {nameLabel, nameEntry, notesLabel, notesEntry, saveButton, deleteButton, cancelButton
				}
			};
		}
	}
}

