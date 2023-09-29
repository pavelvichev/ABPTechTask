using Application.Experiments;
using Application.Results;
using Application.Statistic.Results;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Application.Statistic;

namespace ABPTechTask.Controllers
{
    [Route("experiment")]
    public class ExperimentsController : ControllerBase
    {
        // Поле для зберігання посередника (Mediator).
        private readonly IMediator _mediator;
        private readonly ILogger<ExperimentsController> _logger;

        // Конструктор контролера, що використовує внедрення залежностей для отримання посередника (Mediator).
        public ExperimentsController(IMediator mediator, ILogger<ExperimentsController> logger)
        {
            // Ініціалізація поля _mediator отриманим посередником (Mediator).
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("button-color")]
        public async Task<IActionResult> ButtonColorExperiment([FromQuery(Name = "device-token")] string deviceToken)
        {
            try
            {
                // Отримання інформації про експеримент з бази даних за ключем "button_color" за допомогою Mediator.
                var experiment = await _mediator.Send(new FindExperiment.Query { Key = "button_color" });

                // Пошук результату експерименту за токеном пристрою.
                var existingResult = await _mediator.Send(new FindResult.Query { DeviceToken = deviceToken });

                if (existingResult != null)
                {
                    // Якщо пристрій вже брав участь у експерименті
                    if (existingResult.ExperimentId != experiment.Id)
                    {
                        _logger.LogInformation("Device already participating");
                        return Ok("Пристрій вже бере участь в експерименті");
                    }
                    // Якщо пристрій вже взяв участь в цьому експерименті
                    else if (existingResult.ExperimentId == experiment.Id)
                    {
                        _logger.LogInformation("Show Result for existing device");
                        return Ok(new Dictionary<string, string> { { "key", "button_color" }, { "value", existingResult.Value } });
                    }
                }

                // Розділення рядка опцій на окремі варіанти.
                var options = experiment.Options.Split(',');

                // Генерація випадкового індексу для вибору опції.
                var random = new Random();
                var selectedOption = options[random.Next(options.Length)];

                // Створення об'єкта результату експерименту.
                var result = new ExperimentResult
                {
                    ExperimentId = experiment.Id,
                    DeviceToken = deviceToken.ToLower(), // Перетворення токену до нижнього регістру.
                    Value = selectedOption
                };

                // Збереження результату в базі даних за допомогою Mediator.
                await _mediator.Send(new AddResult.Command { ExperimentResult = result });

                // Повертаємо результат експерименту.
                _logger.LogInformation("Successfully added");
                return Ok(new Dictionary<string, string> { { "key", "button_color" }, { "value", selectedOption } });
            }
            catch (Exception ex)
            {   // Повертаємо результат експерименту з текстом помилки.
                return  BadRequest(ex.Message);
            }
            
        }


        [HttpGet]
        [Route("price")]
        public async Task<IActionResult> PriceExperiment([FromQuery(Name = "device-token")] string deviceToken)
        {
            try
            {
                // Отримання експерименту з бази даних за ключем "price" за допомогою Mediator
                var experiment = await _mediator.Send(new FindExperiment.Query { Key = "price" });

                // Пошук результату експерименту за токеном пристрою
                var existingResult = await _mediator.Send(new FindResult.Query { DeviceToken = deviceToken });

                // Перевірка наявності результату
                if (existingResult != null)
                {
                    // Якщо пристрій вже брав участь у експерименті
                    if (existingResult.ExperimentId != experiment.Id)
                    {
                        _logger.LogInformation("Device already participating");
                        return Ok("Пристрій вже бере участь в експерименті");
                    }
                    // Якщо пристрій вже взяв участь в цьому експерименті
                    else if (existingResult.ExperimentId == experiment.Id)
                    {
                        _logger.LogInformation("Show Result for existing device");
                        return Ok(new Dictionary<string, string> { { "key", "price" }, { "value", existingResult.Value } });
                    }
                }

                // Генерація випадкового числа
                var random = new Random();
                var randomNumber = random.NextDouble();

                var selectedPrice = 0;

                // Розпакування опцій експерименту з JSON формату
                var options = JsonSerializer.Deserialize<Dictionary<int, double>>(experiment.Options);

                // Вибір ціни на основі випадкового числа та ймовірностей
                foreach (var option in options)
                {
                    if (randomNumber <= option.Value)
                    {
                        selectedPrice = option.Key;
                        break;
                    }

                    randomNumber -= option.Value;
                }

                // Створення об'єкта результату експерименту
                var result = new ExperimentResult
                {
                    ExperimentId = experiment.Id,
                    DeviceToken = deviceToken.ToLower(),
                    Value = selectedPrice.ToString(),
                };

                // Збереження результату в базі даних за допомогою Mediator
                await _mediator.Send(new AddResult.Command { ExperimentResult = result });
                
                _logger.LogInformation("Successfully added");
                return Ok(new Dictionary<string, string> { { "key", "price" }, { "value", selectedPrice.ToString() } });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error : {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("statistic")]
        public async Task<IActionResult> GetStatistic()
        {
            try
            {
                // Отримання інформації про експеримент з кнопкою кольору з бази даних за допомогою Mediator.
                var experimentButtonColor = await _mediator.Send(new FindExperiment.Query { Key = "button_color" });

                // Отримання інформації про експеримент з ціною з бази даних за допомогою Mediator.
                var experimentPrice = await _mediator.Send(new FindExperiment.Query { Key = "price" });

                // Отримання кількості результатів для експерименту з кнопкою кольору.
                var experimentResultsCount = await _mediator.Send(new ExperimentResultsCount.Query());

                // Отримання детальної інформації для кожного експерименту
                var buttonStat = await _mediator.Send(new ButtonColorStatistic.Query());
                var priceStat = await _mediator.Send(new PriceStatistic.Query());

                // Створення словника, що містить статистику експериментів.
                var statistic ="button_color : [ \n" +  buttonStat + " \n],\n" + "price : [\n" + priceStat + "\n],\n" + "\nTotal Devices : " + experimentResultsCount;

                // Повертаємо статистику у форматі JSON.
                _logger.LogInformation($"Statistic {statistic}");
                
                return Ok(statistic);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
