using Microsoft.AspNetCore.Mvc;
using uppgift5.Models;

namespace uppgift1_Layout.Components
{
  //Hämtar ärende object som är en kopia av sessionen där användaren skriver in uppgifter för brottet hen vill rapportera
	public class ShowValidate : ViewComponent
	{
    private readonly IRepository repository;

    public ShowValidate(IRepository repo)
    {
      repository = repo;
    }

    public async Task<IViewComponentResult> InvokeAsync(Errand errand)
    {
      return View(errand);
    }
  }
}
