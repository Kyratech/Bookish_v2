using System;

namespace Bookish_V2.DataAccessFmwk
{
	public class UserBookDetails
	{
		public string Title { get; set; }
		public int BookId { get; set; }
		public DateTime DueDate { get; set; }
		public int ItemId { get; set; }
	}
}
