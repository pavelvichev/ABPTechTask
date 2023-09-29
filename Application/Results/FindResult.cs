using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Results
{
  
    public class FindResult
    {
        // Клас запиту, реалізує інтерфейс IRequest<ExperimentResult>
        public class Query : IRequest<ExperimentResult>
        {
            public string DeviceToken { get; set; } // Параметр запиту - токен пристрою.
        }

        // Обробник запиту, реалізує інтерфейс IRequestHandler<Query, ExperimentResult>.
        public class Handler : IRequestHandler<Query, ExperimentResult>
        {
            private readonly ABContext _apiContext; // Контекст бази даних.

            // Конструктор, який отримує контекст бази даних через внедрення залежностей.
            public Handler(ABContext context)
            {
                _apiContext = context;
            }

            // Метод для обробки запиту на пошук результату експерименту за токеном пристрою.
            public async Task<ExperimentResult> Handle(Query request, CancellationToken cancellationToken)
            {
                try // Обробка винятків зв'язаних з помилкою під час звернення до бд
                {
                    // Виконання SQL-запиту до бази даних для пошуку результату експерименту за токеном пристрою.
                    var result = await _apiContext.ExperimentResults
                        .FromSqlInterpolated($"FindResult {request.DeviceToken}").FirstOrDefaultAsync(cancellationToken);

                    // Повертаємо перший результат запиту (або null, якщо результат не знайдений).
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при виконанні SQL-запиту: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
