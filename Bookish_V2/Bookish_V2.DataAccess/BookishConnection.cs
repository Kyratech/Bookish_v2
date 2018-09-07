using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Dapper;

namespace Bookish_V2.DataAccess
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
	}
}
