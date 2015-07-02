using System;
using SQLite;
namespace NoteTaking
{
	public class Note
	{
		public Note ()
		{
		}
		[PrimaryKey, AutoIncrement]
		public int ID { get; set;}

		public string Title { get; set;}

		public string Text { get; set;}

	}
}

