using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Momkn.Core.Identity;
using Momkn.Core.Interfaces;
using Momkn.Core.Interfaces.MainInterface;
using Momkn.Core.Interfaces.UserInterface;
using Momkn.Infrastructure.Data;
using Momkn.Infrastructure.Repositories;
using Momkn.Infrastructure.Repositories.MainRepositories;
using Momkn.Infrastructure.Repositories.UserRepositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momkn.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // 

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            services.AddDbContext<MomknDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity with custom settings [user & usermanager]
            services.AddIdentity<ApplicationUser, IdentityRole>()
                //.AddUserManager<ApplicationUserManager>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MomknDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
             .AddCookie(options =>
             {
                 options.SlidingExpiration = true;
             })
             .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidIssuer = Configuration["Tokens:Issuer"],
                     ValidAudience = Configuration["Tokens:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                     RequireExpirationTime = false,
                     ValidateLifetime = true,
                 };
             });

            var list = new List<string>();
            Configuration.GetSection("AllowedHosts").Bind(list);

            services.AddCors(options => {
                options.AddPolicy(MyAllowSpecificOrigins, builder => {
                    builder.WithOrigins(list.ToArray()).SetIsOriginAllowed(x => _ = true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

         
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    // Configure JSON to ignore reference loop
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  
                });

            // Enable MVC compatibility
            services.AddMvc(options =>
            {
                // Add authorize global filter using dual authorization mode [cookie based identity & bearer]
                var defaultPolicy = new AuthorizationPolicyBuilder(new[] { IdentityConstants.ApplicationScheme, JwtBearerDefaults.AuthenticationScheme })
                        .RequireAuthenticatedUser()
                        .Build();
                // options.Filters.Add(new AuthorizeFilter(defaultPolicy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Allow special chars for Identity Username [email chars]
            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
                options.User.RequireUniqueEmail = true;
            });


            //services.();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserResolverService>();
            services.AddTransient<IDBInitializer, DBInitializer>();
            services.AddTransient<IStepRepository, StepRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
          

            
        
            services.AddTransient<IApplicationUserTokenRepository, ApplicationUserTokenRepository>();
         
            services.AddTransient<IApplicationUserTokenRepository, ApplicationUserTokenRepository>();
            services.AddTransient<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
    

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TeleMedicine.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });
            services.AddOptions();
            services.AddHttpClient();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDBInitializer dbInitializer)
        {
            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

          
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbInitializer.Initialize();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Momkn.API v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
