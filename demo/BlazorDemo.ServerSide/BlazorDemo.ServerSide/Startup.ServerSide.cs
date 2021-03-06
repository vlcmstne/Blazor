using System.Net.Http;
using BlazorDemo.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorDemo.DataProviders;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.ServerSide {
    
    partial class Startup {
        public override string EnvironmentName => "ServerSide";
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services) {
            services.AddServerSideBlazor();

            var optionsBuilder = services.AddOptions();
            optionsBuilder.AddOptions<SeoConfiguration>("BlazorDemo");
            optionsBuilder.AddOptions<SeoConfiguration>("BlazorDemo.Reporting");
            
            
            services.AddScoped<HttpClient>(serviceProvider => serviceProvider.GetService<IHttpClientFactory>().CreateClient());
            
            services.AddScoped<IContosoRetailDataProvider, ContosoRetailDataProvider>();
            services.AddScoped<IRentInfoDataProvider, RentInfoDataProvider>();

            services.AddDbContext<FMRDemoContext>(options => options.UseSqlServer(GetConnectionString("GridLargeDataConnectionString")));

            services.AddDbContext<ContosoRetailContext>(options => options.UseSqlServer(GetConnectionString("PivotGridLargeDataConnectionString")));

            string GetConnectionString(string name) {
                return context.Configuration.GetConnectionString(name);
            }
        }
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                    app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints => endpoints.MapBlazorHub());
        }
    }
}
