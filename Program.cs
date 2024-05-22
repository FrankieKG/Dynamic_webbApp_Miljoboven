using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uppgift5.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IRepository, EFRepository>();

builder.Services.AddSession();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
{
	sqlOptions.EnableRetryOnFailure();
}));

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Home/Login";
	options.AccessDeniedPath = "/Home/AccessDenied";
});

var app = builder.Build();
//anrop för att skapa en service för vår testdata
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	DbInitializer.EnsurePopulated(services);
	IdentityInitializer.EnsurePopulated(services).Wait();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
