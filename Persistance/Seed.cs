using Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistance
{
    public class Seed
    {
        public static async Task SeedData(ABContext context)
        {
            if (context.Experiments.Any()) return;

            var experiments = new List<Experiment>
            {
                new Experiment { Key = "button_color", Options = "#FF0000,#00FF00,#0000FF"},
                new Experiment { Key = "price", Options = JsonSerializer.Serialize(new Dictionary<int, double>
                {
                    { 10, 0.75 }, // 75%
                    { 20, 0.10 }, // 10%
                    { 50, 0.05 }, // 5%
                    { 5, 0.10 }   // 10%
                })}

            };
            await context.Experiments.AddRangeAsync(experiments);
            await context.SaveChangesAsync();
        }
    }
}
