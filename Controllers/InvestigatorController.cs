using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uppgift5.Models;


namespace uppgift5.Controllers
{
	[Authorize(Roles = "Investigator")]

	public class InvestigatorController : Controller
	{
		private readonly IRepository repository;
		private IHttpContextAccessor contextAcc;

		public InvestigatorController(IRepository repo, IHttpContextAccessor contextAcc)
		{
			repository = repo;
			this.contextAcc = contextAcc;
		}

		public ViewResult InvestStart()
		{
			ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
			string user = contextAcc.HttpContext.User.Identity.Name;
			ViewBag.ErrandList = repository.GetErrandListInvestigator(user);
			return View(repository);
		}

		public ViewResult InvestCrime(int id)
		{
			ViewBag.ID = id;
			TempData["ID"] = id;

			return View(repository.ErrandStatuses);
		}

		public async Task<IActionResult> InvestSave(string information, string events, string status, IFormFile loadSample, IFormFile loadImage)
		{
			if ((information == null) && (events == null) && (status == "Välj") && (loadSample == null) && (loadImage == null))
			{
				return RedirectToAction("InvestStart");
			}
			int id = int.Parse(TempData["ID"].ToString());
			if (status != "Välj")
			{
				repository.ChangeStatus(id, status);
			}
			if (information != null)
			{
				repository.CreateInvestigatorInfo(id, information);
			}
			if (events != null)
			{
				repository.CreateInvestigatorEvent(id, events);
			}
			if (loadSample != null)
			{
				await repository.UploadSample(id, loadSample);
			}
			if (loadImage != null)
			{
				await repository.UploadPicture(id, loadImage);
			}

			return RedirectToAction("InvestStart");
		}
	}
}
