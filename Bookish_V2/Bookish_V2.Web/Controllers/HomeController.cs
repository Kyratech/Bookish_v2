using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookish_V2.DataAccessFmwk;
using Bookish_V2.Web.Models;
using System.Data.SqlClient;


namespace Bookish_V2.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Catalogue()
		{
			var bookishConnection = new BookishConnection();
			var catalogue = new LibraryCatalogueViewModel
			{
				inventory = bookishConnection.GetInventory()
			};

			ViewBag.Message = "Your application description page.";

			return View("Catalogue", catalogue);
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}