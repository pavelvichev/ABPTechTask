using Application.Experiments;
using Application.Results;
using Application.Statistic.Results;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ABPTechTask.Controllers
{
    public class ExperimentsController : ControllerBase
    {
        // Поле для зберігання посередника (Mediator).
        private readonly IMediator _mediator;

        // Конструктор контролера, що використовує внедрення залежностей для отримання посередника (Mediator).
        public ExperimentsController(IMediator mediator)
        {
            // Ініціалізація поля _mediator отриманим посередником (Mediator).
            _mediator = mediator;
        }

        [HttpGet]
        [Route("experiment/button-color")]
        public async Task<IActionResult> ButtonColorExperimnt([FromQuery(Name = "device-token")] string deviceToken)
        {
            try
            {
                // Отримання інформації про експеримент з бази даних за ключем "button_color" за допомогою Mediator.
                var
                    experiment =

                        await _mediator.Send(new FindExperiment.Query { Key = "button_color" });

                // Пошук результату експерименту за токеном пристрою.
                var existingResult = await _mediator.Send(new FindResult.Query { DeviceToken = deviceToken });

                if (existingResult != null)
                {
                    // Якщо пристрій вже брав участь у експерименті
                    if (existingResult.ExperimentId != experiment.Id)
                    {
                        return Ok("Пристрій вже бере участь в експерименті");
                    }
                    // Якщо пристрій вже взяв участь в цьому експерименті
                    else if (existingResult.ExperimentId == experiment.Id)
                    {
                        return Ok(new Dictionary<string, string>
                            { { "key", "button_color" }, { "value", existingResult.Value } });
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
                return Ok(new Dictionary<string, string> { { "key", "button_color" }, { "value", selectedOption } });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }


        [HttpGet]
        [Route("experiment/price")]
        public async Task<IActionResult> PriceExperimnt([FromQuery(Name = "device-token")] string deviceToken)
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
                    return Ok("Пристрій вже бере участь в експерименті");
                }
                // Якщо пристрій вже взяв участь в цьому експерименті
                else if (existingResult.ExperimentId == experiment.Id)
                {
                    return Ok(new Dictionary<string,string> { {"key" ,"price"}, {"value" , existingResult.Value} });
                }
            }

            // Генерація випадкового числа
            var random = new Random();
            var randomNumber = random.NextDouble();

            int selectedPrice = 0;

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

            return Ok(new Dictionary<string,string> { {"key" ,"price"}, {"value" , selectedPrice.ToString()} });
        }

        [HttpGet]
        [Route("experiment/statistic")]
        public async Task<IActionResult> GetStatistic()
        {
            // Отримання інформації про експеримент з кнопкою кольору з бази даних за допомогою Mediator.
            var experimentButtonColor = await _mediator.Send(new FindExperiment.Query { Key = "button_color" });

            // Отримання інформації про експеримент з ціною з бази даних за допомогою Mediator.
            var experimentPrice = await _mediator.Send(new FindExperiment.Query { Key = "price" });

            // Отримання кількості результатів для експерименту з кнопкою кольору.
            var experimentButtonColorResultCount = await _mediator.Send(new ExperimentResultsCount.Query { ExperimentId = experimentButtonColor.Id });

            // Отримання кількості результатів для експерименту з ціною.
            var experimentPriceResultCount = await _mediator.Send(new ExperimentResultsCount.Query { ExperimentId = experimentPrice.Id });

            // Створення словника, що містить статистику експериментів.
            var statistic = new Dictionary<string, int>
                {
                    { "Всі пристрої", experimentButtonColorResultCount + experimentPriceResultCount },
                    { "Експеримент з кольором кнопки", experimentButtonColorResultCount },
                    { "Експеримент з ціною", experimentPriceResultCount }
                };

            // Повертаємо статистику у форматі JSON.
            return Ok(statistic);
        }
    }
}
