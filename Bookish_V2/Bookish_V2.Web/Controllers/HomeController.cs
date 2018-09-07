using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookish_V2.DataAccess;
using Bookish_V2.Web.Models;

namespace Bookish_V2.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var db = new BookishConnection();
			var catalogue = new LibraryCatalogueViewModel
			{
				inventory = db.GetInventory()
			};

			return View("Index", catalogue);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}