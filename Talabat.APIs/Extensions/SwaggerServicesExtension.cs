using Microsoft.AspNetCore.Builder;

namespace Talabat.APIs.Extensions
{
    public static class SwaggerServicesExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //services.AddSwaggerGen(C =>
            //{
            //    // I Write this to remove error because i have 2 Entity Table of the same name (Address) for [ Identity && Order ]
            //    // can customize the schema IDs for conflicting types to ensure they are unique.
            //    // This can be done by configuring Swashbuckle to generate unique schema IDs.
            //    // This configuration uses the full name of the type (including the namespace) as the schema ID, ensuring uniqueness
            //    // This configuration uses the full name of the type (including the namespace) as the schema ID, ensuring uniqueness
            //    C.CustomSchemaIds(type => type.FullName);
            //});


            return services;
        }

        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(C => C.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

            return app;
        }

    }
}
