using Microsoft.AspNetCore.Mvc;
using uppgift5.Infrastructure;
using uppgift5.Models;


namespace uppgift5.Controllers
{
	public class CitizenController : Controller
	{
		private readonly IRepository repository;

		public CitizenController(IRepository repo)
		{
			repository = repo;
		}

		public ViewResult Faq()
		{
			return View();
		}

		public ViewResult Kontakt()
		{
			return View();
		}

		public ViewResult Thanks()
		{
			var errand = HttpContext.Session.Get<Errand>("NewErrand");
			HttpContext.Session.Remove("NewErrand");
			repository.SaveErrand(errand);
			ViewBag.RefNumber = errand.RefNumber;

			return View();
		}

		public ViewResult Tjanster()
		{
			return View();
		}

		[HttpPost]
		public ViewResult Validate(Errand errand)
		{
			HttpContext.Session.Set<Errand>("NewErrand", errand);
			ViewBag.errand = errand;
			return View(errand);
		}
	}
}
