using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GayQuyTuThien.DataContext;
using GayQuyTuThien.DataContext.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using GayQuyTuThien.Services;
using GayQuyTuThien;
using GayQuyTuThien.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataDbContext>(opstions => opstions.UseNpgsql(connectionString));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = false;
    opt.SignIn.RequireConfirmedAccount = false;
    //previous code removed for clarity reasons
    opt.Lockout.AllowedForNewUsers = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(365);
    opt.Lockout.MaxFailedAccessAttempts = 5;

    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DataDbContext>();

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    options =>
    {
        //options.Cookie.Expiration = TimeSpan.FromMinutes(20);
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/accessDenied";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
builder.Services.AddScoped<DbContext, DataDbContext>();

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IFunctionService, FunctionService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<IPictureService, PictureService>();
builder.Services.AddTransient<ISubmitFormService, SubmitFormService>();
builder.Services.AddTransient<IHtmlContentService, HtmlContentService>();

var pgrmData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
Directory.CreateDirectory($"{pgrmData}\\myaspnetwebapp\\keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo($"{pgrmData}\\myaspnetwebapp\\keys"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<DataDbContext>();
		context.Database.Migrate();

		var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
		var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

		await DataInitializer.SeedRoleAsync(userManager, roleManager);
		await DataInitializer.SeedBasicUserSupperAsync(userManager, roleManager);
		await DataInitializer.SeedBasicUserAsync(userManager, roleManager);
		await DataInitializer.SeedSuperAdminAsync(userManager, roleManager);
		await DataInitializer.SeedMenuAsync(context, roleManager);
	}
	catch (Exception ex)
	{
		//logger.Info($"Exception Seeding Default Data");
	}
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
             name: "areas",
             pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
           );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); // This route is for Controllers which are situated in project controller folder
});
app.Run();
