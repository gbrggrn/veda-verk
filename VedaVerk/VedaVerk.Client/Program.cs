using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VedaVerk.Client;
using VedaVerk.Client.Services;
using VedaVerk.Client.Services.Implementations;
using VedaVerk.Client.Services.Interfaces;

namespace VedaVerk.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

            builder.Services.AddTransient<CookieHandler>();

			builder.Services.AddHttpClient("API", client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            })
            .AddHttpMessageHandler<CookieHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                  .CreateClient("API"));

			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IBookingService, BookingService>();

			await builder.Build().RunAsync();
        }
    }
}
        