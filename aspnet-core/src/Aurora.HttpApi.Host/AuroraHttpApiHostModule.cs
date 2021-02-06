using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Aurora.EntityFrameworkCore;
using Aurora.MultiTenancy;
using Aurora.Services.Hosts;
using Aurora.Services.Tenants;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AuroraApplicationModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AuroraEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpAspNetCoreSerilogModule)
    )]
    public class AuroraHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureAuthorization(context);
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var environment = context.Services.GetHostingEnvironment();
            if (environment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AuroraDomainSharedModule>(Path.Combine(environment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Aurora.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuroraDomainModule>(Path.Combine(environment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Aurora.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuroraApplicationContractsModule>(Path.Combine(environment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Aurora.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuroraApplicationModule>(Path.Combine(environment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Aurora.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(AuroraApplicationModule).Assembly, settings =>
                {
                    settings.RootPath = "aurora";
                    settings.TypePredicate = type => !type.IsAbstract && type.IsSubclassOf(typeof(AuroraTenantAppService));
                });
                options.ConventionalControllers.Create(typeof(AuroraApplicationModule).Assembly, settings =>
                {
                    settings.RootPath = "aurora/host";
                    settings.TypePredicate = type => !type.IsAbstract && type.IsSubclassOf(typeof(AuroraHostAppService));
                });
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });
        }

        private void ConfigureAuthorization(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization(options =>
            {
                options.AddPolicy("aurora-api", policy =>
                {
                    policy.RequireClaim(JwtClaimTypes.Scope, "aurora-api");
                });
                options.AddPolicy("aurora-host-api", policy =>
                {
                    policy.RequireClaim(JwtClaimTypes.Scope, "aurora-host-api");
                });
            });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("aurora", new OpenApiInfo {Title = "Aurora Api", Version = "v1"});
                options.SwaggerDoc("host", new OpenApiInfo {Title = "Aurora Host Api", Version = "v1"});
                options.DocInclusionPredicate((docName, description) => (description.GroupName?.StartsWith("Abp") ?? false) || docName == description.GroupName);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey});
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{new OpenApiSecurityScheme {Reference = new OpenApiReference {Id = "Bearer", Type = ReferenceType.SecurityScheme}}, Array.Empty<string>()}});
            });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.EnableFilter();
                options.EnableDeepLinking();
                options.DisplayRequestDuration();
                options.DefaultModelsExpandDepth(-1);
                options.ConfigObject.AdditionalItems.Add("persistAuthorization", true);
                options.SwaggerEndpoint("/swagger/aurora/swagger.json", "Aurora Api");
                options.SwaggerEndpoint("/swagger/host/swagger.json", "Aurora Host Api");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}