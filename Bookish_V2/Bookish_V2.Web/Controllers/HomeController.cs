using System.Web.Mvc;

namespace Bookish_V2.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}