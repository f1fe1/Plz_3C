using System.Windows.Input;
using BookByte.Models;
using BookByte.SyncDataServices;

namespace BookByte.Data
{
    //Оголошує статичний клас PrepDb, який містить методи для підготовки та заповнення бази даних.
    public static class PrepDb
    {
        //Цей метод викликається для підготовки та заповнення бази даних. 
        //Використовує об'єкт IApplicationBuilder, який є частиною ASP.NET Core для конфігурування додатку.
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            //дозволяє використовувати ін'єкцію залежностей для отримання сервісів.
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IBookByteData>();

                var bookbytes = grpcClient.ReturnAllBookByte();

                //передаються сервіс репозиторію IBookByteRepo і дані, отримані від GRPC клієнта.
                SeedData(serviceScope.ServiceProvider.GetService<IBookByteRepo>(), bookbytes);    
            }
        }

        //Цей метод використовує переданий repo (репозиторій) для створення записів 
        //в базі даних на основі отриманих bookbytes (переданий через GRPC).
        private static void SeedData(IBookByteRepo repo, IEnumerable<BookBytes> bookbytes)
        {
            Console.WriteLine("Seeding new platforms...");

            //Ітерується по bookbytes, перевіряє, чи існує запис з таким ISBN у базі даних 
            foreach(var plat in bookbytes)
            {
                // якщо ні, то створює новий запис
                if(!repo.ExternalBookByteExist(plat.ISBN))
                {
                    repo.CreateBookByte(plat);
                }
                // для збереження змін.
                repo.SaveChanges();
            }
        }
    } 
}