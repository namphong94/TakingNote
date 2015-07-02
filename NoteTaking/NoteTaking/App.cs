using System;
using Xamarin.Forms;

namespace NoteTaking
{
	public class App : Application
	{
		static NoteDatabase database;

		public App ()
		{
			database = new NoteDatabase ();
			var mainNav = new NavigationPage (new NoteListPage ());
			MainPage = mainNav;
		}

		public static NoteDatabase Database
		{
			get{ return database; }
		}
	
	}
}

