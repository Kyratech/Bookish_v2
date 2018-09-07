namespace Bookish_V2.DataAccess
{
    public class Book
    {
		public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }

	    public override string ToString()
	    {
		    return "Title: " + Title + "\nAuthors: " + Authors + "\nISBN: " + ISBN;
	    }
    }
}
