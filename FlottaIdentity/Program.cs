using FlottaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FlottaIdentity
{
    public class Program
    { 
         public static void Main(string[] args)
         {
            
            var builder = WebApplication.CreateBuilder(args);

            //Connection string alapján előkészítjük a FlottaContext objektumot
            var connStr = builder.Configuration.GetConnectionString("FlottaConnection");
            builder.Services.AddDbContext<FlottaContext>(options => options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));


            //Konfiguráljuk az IdentityServer-t, hogy a Config.cs-ben megadott beállítások szerint működjön
            builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddResourceOwnerValidator<FlottaPasswordValidator>() // <--- Saját Argon2 ellenőrzés
            .AddProfileService<FlottaProfileService>()             // <--- Saját claim-ek betöltése
            .AddDeveloperSigningCredential();



            var app = builder.Build();

            app.UseIdentityServer();



            app.Run();

         }


    }

}


