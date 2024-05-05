using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uppgift5.Models;


namespace uppgift5.Controllers
{
	[Authorize(Roles = "Manager")]

	public class ManagerController : Controller
	{
		private readonly IRepository repository;
		private IHttpContextAccessor contextAcc;

		public ManagerController(IRepository repo, IHttpContextAccessor contextAcc)
		{
			repository = repo;
			this.contextAcc = contextAcc;
		}

		public ViewResult ManagStart()
		{
			ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
			string user = contextAcc.HttpContext.User.Identity.Name;
			ViewBag.ErrandList = repository.GetErrandListManager(user);
			ViewBag.Employee = repository.GetEmployee(user);
			ViewBag.Department = repository.GetDepFromEmp(user);

			return View(repository);
		}

		public ViewResult ManagCrime(int id)
		{
			ViewBag.ID = id;
			TempData["ID"] = ViewBag.ID;

      ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
      string user = contextAcc.HttpContext.User.Identity.Name;
      ViewBag.Employee = repository.GetEmployee(user);
      ViewBag.Department = repository.GetDepFromEmp(user);

      return View(repository.Employees);
		}

		public IActionResult ManagSave(string reason, string investigator, bool noAction)
		{
			int id = int.Parse(TempData["ID"].ToString());

			if ((noAction == true) && (investigator == "Välj"))
			{
				if (reason != "Ange motivering")
				{
					repository.UpdateReason(id, reason);
				}
			}
			if ((investigator != "Välj") && (noAction == false))
			{
				repository.UpdateEmployeeId(id, investigator);
			}

			return RedirectToAction("ManagStart");
		}
	}
}
