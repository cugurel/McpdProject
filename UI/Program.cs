using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UI.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
	options.UseSqlServer(
		"server = Cagri; database=CrmDb; integrated security=true; TrustServerCertificate = True",
		sql => sql.EnableRetryOnFailure()
	);
});


builder.Services.AddSession(o =>
{
	o.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationContext>()
	.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 6;
	options.User.RequireUniqueEmail = true;
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureContainer<ContainerBuilder>(c =>
	{
		c.RegisterModule(new AutofacBusinessModule());
	});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Auth/Login";
	options.LogoutPath = "/Auth/Logout";
	options.SlidingExpiration = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();

// Areas → önce
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Auth}/{action=Login}/{id?}"
);

// Default
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


