using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Infrastructure._Data;
using Talabat.Infrastructure._Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling= ReferenceLoopHandling.Ignore;
            });
            // Register Required Web APIs Services  to the Di Container
            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddApplicationServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultCOnnection"));
            });

            webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityCOnnection"));
            });

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyOptions =>
                {
                    policyOptions.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials().WithOrigins("https://localhost:7019", "http://localhost:4200");
                });
            });

            #endregion

            var app = webApplicationBuilder.Build();

            #region Apply All Pending Migrations [Update Database] and Data Seeding

            ///var scope = app.Services.CreateScope();
            ///try
            ///{
            ///
            ///    var services = scope.ServiceProvider;
            ///
            ///    var _dbContext = services.GetRequiredService<StoreContext>();
            ///    // ASK CLR For Creating Object from DbContext Explicity
            ///    await _dbContext.Database.MigrateAsync();
            ///}
            ///finally { scope.Dispose(); }

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();
            var _identityDbdbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

            // ASK CLR For Creating Object from DbContext Explicitly

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();
            try
            {
                await _dbContext.Database.MigrateAsync();  // UPDATE Database  
                await StoreContextSeed.SeedAsync(_dbContext);  // Data Seeding

                await _identityDbdbContext.Database.MigrateAsync(); // UPDATE Database
                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityContextSeed.SeedUsersAsync(_userManager);

            }
            catch (Exception ex)
            {    
                logger.LogError(ex, "an error has been occured during apply the migration");
            }

            #endregion


            #region Configure Kestrel Middlewares

            app.UseMiddleware<ExceptionMiddleware>();
            ///app.Use(async (httpContext, _next) =>
            ///{
            ///    try
            ///    {
            ///        // Take an Action with the Requst
            ///        await _next.Invoke(httpContext);  // Go to the Next Middleware
            ///        // Take an Action with the Response
            ///    }
            ///    catch (Exception ex)
            ///    {
            ///        logger.LogError(ex.Message);  // Develeopment
            ///                                       // Log Exception in (Database | Files) // Production Env
            ///        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ///        httpContext.Response.ContentType = "application/json";
            ///        var response = app.Environment.IsDevelopment() ?
            ///            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
            ///            :
            ///            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
            ///        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            ///        var json = JsonSerializer.Serialize(response, options);
            ///        await httpContext.Response.WriteAsync(json);
            ///    }
            ///});


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
                
            }

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers(); 

            #endregion

            app.Run();
        }
    }
}
