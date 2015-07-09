using System;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace NoteTaking
{
	public class NoteDatabase
	{
		static object locker = new object();

		SQLiteConnection database;

		string DatabasePath 
		{
			get 
			{ 
				var sqliteFilename= "NoteSQLite.db3";
				#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string libraryPath = Path.Combine (documentsPath, "..", "Library");
				var path = Path.Combine(libraryPath, sqliteFilename);
				#else
				#if __ANDROID__
				string documentsPath= Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var path= Path.Combine(documentsPath, sqliteFilename);
				#endif
				#endif
				return path;
			}
		}

		public IEnumerable<Note> GetItems()
		{
			lock (locker)
			{
				return (from i in database.Table<Note>() select i).ToList();
			}
		}

		public Note GetItem (int id)
		{
			lock (locker) 
			{
				return database.Table<Note> ().FirstOrDefault (x=> x.NoteID == id);
			}
		}

		public int SaveItem (Note item)
		{
			lock (locker)
			{
				if (item.NoteID != 0)
				{
					database.Update (item);
					return item.NoteID;
				} 
				else
					return database.Insert (item);
			}
		}

		public int DeleteItem(int id)
		{
			lock (locker) 
			{
				return database.Delete<Note>(id);
			}
		}

		public NoteDatabase ()
		{
			database = new SQLiteConnection (DatabasePath);
			database.CreateTable<Note>();
		}


	}

}