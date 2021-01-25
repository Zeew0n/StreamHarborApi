using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StreamHarborAPI.Services;
using System.Text;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Application.MappingProfile;
using WorkFlowTaskManager.Application.Services;
using WorkFlowTaskManager.Application.Services.CurrentUserService;
using WorkFlowTaskManager.Domain.Models;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Infrastructure.Identity.Services;
using WorkFlowTaskManager.Infrastructure.Persistance.Data;
using WorkFlowTaskManager.Infrastructure.Persistence.Repository;
using WorkFlowTaskManager.Infrastructure.Shared.Services;
using WorkFlowTaskManager.Services;

namespace WorkFlowTM.WebAPI
{
    public class Startup
    {

        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString =  Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddControllers();

            var origins = Configuration["CorsUrl"].Split(",");
            services.AddCors(options =>
            {
                options.AddPolicy("BasePolicy",
                builder =>
                {
                    builder

                    .AllowAnyOrigin()
                     .AllowAnyMethod()
                    .AllowAnyHeader();
                   
                });

            });
            
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = Configuration["AuthSettings:Issuer"];
                options.Audience = Configuration["AuthSettings:Audience"];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            services.AddAuthentication(auth =>
            {


                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = Configuration["AuthSettings:Issuer"];
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthSettings:Audience"],
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = _signingKey,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
                options.SaveToken = true;
            });
            services.AddAuthorization();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserAuthService, UserAuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleServices>();
            services.AddTransient<IRoleServices,RoleService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMailService, SendGridMailService>();
            //services.AddTransient<IMapper, IMapper>();

            //Mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StreamHarbor API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("BasePolicy");
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
