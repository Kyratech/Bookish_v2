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
			var db = GetBookishConnection();
			var sqlString = "SELECT * FROM [Books]";
			return (List<Book>)db.Query<Book>(sqlString);
		}

		public List<Item> GetAllItems()
		{
			var sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId";
			return GetItemsByQuery(sqlString);
		}

		public List<Item> GetAllItems(string searchTerm)
		{
			var sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE [Title]='" + searchTerm + "' OR [Authors]='" + searchTerm + "'";
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
			var bookItemsSqlQuery = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE Items.BookId='" + bookId + "'";
			var borrowedCopiesSqlQuery =
				"SELECT AspNetUsers.Email, Items.BookId, BorrowedItems.ItemId, BorrowedItems.DueDate FROM [BorrowedItems] INNER JOIN AspNetUsers ON BorrowedItems.UserId=AspNetUsers.Id INNER JOIN Items ON BorrowedItems.ItemId=Items.ItemId WHERE Items.BookId='" + bookId + "'";
			
			var bookItems = GetItemsByQuery(bookItemsSqlQuery);
			var borrowDetails = GetBorrowDetailsByQuery(borrowedCopiesSqlQuery);

			CatalogueBookDetails bookDetails = new CatalogueBookDetails()
			{
				AvailableCopies = bookItems.Count - borrowDetails.Count,
				TotalCopies = bookItems.Count,
				BookDetails = bookItems[0].BookDetails,
				BorrowedItemDetails = borrowDetails
			};

			return bookDetails;
		}

		private IDbConnection GetBookishConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
		}

		private List<Item> GetItemsByQuery(string sqlString)
		{
			var db = GetBookishConnection();

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

		private List<BorrowDetails> GetBorrowDetailsByQuery(string sqlString)
		{
			var db = GetBookishConnection();

			return db.Query<BorrowDetails>(sqlString).ToList();
		}
	}
}
