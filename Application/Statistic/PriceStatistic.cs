using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using System.Text.Json;

namespace Application.Statistic;

public class PriceStatistic
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
            var result = await _apiContext.Statistics.FromSqlRaw($"PriceStat").ToListAsync(cancellationToken);

            return result.FirstOrDefault()?.Statistic.Replace("},", "},\n");
        }
    }
}