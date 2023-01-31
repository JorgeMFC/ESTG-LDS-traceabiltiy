global using MyTrace.Models;
global using Microsoft.EntityFrameworkCore;
global using MyTrace.Domain.Exceptions;
global using MyTrace.Domain.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyTrace.Utils;
using Microsoft.OpenApi.Models;
using MyTrace.Filters;
using Microsoft.Extensions.FileProviders;

namespace MyTrace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "AllowAnyCorsPolicy";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerHeaderFilter>();
            });

            //Database
            builder.Services.AddDbContext<MyTraceContext>();

            //API access policies
            builder.Services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //Add header
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:JWT:ScretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddSingleton<JwtAuthenticationManager>();

            //Set types env's
            builder.Services.Configure<PinataEnv>(builder.Configuration.GetSection("AppSettings:Blockchain:Pinata"));
            builder.Services.Configure<CryptocurrencyBrokerageEnv>(builder.Configuration.GetSection("AppSettings:Blockchain:CryptocurrencyBrokerage"));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                option => {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTrace API", Version = "v1" });
                    option.OperationFilter<Filters.ReApplyOptionalRouteParameterOperationFilter>(); 
                }
            ); 
            builder.Services.AddDbContext<MyTraceContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure public folder
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/public"
            });

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
