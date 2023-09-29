using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;


namespace Application.Experiments
{
   
    public class FindExperiment
    {
        // Клас запиту, реалізує інтерфейс IRequest<Experiment>, де Experiment - це тип результату запиту.
        public class Query : IRequest<Experiment>
        {
            public string Key { get; set; } // Параметр запиту - ключ експерименту.
        }

        // Обробник запиту, реалізує інтерфейс IRequestHandler<Query, Experiment>.
        public class Handler : IRequestHandler<Query, Experiment>
        {
            private readonly ABContext _apiContext; // Контекст бази даних.

            // Конструктор, який отримує контекст бази даних через внедрення залежностей.
            public Handler(ABContext context)
            {
                _apiContext = context;
            }

            // Метод для обробки запиту на пошук експерименту за ключем.
            public async Task<Experiment> Handle(Query request, CancellationToken cancellationToken)
            {
                try // Обробка винятків зв'язаних з помилкою під час звернення до бд
                {
                    // Виконання SQL-запиту до бази даних для пошуку експерименту за ключем.
                    var experiment = await _apiContext.Experiments.FromSqlInterpolated($"FindExperiment {request.Key}")
                        .ToListAsync(cancellationToken);

                    // Повертаємо перший результат запиту (або null, якщо результат не знайдений).
                    return experiment.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при виконанні SQL-запиту: {ex.Message}");
                    throw;
                }
            }
        }
    }
}

