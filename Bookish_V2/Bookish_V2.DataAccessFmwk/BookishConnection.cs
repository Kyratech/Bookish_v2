using System;
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
			var sqlString = "SELECT * FROM Books;";
			return (List<Book>)db.Query<Book>(sqlString);
		}

		public List<Item> GetAllItems()
		{
			var sqlString = "SELECT * FROM Items INNER JOIN Books ON Items.BookId=Books.BookId;";
			return GetItemsByQuery(sqlString, null);
		}

		public List<Item> GetAllItems(string searchTerm)
		{
			var sqlString = "SELECT * FROM Items INNER JOIN Books ON Items.BookId=Books.BookId WHERE Title LIKE '%' + @Search + '%' OR Authors LIKE '%' + @Search + '%';";
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
			var bookItemsSqlQuery = "SELECT * FROM Items INNER JOIN Books ON Items.BookId=Books.BookId WHERE Items.BookId=@BookID;";
			var borrowedCopiesSqlQuery =
				"SELECT AspNetUsers.Email, Items.BookId, BorrowedItems.ItemId, BorrowedItems.DueDate FROM BorrowedItems INNER JOIN AspNetUsers ON BorrowedItems.UserId=AspNetUsers.Id INNER JOIN Items ON BorrowedItems.ItemId=Items.ItemId WHERE Items.BookId=@BookID;";
			
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
				"SELECT Books.*, Items.ItemId, BorrowedItems.DueDate FROM BorrowedItems INNER JOIN Items ON BorrowedItems.ItemId=Items.ItemId INNER JOIN Books ON Items.BookId=Books.BookId WHERE BorrowedItems.UserId=@UserID;";

			return db.Query<UserBookDetails>(userBookSqlString, new {UserID = userId}).ToList();
		}

		public void SubmitNewBook(Book bookDetails, int copies)
		{
			var db = GetBookishConnection();

			if (!BookAlreadyExists(bookDetails, db))
			{
				AddBook(bookDetails, db);
			}

			AddBookItems(bookDetails, copies, db);
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

		private bool BookAlreadyExists(Book bookDetails, IDbConnection db)
		{
			var checkSqlString = "SELECT COUNT(1) FROM Books WHERE ISBN=@bookIsbn;";

			var entries = db.QueryFirst<int>(checkSqlString, new {bookIsbn = bookDetails.ISBN});

			switch (entries)
			{
				case 0:
					return false;
				case 1:
					return true;
				default:
					throw new Exception("Database contains multiple books with the same ISBN.");
			}
		}

		private void AddBook(Book bookDetails, IDbConnection db)
		{
			var insertBookSqlString = "INSERT INTO Books (ISBN, Title, Authors) VALUES (@isbn, @title, @author);";

			var newBooks = db.Execute(insertBookSqlString,
				new
				{
					isbn = new DbString { Value = bookDetails.ISBN, IsAnsi = true},
					title = new DbString { Value = bookDetails.Title, IsAnsi = true },
					author = new DbString { Value = bookDetails.Authors, IsAnsi = true }
				});

			if (newBooks == 0)
			{
				throw new Exception("Failed to add book to database.");
			}
		}

		private void AddBookItems(Book bookDetails, int copies, IDbConnection db)
		{
			var bookId = GetBookId(bookDetails, db);

			var insertItemsSqlString = "INSERT INTO Items (BookId) VALUES (@id);";
			var newItems = 0;
			for (int i = 0; i < copies; i++)
			{
				newItems += db.Execute(insertItemsSqlString, new { id = bookId });
			}

			if (newItems != copies)
			{
				throw new Exception("Incorrect number of copies added. Should be: " + copies + ", but was actually: " +
				                    newItems);
			}
		}

		private int GetBookId(Book bookDetails, IDbConnection db)
		{
			var getBookByIsbnSqlString = "SELECT BookId FROM Books WHERE Isbn=@isbn;";
			return db.QuerySingle<int>(getBookByIsbnSqlString, new { isbn = new DbString { Value = bookDetails.ISBN, IsFixedLength  = true, Length = 13, IsAnsi = true}});
		}
	}
}
