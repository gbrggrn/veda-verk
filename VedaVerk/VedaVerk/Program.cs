using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VedaVerk.Client.Pages;
using VedaVerk.Services;
using VedaVerk.Components;
using VedaVerk.Components.Account;
using VedaVerk.Data;
using VedaVerk.Models.Enitites;
using VedaVerk.Repositiories.Implementations;
using VedaVerk.Repositiories.Interfaces;
using VedaVerk.Shared.Enums;

namespace VedaVerk
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

			builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			builder.Services.AddScoped<BookingService>();

			builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            //!!! Seed test data
			using (var scope = app.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

				context.Database.EnsureCreated();

				if (!context.Products.Any())
				{
					context.Products.AddRange(new List<Product>
					{
						new Product {
							Name = "Höstkasse (Small)",
							Price = 250,
							Type = ProductType.VegBag,
							Description = "Grönsaker",
							Capacity = 50,
							OpenTime = new TimeSpan(8, 0, 0),
							CloseTime = new TimeSpan(18, 0, 0),
							IntervalMinutes = 30,
							CapacityPerSlot = 5,
							IsActive = true,
							Created = DateTime.UtcNow,
							LastUpdated = DateTime.UtcNow,
							ActiveFrom = DateTime.UtcNow,
							ActiveTo = DateTime.UtcNow.AddDays(7)
							},
						new Product {
							Name = "Matlagningskurs",
							Price = 1250,
							Type = ProductType.Course,
							Description = "Kurs kurs!",
							Capacity = 2,
							OpenTime = new TimeSpan(8, 0, 0),
							CloseTime = new TimeSpan(18, 0, 0),
							IntervalMinutes = 30,
							CapacityPerSlot = 2,
							IsActive = true,
							Created = DateTime.UtcNow,
							LastUpdated = DateTime.UtcNow,
							ActiveFrom = DateTime.UtcNow,
							ActiveTo = DateTime.UtcNow.AddDays(7)
						},
						new Product {
							Name = "Pizza-Fredag!",
							Price = 145,
							Type = ProductType.Pizza,
							Description = "Goda pizzor!",
							Capacity = 20,
							OpenTime = new TimeSpan(8, 0, 0),
							CloseTime = new TimeSpan(8, 0 ,0),
							IntervalMinutes = 30,
							CapacityPerSlot = 10,
							IsActive = true,
							Created = DateTime.UtcNow,
							LastUpdated = DateTime.UtcNow,
							ActiveFrom = DateTime.UtcNow,
							ActiveTo = DateTime.UtcNow.AddDays(7)
						}
					});

					context.SaveChanges();
				}
			}
			//!!! End seed test data

			// -----------------------------------------------------------------------------
			// SECURE ADMIN SEEDER
			// -----------------------------------------------------------------------------
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					// 1. Get the password from Configuration (User Secrets or Env Vars)
					var adminPassword = app.Configuration["Seed:AdminPassword"];

					// Only run if a password is configured
					if (!string.IsNullOrEmpty(adminPassword))
					{
						var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
						var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
						var adminEmail = "admin@vedaverk.se";

						// 2. Ensure Admin Role Exists
						if (!await roleManager.RoleExistsAsync("Admin"))
						{
							await roleManager.CreateAsync(new IdentityRole("Admin"));
						}

						// 3. Create Admin User if Missing
						if (await userManager.FindByEmailAsync(adminEmail) == null)
						{
							var adminUser = new ApplicationUser
							{
								UserName = adminEmail,
								Email = adminEmail,
								EmailConfirmed = true
							};

							var result = await userManager.CreateAsync(adminUser, adminPassword);

							if (result.Succeeded)
							{
								await userManager.AddToRoleAsync(adminUser, "Admin");
								Console.WriteLine("Admin user seeded successfully.");
							}
							else
							{
								// Log errors to console so you know why it failed (e.g. password too weak)
								foreach (var error in result.Errors)
								{
									Console.WriteLine($"Seeding Failed: {error.Description}");
								}
							}
						}
					}
					else
					{
						Console.WriteLine("Skipping Admin Seeding: 'Seed:AdminPassword' not found.");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
				}
			}
			// -----------------------------------------------------------------------------

			app.Run();
        }
    }
}
