using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic;
using RBAT.Logic.Interfaces;
using RBAT.Web.Services;
using System;

namespace RBAT.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationService(this IServiceCollection services)
        {
            services.AddScoped<RBATDbInitializer>();

            services.AddTransient<DropDownService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<ITimeNaturalFlowService, TimeNaturalFlowService>();
            services.AddScoped<ITimeWaterUseService, TimeWaterUseService>();
            services.AddScoped<ITimeHistoricLevelService, TimeHistoricLevelService>();
            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<IClimateStationService, ClimateStationService>();
            services.AddScoped<ITimeClimateDataService, TimeClimateDataService>();
            services.AddScoped<INetEvaporationService, NetEvaporationService>();
            services.AddScoped<ITimeStorageCapacityService, TimeStorageCapacityService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IChannelTravelTimeService, ChannelTravelTimeService>();
            services.AddScoped<IChannelOutflowCapacityService, ChannelOutflowCapacityService>();
            services.AddScoped<IRecordedFlowStationService, RecordedFlowStationService>();
            services.AddScoped<ITimeRecordedFlowService, TimeRecordedFlowService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IChannelZoneLevelService, ChannelZoneLevelService>();
            services.AddScoped<IChannelZoneWeightService, ChannelZoneWeightService>();
            services.AddScoped<IChannelPolicyGroupService, ChannelPolicyGroupService>();
            services.AddScoped<IChannelPolicyGroupChannelService, ChannelPolicyGroupChannelService>();
            services.AddScoped<IScenarioService, ScenarioService>();
            services.AddScoped<INodeZoneLevelService, NodeZoneLevelService>();
            services.AddScoped<INodeZoneWeightService, NodeZoneWeightService>();
            services.AddScoped<INodePolicyGroupService, NodePolicyGroupService>();
            services.AddScoped<INodePolicyGroupNodeService, NodePolicyGroupNodeService>();
            services.AddScoped<IStartingReservoirLevelService, StartingReservoirLevelService>();
            services.AddScoped<ISeasonalReservoirLevelService, SeasonalReservoirLevelService>();
            services.AddScoped<ISeasonalDiversionLicenseService, SeasonalDiversionLicenseService>();
            services.AddScoped<ISeasonalApportionmentTargetService, SeasonalApportionmentTargetService>();
            services.AddScoped<ISeasonalWaterDemandService, SeasonalWaterDemandService>();
            services.AddScoped<ICustomTimeStepService, CustomTimeStepService>();
            services.AddScoped<IOptimalSolutionsService, OptimalSolutionsService>();
            services.AddScoped<IChannelZoneLevelRecordedFlowStationService, ChannelZoneLevelRecordedFlowStationService>();
            services.AddScoped<INodeZoneLevelHistoricReservoirLevelService, NodeZoneLevelHistoricReservoirLevelService>();
            //Accessing HTTPContext in locations where it's not directly available, out of MVC Controllers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void ConfigureCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options => {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(180);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }
    }
}
