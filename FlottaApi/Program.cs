using FlottaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FlottaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<FlottaContext>(option => { string conn = builder.Configuration.GetConnectionString("FlottaConnection"); option.UseMySql(conn, ServerVersion.AutoDetect(conn)); });

            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5000"; // Az IdentityServer címe
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = false; // Kezdésnek kikapcsolva
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            

            app.Run();
        }
    }
}