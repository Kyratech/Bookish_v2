using System;
using System.Collections.Generic;
using Bookish_V2.DataAccessFmwk;

namespace Bookish_V2.Web.Models
{
	public class LibraryCatalogueViewModel
	{
		public SortedDictionary<Book, int> inventory { get; set; }

		public string SearchTerm { get; set; }
	}

	public class UserCatalogueViewModel
	{
		public List<UserBookDetails> MyBooks { get; set; }
	}

	public class LibraryItemViewModel
	{
		public Book Book { get; set; }
		public int TotalCopies { get; set; }
		public int AvailableCopies { get; set; }
		public List<BorrowDetails> BorrowedItemDetails { get; set; }
	}
}