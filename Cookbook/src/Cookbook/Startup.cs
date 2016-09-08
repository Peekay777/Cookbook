using AutoMapper;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Cookbook
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
              .SetBasePath(_env.ContentRootPath)
              .AddJsonFile("config.json")
              .AddEnvironmentVariables();

            _config = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);
            services.AddLogging();
            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddDbContext<CookbookContext>();
            services.AddScoped<ICookbookRepo, CookbookRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<RecipeViewModel, Recipe>().ReverseMap();
                config.CreateMap<IngredientViewModel, Ingredient>().ReverseMap();
                config.CreateMap<InstructionViewModel, Instruction>().ReverseMap();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddConsole(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
