using System;

namespace Bookish_V2.DataAccessFmwk
{
    public class Book: IComparable
    {
		public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }

	    public override string ToString()
	    {
		    return "Title: " + Title + "\nAuthors: " + Authors + "\nISBN: " + ISBN;
	    }

	    public override bool Equals(object obj)
	    {
		    if (ReferenceEquals(null, obj))
		    {
			    return false;
		    }

		    if (ReferenceEquals(this, obj))
		    {
			    return true;
		    }

		    if (obj.GetType() != this.GetType())
		    {
			    return false;
		    }

		    return IsEquals((Book) obj);
	    }

	    public bool Equals(Book otherBook)
	    {
		    if (ReferenceEquals(null, otherBook))
		    {
			    return false;
		    }

		    if (ReferenceEquals(this, otherBook))
		    {
			    return true;
		    }

		    return IsEquals(otherBook);
	    }

	    private bool IsEquals(Book other)
	    {
		    return BookId == other.BookId;
		}

	    public override int GetHashCode()
	    {
		    unchecked
		    {
			    const int hashBase = (int) 2166136261;
			    const int hashMultiplier = 16777619;

			    return (hashBase * hashMultiplier) ^
			               (!Object.ReferenceEquals(null, BookId) ? BookId.GetHashCode() : 0);
		    }
	    }

		public int CompareTo(object obj)
		{
			Book other = (Book) obj;
			return String.Compare(this.Title, other.Title);
		}
	}
}
