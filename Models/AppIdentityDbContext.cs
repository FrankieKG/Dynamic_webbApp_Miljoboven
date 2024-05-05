using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace uppgift5.Models
{
  //Användas för att fråga och spara instanser av entiteter

  public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
	{
		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
		{

		}
	}
}
