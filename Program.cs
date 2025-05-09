using EvidenceVault.Components;
using EvidenceVault.Data;
using EvidenceVault.DTO;
using EvidenceVault.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure Entity Framework with SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ASP.NET Core Identity services.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register HttpClient with the base address of your API.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });


// Add controllers to the services collection.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI documentation generation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EvidenceVault API",
        Version = "v1",
        Description = "API for managing forensic evidence",
    });
});

var app = builder.Build();

// Run Role and SuperAdmin Seeding on Startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await SeedRolesAndSuperAdmin(roleManager, userManager);
}

// Configure the HTTP request pipeline.

// Enable Swagger and Swagger UI in the development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EvidenceVault API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the root URL
    });
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enable authentication and authorization.
app.UseAuthentication();
app.UseAuthorization();

// Enable routing.
app.UseRouting();

// Map controller endpoints.
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Map static assets and Razor components.
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

//  Move the Seed Method Outside to Keep Things Clean
static async Task SeedRolesAndSuperAdmin(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
{
    string[] roleNames = { "Investigator", "Police Officer" };

    foreach (var role in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create a default Super Admin
    var superAdminEmail = "superadmin@gmail.com";
    var existingSuperAdmin = await userManager.FindByEmailAsync(superAdminEmail);

    if (existingSuperAdmin == null)
    {
        var superAdmin = new ApplicationUser
        {
            UserName = superAdminEmail,
            Email = superAdminEmail,
            Name = "Super Admin",
            Role = "SuperAdmin",
            IsSuperAdmin = true
        };

        var result = await userManager.CreateAsync(superAdmin, "Password123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }
    }
}
