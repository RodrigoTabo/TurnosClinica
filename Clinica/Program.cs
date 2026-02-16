using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TurnosClinica.ApiClients.Especialidades;
using TurnosClinica.ApiClients.Medicos;
using TurnosClinica.ApiClients.Paises;
using TurnosClinica.ApiClients.Turnos;
using TurnosClinica.ApiClients.Provincias;
using TurnosClinica.ApiClients.Ciudades;
using TurnosClinica.ApiClients.Consultorios;
using TurnosClinica.ApiClients.Estados;
using TurnosClinica.ApiClients.Pacientes;
using TurnosClinica.Application.Services.Especialidades;
using TurnosClinica.Application.Services.Medicos;
using TurnosClinica.Application.Services.Paises;
using TurnosClinica.Application.Services.Turnos;
using TurnosClinica.Application.Services.Provincias;
using TurnosClinica.Application.Services.Ciudades;
using TurnosClinica.Application.Services.Consultorios;
using TurnosClinica.Application.Services.Estados;
using TurnosClinica.Application.Services.Pacientes;
using TurnosClinica.Components;
using TurnosClinica.Controllers;
using TurnosClinica.Infrastructure.Data;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TurnosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TurnosDbContext")));

//CONTEXTOS SERVICES.
builder.Services.AddScoped<ITurnosService, TurnosService>();
builder.Services.AddScoped<IMedicosService, MedicosService>();
builder.Services.AddScoped<IEspecialidadesService, EspecialidadesService>();
builder.Services.AddScoped<IPaisesService, PaisesService>();
builder.Services.AddScoped<IProvinciasService, ProvinciasService>();
builder.Services.AddScoped<ICiudadService, CiudadService>();
builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();
builder.Services.AddScoped<IPacientesService, PacientesService>();








builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});

builder.Services.AddScoped<TurnosApiClient>();
builder.Services.AddScoped<MedicosApiClient>();
builder.Services.AddScoped<EspecialidadesApiClient>();
builder.Services.AddScoped<PaisesApiClient>();
builder.Services.AddScoped<ProvinciasApiClient>();
builder.Services.AddScoped<CiudadesApiClient>();
builder.Services.AddScoped<ConsultoriosApiClient>();
builder.Services.AddScoped<EstadosApiClient>();
builder.Services.AddScoped<PacientesApiClient>();


builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        // Password
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;

        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TurnosDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
.AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    options.SlidingExpiration = true;
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseAntiforgery();

// MUST be before mapping endpoints
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ApiExceptionMiddleware>();
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
