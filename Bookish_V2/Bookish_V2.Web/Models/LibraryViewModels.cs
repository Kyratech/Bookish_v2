using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

	public class NewBookViewModel
	{
		[Required]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Required]
		[Display(Name = "Author(s)")]
		public string Authors { get; set; }

		[Required]
		[Display(Name = "ISBN")]
		[RegularExpression("^(97(8|9))?\\d{9}(\\d|X)$", ErrorMessage = "This is not a valid 10 or 13 digit ISBN number. Please check and enter it again.")]
		public string Isbn { get; set; }

		[Display(Name = "Number of copies")]
		public int Copies { get; set; }
	}
}