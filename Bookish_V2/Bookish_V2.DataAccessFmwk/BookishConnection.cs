using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Dapper;

namespace Bookish_V2.DataAccessFmwk
{
	public class BookishConnection
	{
		public List<Book> GetAllBooks()
		{
			IDbConnection db = GetBookishConnection();
			string sqlString = "SELECT * FROM [Books]";
			return (List<Book>)db.Query<Book>(sqlString);
		}

		public List<Item> GetAllItems()
		{
			string sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId";
			return GetItemsByQuery(sqlString);
		}

		public List<Item> GetAllItems(string searchTerm)
		{
			string sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE [Title]='" + searchTerm + "' OR [Authors]='" + searchTerm + "'";
			return GetItemsByQuery(sqlString);
		}

		public SortedDictionary<Book, int> GetInventory()
		{
			var allItems = this.GetAllItems();
			return GenerateInventoryFromListOfAllItems(allItems);
		}

		public SortedDictionary<Book, int> GetInventory(string searchTerm)
		{
			List<Item> allItems;

			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				allItems = this.GetAllItems();
			}
			else
			{
				allItems = this.GetAllItems(searchTerm);
			}

			return GenerateInventoryFromListOfAllItems(allItems);
		}

		public CatalogueBookDetails GetBookDetails(int bookId)
		{
			string sqlQuery = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE Items.BookId='" + bookId + "'";
			List<Item> bookItems = GetItemsByQuery(sqlQuery);

			CatalogueBookDetails bookDetails = new CatalogueBookDetails()
			{
				AvailableCopies = bookItems.Count,
				TotalCopies = bookItems.Count,
				BookDetails = bookItems[0].BookDetails
			};

			return bookDetails;
		}

		private IDbConnection GetBookishConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
		}

		private List<Item> GetItemsByQuery(string sqlString)
		{
			IDbConnection db = GetBookishConnection();

			var items = db.Query<Item, Book, Item>(
				sqlString,
				(item, book) =>
				{
					item.BookDetails = book;
					return item;
				},
				splitOn: "BookId").Distinct().ToList();
			return items;
		}

		private SortedDictionary<Book, int> GenerateInventoryFromListOfAllItems(List<Item> listOfAllItems)
		{
			var inventory = new SortedDictionary<Book, int>();

			foreach (var item in listOfAllItems)
			{
				if (inventory.ContainsKey(item.BookDetails))
				{
					inventory[item.BookDetails] = inventory[item.BookDetails] + 1;
				}
				else
				{
					inventory.Add(item.BookDetails, 1);
				}
			}

			return inventory;
		}
	}
}
