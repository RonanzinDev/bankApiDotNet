using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bankApi.DAL;
using bankApi.Services.Implementations;
using bankApi.Services.Interfaces;
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

namespace bankApi
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
            services.AddDbContext<BankApiContext>(x => x.UseSqlServer(Configuration.GetConnectionString("BankApiDbConnection")));
            services.AddControllers();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "bankApi",
                    Version = "v1",
                    Description = "Bank Api for studies",
                    Contact = new OpenApiContact
                    {
                        Name = "Gabriel Ronan",
                        Email = "morellianogm@gmail.com",
                        Url = new Uri("https://github.com/ronanzindev")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "bankApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                var prefix = string.IsNullOrWhiteSpace(x.RoutePrefix) ? "." : "..";
                x.SwaggerEndpoint($"{prefix}/swagger/v2/swagger.json", "bankApi doc");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
