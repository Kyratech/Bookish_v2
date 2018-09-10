using System;

namespace Bookish_V2.DataAccessFmwk
{
	public class BorrowDetails
	{
		public string Email { get; set; }
		public int ItemId { get; set; }
		public int BookId { get; set; }
		public DateTime DueDate { get; set; }
	}
}
