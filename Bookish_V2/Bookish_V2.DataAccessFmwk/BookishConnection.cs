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
			IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
			string sqlString = "SELECT * FROM [Books]";
			return (List<Book>)db.Query<Book>(sqlString);
		}

		public List<Item> GetAllItems()
		{
			var x = new SqlConnection();
			IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
			string sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId";
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

		public List<Item> GetAllItems(string searchTerm)
		{
			var x = new SqlConnection();
			IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
			string sqlString = "SELECT * FROM [Items] INNER JOIN [Books] ON Items.BookId=Books.BookId WHERE [Title]='" + searchTerm + "' OR [Authors]='" + searchTerm + "'";
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

		public SortedDictionary<Book, int> GetInventory()
		{
			var allItems = this.GetAllItems();
			var inventory = new SortedDictionary<Book, int>();

			foreach (var item in allItems)
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

		public SortedDictionary<Book, int> GetInventory(string searchTerm)
		{
			var allItems = this.GetAllItems(searchTerm);
			var inventory = new SortedDictionary<Book, int>();

			foreach (var item in allItems)
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
