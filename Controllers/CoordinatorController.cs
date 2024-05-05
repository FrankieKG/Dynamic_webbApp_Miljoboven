using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uppgift5.Infrastructure;
using uppgift5.Models;

namespace uppgift5.Controllers
{
	[Authorize(Roles = "Coordinator")]

	public class CoordinatorController : Controller
	{
		private readonly IRepository repository;
		private IHttpContextAccessor contextAcc;

		public CoordinatorController(IRepository repo, IHttpContextAccessor contextAcc)
		{
			repository = repo;
			this.contextAcc = contextAcc;
		}

		public ViewResult CoordStart()
		{
			ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
			ViewBag.ErrandList = repository.GetErrandListCoordinator();
			return View(repository);
		}

		public ViewResult CoordReport()
		{
			var myErrand = HttpContext.Session.Get<Errand>("NewErrand");
			if (myErrand == null)
			{
				return View();
			}
			else
			{
				return View(myErrand);
			}
		}

		public ViewResult CoordThanks()
		{
			var errand = HttpContext.Session.Get<Errand>("NewErrand");
			repository.SaveErrand(errand);
			ViewBag.RefNumber = errand.RefNumber;

			HttpContext.Session.Remove("NewErrand");

			return View();
		}

		[HttpPost]
		public ViewResult CoordValidate(Errand errand)
		{
			HttpContext.Session.Set<Errand>("NewErrand", errand);
			ViewBag.errand = errand;
			return View(errand);
		}

		public ViewResult CoordCrime(int id)
		{
			ViewBag.ID = id;
			TempData["ID"] = ViewBag.ID;

			return View(repository.Departments);
		}

		public IActionResult CoordSave(string choosenDepartment)
		{
			ViewBag.departmentID = choosenDepartment;
			TempData["DEP"] = choosenDepartment;

			return RedirectToAction("CoordUpdate");
		}

		public IActionResult CoordUpdate()
		{
			int id = int.Parse(TempData["ID"].ToString());
			string depID = TempData["DEP"].ToString();

			repository.UpdateDepartment(id, depID);
			return RedirectToAction("CoordStart");
		}
	}
}
