using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add services to the container.
        builder.Services.AddDbContext<AnimalPartyContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddEntityFrameworkStores<AnimalPartyContext>()
        .AddDefaultTokenProviders();


        var app = builder.Build();

        await SetupRolesAndUsersAsync(app.Services);

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static async Task SetupRolesAndUsersAsync(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            // Ensure the database is created and migrated
            var context = serviceProvider.GetRequiredService<AnimalPartyContext>();
            context.Database.Migrate();

            // Create roles if they do not exist
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "admin", "klant" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create a default manager user if it doesn't exist
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string email = "admin@boerderij.nl";
            string password = "Wachtwoord123!";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}