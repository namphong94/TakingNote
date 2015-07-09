using System;
using SQLite;
using Xamarin.Forms;


namespace NoteTaking
{
	public class Note
	{
		public Note ()
		{
		}
		[PrimaryKey, AutoIncrement]
		public int NoteID { get; set;}

		public string Title { get; set;}

		public string Text { get; set;}

		//public ImageSource Image { set; get; }

		//public string FileUri { set; get; }
	}
}

