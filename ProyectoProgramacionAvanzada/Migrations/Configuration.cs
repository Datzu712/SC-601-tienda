using ProyectoProgramacionAvanzada.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ProyectoProgramacionAvanzada.Migrations
{
    internal sealed class Configuration
        : DbMigrationsConfiguration<ProyectoProgramacionAvanzada.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProyectoProgramacionAvanzada.Models.ApplicationDbContext context)
        {
            // ============================
            // ROLES
            // ============================
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Administrador"))
                roleManager.Create(new IdentityRole("Administrador"));

            if (!roleManager.RoleExists("Asociado"))
                roleManager.Create(new IdentityRole("Asociado"));

            // ============================
            // USUARIO ADMIN
            // ============================
            var adminEmail = "admin@bazar.com";
            var adminUser = userManager.FindByName(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    CodigoUsuario = "ADM-001",
                    EstaActivo = true,
                    UltimaConexion = DateTime.Now
                };

                var result = userManager.Create(adminUser, "Admin123!");

                if (result.Succeeded)
                    userManager.AddToRole(adminUser.Id, "Administrador");
            }
            
            if (!context.Productos.Any())
            {
                context.Productos.Add(new Producto
                {
                    CodigoProducto = "PROD-001",
                    Nombre = "Cuaderno Profesional",
                    Precio = 2500,
                    Inventario = 10,
                    Activo = true
                });

                context.Productos.Add(new Producto
                {
                    CodigoProducto = "PROD-002",
                    Nombre = "Lapicero Azul",
                    Precio = 500,
                    Inventario = 20,
                    Activo = true
                });
            }
        }
    }
}
