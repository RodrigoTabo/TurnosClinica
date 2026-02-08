using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TurnosClinica.ApiClients;
using TurnosClinica.Application.Services;
using TurnosClinica.Components;
using TurnosClinica.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TurnosDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TurnosDbContext")));
builder.Services.AddScoped<ITurnosService, TurnosService>();

builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(nav.BaseUri) }; // <-- BaseUri, NO Uri
});

//inyectamos el servicio de Identity.
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;

    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TurnosDbContext>();


builder.Services.AddScoped<TurnosApiClient>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Agregamos el servicio de razor pages.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//Mapeamos razor pages
app.MapRazorPages();
app.Run();
