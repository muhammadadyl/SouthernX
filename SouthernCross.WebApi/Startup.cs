using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SouthernCross.Core.Configs;
using SouthernCross.Core.Entities;
using SouthernCross.Core.Services;
using SouthernCross.Core.Services.HelperServices;
using SouthernCross.WebApi.Filters;
using System.Threading.Tasks;

namespace SouthernCross.WebApi
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

            services.AddControllers(options =>
            {
                options.Filters.Add<UnhandledExceptionFilter>();
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.RegisterMiddleware = true;
            })
            .AddVersionedApiExplorer(options => { options.SubstituteApiVersionInUrl = true; });

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            var southernXDatabaseOptionsSections = Configuration.GetSection(nameof(SouthernXDatabaseOptions));
            services.Configure<SouthernXDatabaseOptions>(southernXDatabaseOptionsSections);

            var southernXDatabaseOptions = southernXDatabaseOptionsSections.Get<SouthernXDatabaseOptions>();
            services.AddSingleton(southernXDatabaseOptions);

            var sampleDataOptionsSections = Configuration.GetSection(nameof(SampleDataOptions));
            services.Configure<SampleDataOptions>(sampleDataOptionsSections);

            var sampleDataOptions = sampleDataOptionsSections.Get<SampleDataOptions>();
            services.AddSingleton(sampleDataOptions);

            ConfigureDbServices(services);
        }

        protected virtual void ConfigureDbServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IMemberService), typeof(MemberService));
            services.AddSingleton(typeof(ISampleData<Member>), typeof(MemberService));
            services.AddSingleton(typeof(ISimpleMemoryCache), typeof(SimpleMemoryCache));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var sampleDataService = app.ApplicationServices.GetRequiredService<ISampleData<Member>>();
                Task.Run(() => {
                        sampleDataService.LoadDataAsync();                   
                });
            }

            app.UseSerilogRequestLogging();

            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
