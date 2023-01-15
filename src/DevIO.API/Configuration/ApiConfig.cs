using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());

                // exemplo de como seria adicionar um CORS
                options.AddPolicy("Production",
                   builder =>
                       builder
                           .WithMethods("GET")
                           .WithOrigins("https://desenvolvedor.io")
                           .SetIsOriginAllowedToAllowWildcardSubdomains()
                           .AllowAnyHeader());

            });


            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production"); // Usar apenas nas demos => Configuração Ideal: Production
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Autenticacao e autorização (Identity)
            app.UseAuthentication();
            app.UseAuthorization();

            return app;

        }
    }
}
