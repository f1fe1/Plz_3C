using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookByte.AsyncDataServices;
using BookByte.Data;
using BookByte.EventProcessing;
using BookByte.SyncDataServices;
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

namespace CommandsService
{
    public class Startup
    {
        // Конструктор, який приймає об'єкт конфігурації
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

         // Властивість для доступу до конфігурації
        public IConfiguration Configuration { get; }

        // Конфігурація служб (services) для Dependency Injection
        public void ConfigureServices(IServiceCollection services)
        {
            // Додаємо контекст бази даних InMemory
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMen"));
            services.AddScoped<IBookByteRepo, BookByteRepo>();  // Додаємо реєстр репозиторію для доступу до даних
            services.AddControllers();// Додаємо контролери ASP.NET Core MVC

            services.AddHostedService<MessageBusSubscriber>();// Додаємо фоновий сервіс для підписки на події через Message Bus

            services.AddSingleton<IEventProcessor, EventProcessor>();// Додаємо один об'єкт EventProcessor, який буде використовуватися під час обробки подій
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());// Додаємо AutoMapper для мапінгу об'єктів
            services.AddScoped<IBookByteData, BookByteData>();// Додаємо сервіс для роботи з даними BookByte
            // Додаємо Swagger для документації API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookByte", Version = "v1" });
            });
        }

       // Конфігурація додатка
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Перевіряємо, чи додаток запущений в режимі розробки
            if (env.IsDevelopment())
            {
                // Додаємо сторінку помилок для розробників та налаштування Swagger
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookByte v1"));
            }

            // Налаштування HTTPS переадресації (закоментовано)
            //app.UseHttpsRedirection();

            // Налаштування маршрутизації, авторизації та кінцевих точок
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Підготовка бази даних (заповнення початковими даними)
            PrepDb.PrepPopulation(app);
        }
    }
}
