using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebVerdandiMedReg.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity + Roles
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequiredLength = 6;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();


// --------------------------------------------------
// Seed roles + bootstrap SuperAdmin
// --------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Roles used by the app
    string[] roles = { "User", "PowerUser", "SuperAdmin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Make sure YOU are SuperAdmin (change email if needed)
    var superAdminEmail = "edgar.massey@gmail.com";

    var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
    if (superAdminUser != null)
    {
        if (!await userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
            await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");

        if (!await userManager.IsInRoleAsync(superAdminUser, "PowerUser"))
            await userManager.AddToRoleAsync(superAdminUser, "PowerUser");
    }
}


// --------------------------------------------------
// Middleware pipeline
// --------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
