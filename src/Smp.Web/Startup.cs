using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Smp.Web.Factories;
using Smp.Web.Repositories;
using Smp.Web.Services;
using Smp.Web.Validators;
using Smp.Web.Wrappers;

namespace Smp.Web
{
    [ExcludeFromCodeCoverage]
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
            services.AddAuthentication(jwt =>
            {
                jwt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                jwt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["Tokens:SigningKey"]))
                };
            });

            services.AddAuthorization();

            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IFileWrapper, FileWrapper>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISmtpClient>(smtp => new SmtpClientWrapper(Configuration["Mail:Host"], ushort.Parse(Configuration["Mail:Port"])));

            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IRelationshipsRepository, RelationshipsRepository>();
            services.AddTransient<IRequestsRepository, RequestsRepository>();
            services.AddTransient<IPostsRepository, PostsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IActionsRepository, ActionsRepository>();
            services.AddTransient<IMessagesRepository, MessagesRepository>();
            services.AddTransient<IConversationsRepository, ConversationsRepository>();

            services.AddTransient<IUserValidator, UserValidator>();
            services.AddTransient<IRequestValidator, RequestValidator>();
            services.AddTransient<IActionValidator, ActionValidator>();

            services.AddTransient<IRelationshipsService, RelationshipsService>();
            services.AddTransient<IRequestsService, RequestsService>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IAccountsService, AccountsService>();
            services.AddTransient<IConversationsService, ConversationsService>();
            services.AddTransient<IMessagesService, MessagesService>();

            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SMP API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
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
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SMP API v1");
            });

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
            
                spa.Options.SourcePath = "ClientApp";
            
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
