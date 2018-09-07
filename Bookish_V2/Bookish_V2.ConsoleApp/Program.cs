using System;
using System.Collections.Generic;
using Bookish_V2.DataAccess;

namespace Bookish_V2.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			BookishConnection db = new BookishConnection();
			List<Book> allBooks = db.GetAllBooks();

			foreach (var book in allBooks)
			{
				Console.WriteLine(new string('*', 20));
				Console.WriteLine();
				Console.WriteLine("Book Title: " + book.Title);
				Console.WriteLine("Authors: " + book.Authors);
				Console.WriteLine("ISBN: " + book.ISBN);
				Console.WriteLine();
				Console.WriteLine(new string('*', 20));
			}

			Console.ReadLine();
		}
	}
}
