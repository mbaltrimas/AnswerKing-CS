using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Answer.King.Api.Services;
using Answer.King.Domain.Repositories;
using Answer.King.Infrastructure;
using Answer.King.Infrastructure.Extensions.DependancyInjection;
using Answer.King.Infrastructure.Repositories;
using Answer.King.Infrastructure.Repositories.Mappings;
using Answer.King.Infrastructure.SeedData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;

namespace Answer.King.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAnyOrigin",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(
                    fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.ConfigureLiteDb(options =>
                {
                    options.RegisterEntityMappingsFromAssemblyContaining<IEntityMapping>();
                    options.RegisterDataSeedingFromAssemblyContaining<ISeedData>();
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Answer King API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
                options.CustomSchemaIds(x => x.FullName);
            });

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddSingleton<ILiteDbConnectionFactory, LiteDbConnectionFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((document, request) =>
                    {
                        document.Paths = document.Paths.ToDictionary(p => p.Key.ToLowerInvariant(), p => p.Value);
                    });
                });

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Answer King API V1");
                    options.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("AllowAnyOrigin");
            app.UseLiteDb();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
