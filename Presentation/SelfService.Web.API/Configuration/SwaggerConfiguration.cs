namespace InvestTeam.AutoBox.SelfService.Web.API.Configuration
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services/*, IConfiguration appConf*/)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(config =>
            {
                // add JWT Authentication
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                config.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                {
                                                    {securityScheme , new string[] { }}
                                                });

                // add Basic Authentication
                var basicSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                };
                config.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                {
                                                    {basicSecurityScheme, new string[] { }}
                                                });

                config.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "SelfService Web API", //appConf[""],
                        Version = "1.0" //appConf[""]
                    });

                //OpenApiSecurityScheme scheme = new OpenApiSecurityScheme()
                //{
                //    Description = "Authorization header using Api Key",
                //    Name = "X-API-KEY",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "ApiKey"
                //};

                //config.AddSecurityDefinition("ApiKey", scheme);

                //config.AddSecurityRequirement(new OpenApiSecurityRequirement
                //    {
                //        {
                //            new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                //            },
                //            new[] { "readAccess", "writeAccess" }
                //        }
                //    });
            });
        }
    }
}
