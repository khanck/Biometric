using AspNetCoreRateLimit;
using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

using Refit;
using Serilog;
using System.Reflection;
using TCC.Payment.Data.Context;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using TCC.Biometric.Payment.Handlers;
using TCC.Biometric.Payment.Logging;

using ILogger = Serilog.ILogger;
using TCC.Payment.Integration.Config;
using TCC.Payment.Integration.Interfaces;
using TCC.Payment.Integration.Biometric;
using TCC.Biometric.Payment.Config;

namespace TCC.Biometric.Payment
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        private ILogger _logger;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.EnableEndpointRouting = false);
            //if (_configuration.GetConnectionString("ConnecetType") == "Oracle")
            //{
            //    services.AddDbContext<AppDbContext>(options =>
            //        options.UseOracle(_configuration.GetConnectionString("OracleConnecet"),
            //        optionsBuilder => optionsBuilder.MigrationsAssembly("TCC.Payment.Migrations.Oracle"))
            //       );
            //}
            if (_configuration.GetConnectionString("ConnecetType") == "Sqlserver")
            {
                services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("SqlConnecet"),
               //options.UseInMemoryDatabase("InMemoryDb")
               optionsBuilder => optionsBuilder.MigrationsAssembly("TCC.Payment.Migrations.SqlServer"))
               );
            }        
            services.AddControllers();

            var builder = WebApplication.CreateBuilder();

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();


            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            _logger = logger;
            builder.Logging.ClearProviders();
            services.AddSingleton(logger);
            services.AddSerilog(logger);



            
            services.Configure<AlpetaConfiguration>(_configuration.GetSection("AlpetaConfiguration"));
            //services
            // .AddCore()
            // .AddInfrastructure()
            // .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // services
 
            //Add services to the container.
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IBiometricRepository,BiometricRepository >();
            services.AddScoped<IBiometricVerificationRepository,BiometricVerificationRepository >();
            services.AddScoped<IBusinessRepository,BusinessRepository >();
            services.AddScoped<IPaymentCardRepository,PaymentCardRepository >();
            services.AddScoped<ITransactionRepository,TransactionRepository >();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            services.AddScoped<IAlpetaServer, AlpetaServer>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.ExceptionHandler(_logger);
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id?}");
            });

        }               

    }
}
