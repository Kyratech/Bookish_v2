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
			return GetItemsByQuery(sqlString, null);
		}

		public List<Item> GetAllItems(string searchTerm)
		{
			var sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE [Title]=@Search OR [Authors]=@Search";
			return GetItemsByQuery(sqlString, new {Search = searchTerm});
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
			var bookItemsSqlQuery = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE Items.BookId=@BookID";
			var borrowedCopiesSqlQuery =
				"SELECT AspNetUsers.Email, Items.BookId, BorrowedItems.ItemId, BorrowedItems.DueDate FROM [BorrowedItems] INNER JOIN AspNetUsers ON BorrowedItems.UserId=AspNetUsers.Id INNER JOIN Items ON BorrowedItems.ItemId=Items.ItemId WHERE Items.BookId=@BookID";
			
			var bookItems = GetItemsByQuery(bookItemsSqlQuery, new{BookID = bookId});
			var borrowDetails = GetBorrowDetailsByQuery(borrowedCopiesSqlQuery, new {BookID = bookId});

			CatalogueBookDetails bookDetails = new CatalogueBookDetails
			{
				AvailableCopies = bookItems.Count - borrowDetails.Count,
				TotalCopies = bookItems.Count,
				BookDetails = bookItems[0].BookDetails,
				BorrowedItemDetails = borrowDetails
			};

			return bookDetails;
		}

		public List<UserBookDetails> GetUsersBooks(string userId)
		{
			var db = GetBookishConnection();
			var userBookSqlString =
				"SELECT Books.*, Items.ItemId, BorrowedItems.DueDate FROM [BorrowedItems] INNER JOIN Items ON BorrowedItems.ItemId=Items.ItemId INNER JOIN Books ON Items.BookId=Books.BookId WHERE BorrowedItems.UserId=@UserID";

			return db.Query<UserBookDetails>(userBookSqlString, new {UserID = userId}).ToList();
		}

		private IDbConnection GetBookishConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
		}

		private List<Item> GetItemsByQuery(string sqlString, object param)
		{
			var db = GetBookishConnection();

			var items = db.Query<Item, Book, Item>(
				sqlString,
				(item, book) =>
				{
					item.BookDetails = book;
					return item;
				},
				param,
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

		private List<BorrowDetails> GetBorrowDetailsByQuery(string sqlString, object param)
		{
			var db = GetBookishConnection();

			return db.Query<BorrowDetails>(sqlString, param).ToList();
		}
	}
}
