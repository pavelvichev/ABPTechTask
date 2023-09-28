using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Statistic.Results
{
    public class ExperimentResultsCount
    {
        // Клас запиту, реалізує інтерфейс IRequest<int>
        public class Query : IRequest<int>
        {
            public int ExperimentId { get; set; } // Параметр запиту - ідентифікатор експерименту.
        }

        // Обробник запиту, реалізує інтерфейс IRequestHandler<Query, int>.
        public class Handler : IRequestHandler<Query, int>
        {
            private readonly ABContext _apiContext; // Контекст бази даних.

            // Конструктор, який отримує контекст бази даних через внедрення залежностей.
            public Handler(ABContext context)
            {
                _apiContext = context;
            }

            // Метод для обробки запиту на отримання кількості результатів експерименту.
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                try // Обробка винятків зв'язаних з помилкою під час звернення до бд
                {
                    // Виконання SQL-запиту до бази даних для отримання кількості результатів експерименту за його ідентифікатором.
                    var result = await _apiContext.Database.SqlQuery<int>($"EXEC PriceExprerimntsCount {request.ExperimentId}").ToListAsync(cancellationToken);

                    // Повертаємо перший результат запиту (або 0, якщо результати відсутні).
                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при виконанні SQL-запиту: {ex.Message}");
                    return 0;
                }
            }
        }
    }
}

