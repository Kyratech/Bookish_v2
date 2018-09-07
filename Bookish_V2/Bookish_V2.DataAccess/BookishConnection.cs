using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
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
	}
}
