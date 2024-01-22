using AutoMapper;
using BookCollection.Data;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookCollection.SyncDataServices
{
    //Наслідується від GrpcBooks.GrpcBooksBase, що вказує на те, що це служба gRPC.s
    public class GrpcBookCollection : GrpcBooks.GrpcBooksBase
    {
        //Містить приватні поля _repository та _mapper для зберігання екземплярів
        // репозиторію книг і AutoMapper відповідно.
        private readonly IBookRepo _repository;
        private readonly IMapper _mapper;


        //Приймає екземпляри IBookRepo та IMapper в якості параметрів.
        //Ініціалізує приватні поля.
        public GrpcBookCollection(IBookRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //Обробляє запит на отримання всіх книг. Створює новий екземпляр BooksResponse.
        public virtual Task<BooksResponse> GetAllBooks(GetAllRequest request, ServerCallContext context)
        {
            //Викликає метод GetAllBook на репозиторії, щоб отримати список книг.
            var response = new BooksResponse();
            var books = _repository.GetAllBook();
            
            //Використовує AutoMapper для відображення книг і додає їх до відповіді. 
            response.Platform.AddRange(_mapper.Map<IEnumerable<BookCollection.GrpcBookCollection>>(books));
            //Повертає завершене завдання із заповненою відповіддю.
            return Task.FromResult(response);
        }
    }
}
