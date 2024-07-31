using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Application.AuthService;
using Talabat.Application.CacheService;
using Talabat.Application.OrderService;
using Talabat.Application.PaymentService;
using Talabat.Application.ProductService;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure._Identity;
using Talabat.Infrastructure.Generic_Repository;
using Talabat.Infrastructure.Repositories.Basket_Repository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IResponseCacheSerevice), typeof(ResponseCacheSerevice));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            ///webApplicationBuilder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            ///webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
            ///webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToList();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            //services.AddScoped<IConnectionMultiplexer>((serviceProvider) =>
            //{
            //    var connection = Configuration.GetConnectionString("redis");
            //    return ConnectionMultiplexer.Connect(connection);
            //});

            return services;

        }


        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireUppercase = true;
            }) // Allow Dependency Injection 
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.AddAuthentication(/*"Bearer"*/JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                })/*.AddJwtBearer("Bearer02", options => { })*/;

            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            return services;
        }
    }
}
