using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uppgift5.Infrastructure;
using uppgift5.Models;



namespace uppgift5.Controllers
{
	public class HomeController : Controller
	{
		private readonly IRepository repository;
		private UserManager<IdentityUser> userManager;
		private SignInManager<IdentityUser> signInManager;

		public HomeController(IRepository repo, UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInmgr)
		{
			userManager = userMgr;
			signInManager = signInmgr;
			repository = repo;
		}

		public ViewResult Index()
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

		[AllowAnonymous]
		public ViewResult Login(string returnUrl)
		{
			return View(new LoginModel
			{
				ReturnUrl = returnUrl
			});
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{
			if (ModelState.IsValid)
			{
				IdentityUser user = await userManager.FindByNameAsync(loginModel.UserName);
				if (user != null)
				{
					await signInManager.SignOutAsync();
					if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
					{
						if (await userManager.IsInRoleAsync(user, "Coordinator"))
						{
							return Redirect("/Coordinator/CoordStart");
						}
						if (await userManager.IsInRoleAsync(user, "Investigator"))
						{
							return Redirect("/Investigator/InvestStart");
						}
						if (await userManager.IsInRoleAsync(user, "Manager"))
						{
							return Redirect("/Manager/ManagStart");
						}
					}
				}
			}
			ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord");
			return View(loginModel);
		}

		public async Task<RedirectResult> Logout(string returnUrl = "Login")
		{
			await signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

		[AllowAnonymous]
		public ViewResult AccessDenied()
		{
			return View();
		}
	}
}
