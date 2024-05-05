using Microsoft.AspNetCore.Mvc;
using uppgift5.Models;

namespace uppgift5.Components
{
	//Visar detaljer av ett ärende med ett specifikt ärende ID
	public class ShowOneCrime : ViewComponent
	{
		private readonly IRepository repository;
		public ShowOneCrime(IRepository repo)
		{
			repository = repo;
		}

		public async Task<IViewComponentResult> InvokeAsync(int id)
		{
			var ErrandDetails = await repository.GetErrandDetails(id);
			return View(ErrandDetails);
		}
	}
}
