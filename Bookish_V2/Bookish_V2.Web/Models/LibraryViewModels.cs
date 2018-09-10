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
}