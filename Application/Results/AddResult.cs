using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Results
{
   
    public class AddResult
    {
        // Клас команди, який не повертає результат, реалізує інтерфейс IRequest.
        public class Command : IRequest
        {
            public ExperimentResult ExperimentResult { get; set; } // Параметр команди - результат експерименту для додавання.
        }

        // Обробник команди, реалізує інтерфейс IRequestHandler<Command>.
        public class Handler : IRequestHandler<Command>
        {
            private readonly ABContext _context; // Контекст бази даних.

            // Конструктор, який отримує контекст бази даних через внедрення залежностей.
            public Handler(ABContext context)
            {
                _context = context;
            }

            // Метод для обробки команди на додавання результату експерименту.
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                try // Обробка винятків зв'язаних з помилкою під час звернення до бд
                {
                    // Виконання SQL-запиту до бази даних для додавання результату експерименту.
                    await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC AddResult {request.ExperimentResult.ExperimentId}, {request.ExperimentResult.DeviceToken}, {request.ExperimentResult.Value}", cancellationToken);

                    // Збереження змін у базі даних.
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при виконанні SQL-запиту: {ex.Message}");
                }
            }
        }
    }
}
