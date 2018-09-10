using System.Web.Mvc;
using Bookish_V2.DataAccessFmwk;
using Bookish_V2.Web.Models;
using Microsoft.AspNet.Identity;

namespace Bookish_V2.Web.Controllers
{
    public class LibraryController : Controller
    {
		public ActionResult Catalogue()
	    {
		    var bookishConnection = new BookishConnection();
		    var catalogue = new LibraryCatalogueViewModel()
		    {
			    inventory = bookishConnection.GetInventory()
		    };

		    return View("Catalogue", catalogue);
	    }

	    [HttpPost]
	    public ActionResult Catalogue(LibraryCatalogueViewModel viewModel)
	    {
		    var bookishConnection = new BookishConnection();
		    var catalogue = new LibraryCatalogueViewModel()
		    {
			    inventory = bookishConnection.GetInventory(viewModel.SearchTerm)
		    };

		    return View("Catalogue", catalogue);
	    }

	    public ActionResult Book(int bookId)
	    {
		    var bookishConnection = new BookishConnection();
		    var bookDetails = bookishConnection.GetBookDetails(bookId);
		    var bookDetailsViewModel = new LibraryItemViewModel
		    {
			    AvailableCopies = bookDetails.AvailableCopies,
			    TotalCopies = bookDetails.TotalCopies,
			    Book = bookDetails.BookDetails,
			    BorrowedItemDetails = bookDetails.BorrowedItemDetails
		    };

		    return View(bookDetailsViewModel);
	    }

	    public ActionResult MyBooks()
	    {
			var bookishConnection = new BookishConnection();
		    var myId = User.Identity.GetUserId();

		    var userBooks = bookishConnection.GetUsersBooks(myId);
		    var userBooksModel = new UserCatalogueViewModel
		    {
			    MyBooks = userBooks
		    };

		    return View("MyBooks", userBooksModel);
	    }
	}
}