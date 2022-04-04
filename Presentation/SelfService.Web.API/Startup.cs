using InvestTeam.AutoBox.Application;
using InvestTeam.AutoBox.Application.Common.Interfaces;
using InvestTeam.AutoBox.Infrastructure.Externals;
using InvestTeam.AutoBox.Infrastructure.Persistence;
using InvestTeam.AutoBox.SelfService.Web.API;
using InvestTeam.AutoBox.SelfService.Web.API.Configuration;
using InvestTeam.AutoBox.SelfService.Web.API.Context;
using InvestTeam.AutoBox.SelfService.Web.API.Helper;
using InvestTeam.AutoBox.SelfService.Web.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SelfService.Web.API
{
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapperConfigurations(this IServiceCollection services)
        {
            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.CreateMap<Soure, Dest>()
            //        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Sip));             
            //}, typeof(Soure), typeof(Dest));
        }
    }

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
            services.AddAutoMapperConfigurations();

            services.AddScoped<ISource, WebAPICommandSource>();
            services.AddScoped<UserHelper>();
            services.AddScoped<VechicleHelper>();
            services.AddScoped<OrderHelper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services
                .AddExternalsDI()
                .AddPersistenceDI(Configuration.GetConnectionString("DefaultConnection"))
                .AddApplicationDI();

            services.AddDbContext<AppIdentityDbContext>(opts =>
            {
                opts.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"],
                    opts => opts.MigrationsAssembly("SelfService.Web.API")
                );
            });

            services.AddControllers();

            services.AddSwaggerConfiguration();

            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireDigit = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
                {
                    opts.TokenValidationParameters.ValidateAudience = false;
                    opts.TokenValidationParameters.ValidateIssuer = false;
                    opts.TokenValidationParameters.IssuerSigningKey
                        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            Configuration["BearerTokens:Key"]));
                });

            services.ConfigureApplicationCookie(opts =>
            {
                opts.Events.DisableRedirectionForApiClients();
            });

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(config =>
            {
                string endPointName =
                    string.Format("{0} {1}", "Self Service API", "1.0");
                config.SwaggerEndpoint("/swagger/v1/swagger.json", endPointName);
                config.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateDatabase();
            app.SeedUserStore();
        }
    }
}
