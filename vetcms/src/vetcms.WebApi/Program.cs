using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Web.Http;
using vetcms.ServerApplication;
using vetcms.WebApi.Filters;

namespace vetcms.WebApi
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //options => options.Filters.Add<ApiExceptionFilterAttribute>()
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(ServerDependencyInitializer).Assembly)
                .AddControllersAsServices();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options => options.AddDefaultPolicy(
                    policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

            // Register the Swagger generator, defining 1 or more Swagger documents
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "vetcms API", Version = "v1" }));

            builder.Services.AddProblemDetails();

            // csekkolja hogy az ef add-migrationba futtatja, �s akkor nem hajtja v�gre, mert magyar�zni fog a mediatr
            if (!EF.IsDesignTime)
                builder.Services.AddServerApp();


            builder.Services.AddInfrastructure(builder.Configuration);


            builder.Services.AddHealthChecks();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseCors();

            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error-development"); //ez
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }

}