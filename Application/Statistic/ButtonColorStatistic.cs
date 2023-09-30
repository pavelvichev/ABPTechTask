using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            var statistic = result.FirstOrDefault()?.Statistic;
            
            JObject json = JObject.Parse(statistic!);
            
            string formattedJson = json.ToString(Formatting.Indented);
            
            return formattedJson;
        }
    }
        
        
}