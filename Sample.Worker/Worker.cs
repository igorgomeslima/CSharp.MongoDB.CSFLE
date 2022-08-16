using System;
using System.Linq;
using System.Threading;
using Sample.Worker.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Worker.Persistence.Repositories;

using static System.Threading.Tasks.Task;

namespace Sample.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFooRepository _fooRepository;

        public Worker(ILogger<Worker> logger, IFooRepository fooRepository)
        {
            _logger = logger;
            _fooRepository = fooRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var foo = new Foo
            {
                Name = nameof(Foo.Name),
                MotherName = nameof(Foo.MotherName),
                Bars = new[]
                {
                    new Bar { Name = BarNames.SuperA, Description = "Desc. SuperA", CreatedAt = DateTime.Now },
                    new Bar { Name = BarNames.SuperB, Description = "Desc. SuperB", CreatedAt = DateTime.Now }
                }
            };
            
            var fooId = await _fooRepository.CreateFoo(foo, stoppingToken);

            await Delay(500, stoppingToken);
            
            var updatedBar = foo.Bars.FirstOrDefault(bar => { bar.UpdatedAt = DateTime.Now; bar.Description = "New Desc. SuperA"; return true; });

            var upsertResult = await _fooRepository.UpsertBar(fooId, updatedBar, stoppingToken);

            Console.WriteLine("Upsert Success? {0}", upsertResult);
            Console.ReadLine();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Delay(1000, stoppingToken);
            }
        }
    }
}