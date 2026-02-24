using Microsoft.AspNetCore.Identity;
using TurnosClinica.Domain.Entities;

namespace TurnosClinica.Infrastructure.Identity // cambiá el namespace si querés
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); // <- CAMBIAR "Usuario"

            // 1) Crear roles si no existen
            var roles = new[] { Roles.Admin, Roles.Recepcionista, Roles.Medico };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                        throw new Exception($"No se pudo crear el rol '{role}': {errors}");
                    }
                }
            }

            // 2) Crear usuarios iniciales y asignar roles
            await CrearUsuarioSiNoExisteAsync(
                userManager,
                email: "admin@turnosclinica.com",
                password: "Admin123!",
                rol: Roles.Admin);

            await CrearUsuarioSiNoExisteAsync(
                userManager,
                email: "recepcion@turnosclinica.com",
                password: "Recep123!",
                rol: Roles.Recepcionista);

            await CrearUsuarioSiNoExisteAsync(
                userManager,
                email: "medico@turnosclinica.com",
                password: "Medico123!",
                rol: Roles.Medico);
        }

        private static async Task CrearUsuarioSiNoExisteAsync(
            UserManager<IdentityUser> userManager, // <- CAMBIAR "Usuario"
            string email,
            string password,
            string rol)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new IdentityUser // <- CAMBIAR "Usuario"
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(" | ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"No se pudo crear el usuario '{email}': {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, rol))
            {
                var addRoleResult = await userManager.AddToRoleAsync(user, rol);

                if (!addRoleResult.Succeeded)
                {
                    var errors = string.Join(" | ", addRoleResult.Errors.Select(e => e.Description));
                    throw new Exception($"No se pudo asignar el rol '{rol}' al usuario '{email}': {errors}");
                }
            }
        }
    }
}