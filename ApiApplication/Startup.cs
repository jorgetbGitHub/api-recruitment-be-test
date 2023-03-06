using ApiApplication.Auth;
using ApiApplication.Auth.Policies;
using ApiApplication.Controllers;
using ApiApplication.Core;
using ApiApplication.Database;
using ApiApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CinemaContext>(options =>
            {
                options.UseInMemoryDatabase("CinemaDb")
                    .EnableSensitiveDataLogging()
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));                
            });
            services.AddTransient<IShowtimesRepository, ShowtimesRepository>();
            services.AddSingleton<ICustomAuthenticationTokenService, CustomAuthenticationTokenService>();
            services.AddHostedService<IMDbUpdaterHostedService>();

            // Authentication configuration
            services.AddAuthentication(options =>
            {
                options.AddScheme<CustomAuthenticationHandler>(CustomAuthenticationSchemeOptions.AuthenticationScheme, CustomAuthenticationSchemeOptions.AuthenticationScheme);
                options.RequireAuthenticatedSignIn = true;                
                options.DefaultScheme = CustomAuthenticationSchemeOptions.AuthenticationScheme;
            });

            // DI Authorization handlers 
            services.AddSingleton<IAuthorizationHandler, ReadHandler>();
            services.AddSingleton<IAuthorizationHandler, WriteHandler>();

            // Authorization configuration
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Read", policy => policy.Requirements.Add(new Read()));
                options.AddPolicy("Write", policy => policy.Requirements.Add(new Write()));
            });

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance);
            services.AddHttpContextAccessor();
            services.Configure<PoliciesOptions>(Configuration.GetSection("Policies"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SampleData.Initialize(app);
        }      
    }
}
