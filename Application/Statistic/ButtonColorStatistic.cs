using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Statistic;

public class ButtonColorStatistic
{
    public class Query : IRequest<string> { }

    public class Handler : IRequestHandler<Query, string>
    {
        private readonly AbContext _apiContext; // Контекст бази даних.

        // Конструктор, який отримує контекст бази даних через внедрення залежностей.
        public Handler(AbContext context)
        {
            _apiContext = context;
        }

        public async Task<string> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _apiContext.Statistics.FromSqlRaw($"ButtonStat").ToListAsync(cancellationToken);

            return result.FirstOrDefault()?.Statistic.Replace("},", "},\n");;
        }
    }
        
        
}