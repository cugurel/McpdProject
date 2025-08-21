using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using UI.Models.Identity;

var builder = WebApplication.CreateBuilder(args);
LoadWkhtmltox(builder.Environment);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
	options.UseSqlServer("server = Cagri; database=CrmDb; " +
		"integrated security=true; TrustServerCertificate = True",
		builder => builder.EnableRetryOnFailure());
});
builder.Services.AddSingleton<IConverter>(sp => new SynchronizedConverter(new PdfTools()));
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddIdentity<User, IdentityRole>().
	AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

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
	.ConfigureContainer<ContainerBuilder>(builder =>
	{
		builder.RegisterModule(new AutofacBusinessModule());
	});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Auth/Login";
	options.LogoutPath = "/Auth/Logout";
	options.SlidingExpiration = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Auth}/{action=Login}/{id?}");

// Sonra default route tanýmý
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
static void LoadWkhtmltox(IWebHostEnvironment env)
{
	var root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

	if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
	{
		var dllPath = Path.Combine(root, "lib", "wkhtmltox", "win64", "wkhtmltox.dll");
		if (!File.Exists(dllPath))
			throw new FileNotFoundException("wkhtmltox.dll bulunamadý", dllPath);

		var success = NativeLibrary.TryLoad(dllPath, out _);
		if (!success) throw new Exception($"wkhtmltox.dll yüklenemedi: {dllPath}");
	}
	else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
	{
		var soPath = Path.Combine(root, "lib", "wkhtmltox", "linux", "libwkhtmltox.so");
		if (!File.Exists(soPath))
			throw new FileNotFoundException("libwkhtmltox.so bulunamadý", soPath);

		var success = NativeLibrary.TryLoad(soPath, out _);
		if (!success) throw new Exception($"libwkhtmltox.so yüklenemedi: {soPath}");
	}
	else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
	{
		var dylibPath = Path.Combine(root, "lib", "wkhtmltox", "osx", "libwkhtmltox.dylib");
		if (!File.Exists(dylibPath))
			throw new FileNotFoundException("libwkhtmltox.dylib bulunamadý", dylibPath);

		var success = NativeLibrary.TryLoad(dylibPath, out _);
		if (!success) throw new Exception($"libwkhtmltox.dylib yüklenemedi: {dylibPath}");
	}
}