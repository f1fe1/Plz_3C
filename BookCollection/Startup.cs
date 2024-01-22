using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookCollection.AsyncDataServices;
using BookCollection.Data;
using BookCollection.SyncDataServices.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BookCollection
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        //IConfiguration (конфігурація) та IWebHostEnvironment (середовище веб-хоста)
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //використовуються для доступу до конфігурації додатку 
            //та інформації про середовище виконання.
            Configuration = configuration;
            _env = env;
        }

        //виконується конфігурація та реєстрація сервісів, які будуть використовуватися в додатку.
        public void ConfigureServices(IServiceCollection services)
        {

            //реєструє контекст бази даних AppDbContext та налаштовує його для використання SQL Server.
            // if(_env.IsProduction())
            // {
                Console.WriteLine("--> SqlServer db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("BookConn")));

            // }
            // else
            // {
            //     Console.WriteLine("--> Using InMem Db");
            //     services.AddDbContext<AppDbContext>(opt => 
            //         opt.UseInMemoryDatabase("InMem"));
            // }
            
            //Dependency Injection (DI) - це шаблон проектування, який дозволяє 
            //класам отримувати залежності зовні, замість того, щоб створювати їх самостійно.
            
            //Реєстрація служб через Dependency Injection.
            //інжектуються в контролери замість того, щоб створювати їх напряму.
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddHttpClient<IBookByteDataClient, HttpBookByteDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Налаштування Swagger для документації API.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookCollection", Version = "v1"});
            });

            Console.WriteLine($"--> BookByte Endpoint {Configuration["BookByte"]}");
        }


        //У цьому методі виконується налаштування обробки HTTP-запитів та середовища виконання додатку
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Включення Swagger UI та сторінки для документації API лише в режимі розробки.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookCollection v1"));
            }

            // app.UseHttpsRedirection();

            //Конфігурація обробки маршрутів та авторизації.
            app.UseRouting();

            app.UseAuthorization();

            //Визначення кінцевих точок (endpoints) для обробки HTTP-запитів.
            app.UseEndpoints(async endpoints => 
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcBookCollection>();

                endpoints.MapGet("/protos/books.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/books.proto"));
                } );
            });

            // PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
