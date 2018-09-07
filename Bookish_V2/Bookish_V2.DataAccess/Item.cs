using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish_V2.DataAccess
{
	public class Item
	{
		public int ItemId { get; set; }
		public Book BookDetails { get; set; }

		public override string ToString()
		{
			return "ID: " + ItemId + "\nBook: \n" + BookDetails.ToString();
		}
	}
}
