using System.Collections.Generic;

namespace Bookish_V2.DataAccessFmwk
{
	public class CatalogueBookDetails
	{
		public Book BookDetails { get; set; }
		public int TotalCopies { get; set; }
		public int AvailableCopies { get; set; }
		public List<BorrowDetails> BorrowedItemDetails { get; set; }
	}
}
