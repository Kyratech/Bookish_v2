using System.Collections.Generic;
using Bookish_V2.DataAccess;

namespace Bookish_V2.Web.Models
{
	public class LibraryCatalogueViewModel
	{
		public Dictionary<Book, int> inventory { get; set; }
	}
}