using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.BLL.Interface;
using TestTask.BLL.Services;
using TestTask.Core.Entities;
using TestTask.Core.Repositories;
using TestTask.DAL.Repository;
using TestTaskREST.Service;

namespace TestTaskREST
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
             var server = Configuration["DbServer"] ?? "localhost";
             var port = Configuration["DbPort"] ?? "1433"; // Default SQL Server port
             var user = Configuration["DbUser"] ?? "mysql"; // Warning do not use the SA account
             var password = Configuration["Password"] ?? "Tdf-y3G-H9u-H73";
             var database = Configuration["Database"] ?? "TestTask";

             services.AddDbContext<TestTask.DAL.Data.Context>(options =>
               options.UseSqlServer($"Server={server}, {port};Initial Catalog={database};User ID={user};Password={password}"));

             //services.AddDbContext<TestTask.DAL.Data.Context>(options =>
             //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("TestTask.DAL")));

            services.AddScoped<IArticleRepository, ArticleRepository>();

            services.AddScoped<IArticleService, ArticleService>();

            var settings = new ConnectionSettings()
                .DefaultMappingFor<Article>(x => x.IndexName("articles"));


            services.AddSingleton<IElasticClient>(new ElasticClient(settings));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestTaskREST", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                try
                {
                    DatabaseManagementService.MigrationInitialisation(app);
                }
                catch(Exception ex)
                {
                     
                }
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestTaskREST v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
