using DotNetLessons.WebApi.DbModel;
using DotNetLessons.WebApi.Entities;
using DotNetLessons.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DotNetLessons.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AsyncLessonController : ControllerBase
{
    private const int NUM_OF_ITERATIONS = 1_000_000_000;

    private readonly IDbContextFactory<DotNetLessonsContext> _dotNetLessonsContextFactory;

    public AsyncLessonController(IDbContextFactory<DotNetLessonsContext> dotNetLessonsContextFactory)
    {
        _dotNetLessonsContextFactory = dotNetLessonsContextFactory;
    }

    [HttpGet]
    public IActionResult GetTheTimeOfLongRunningTask()
    {
        Stopwatch sp = new();
        sp.Start();

        Random rnd = new();
        int calculatedValue = 0;

        for (int i = 0; i < NUM_OF_ITERATIONS; i++)
        {
            calculatedValue += rnd.Next(-5, 6);
        }

        sp.Stop();

        var result = new
        {
            CalculatedValue = calculatedValue,
            NumOfIterations = NUM_OF_ITERATIONS,
            Time = sp.Elapsed.ToString("G"),
        };

        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetTheTimeOfLongRunningTaskInParallel()
    {
        Stopwatch sp = new();
        sp.Start();

        ConcurrentDictionary<int, TaskCompletionSource<int>> threadsResults = new();

        int numOfThreads = Environment.ProcessorCount;
        int numOfIterations = NUM_OF_ITERATIONS / numOfThreads;

        Action iterationActionCreator(TaskCompletionSource<int> taskSource)
        {
            return () =>
            {
                Random rnd = new();
                int result = 0;
                for (int i = 0; i < numOfIterations; i++)
                {
                    result += rnd.Next(-5, 6);
                }
                taskSource.SetResult(result);
            };
        }

        IEnumerable<Task> tasks = Enumerable
            .Range(0, numOfThreads)
            .Select(threadIndex =>
            {
                TaskCompletionSource<int> taskSource = new();
                threadsResults.TryAdd(threadIndex, taskSource);

                Action iterationAction = iterationActionCreator(taskSource);
                Thread thread = new(iterationAction.Invoke);
                thread.Start();

                return taskSource.Task;
            });

        Task.WhenAll(tasks).Wait();

        int calculatedValue = threadsResults.Values
            .Select(x => x.Task.Result)
            .Sum();

        sp.Stop();

        var result = new
        {
            CalculatedValue = calculatedValue,
            NumOfIterations = NUM_OF_ITERATIONS,
            Time = sp.Elapsed.ToString("G"),
            NumOfThreads = numOfThreads,
            ThreadsResults = threadsResults.ToDictionary(x => x.Key, x => x.Value.Task.Result),
        };

        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddPersonSync([FromBody] PersonDto personDto)
    {
        Person person = new()
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            IsFromEarth = personDto.IsFromEarth,
            AddressesNavigations = personDto.Addresses.Select(x => new Address()
            {
                GalaxyName = x.GalaxyName,
                PlanetName = x.PlanetName,
                HasCosmicRadiation = x.HasCosmicRadiation,
            }).ToList(),
        };

        using var context = _dotNetLessonsContextFactory.CreateDbContext();
        context.Persons.Add(person);
        context.SaveChanges();

        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAllSync()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        List<Person> persons = context.Persons
            .Include(x => x.AddressesNavigations)
            .ToList();

        if (persons.Count == 0) return NoContent();
        return Ok(persons);
    }

    [HttpGet]
    public IActionResult GetAllSyncAndLazy()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();
        List<Person> persons = context.Persons.ToList();

        persons.ForEach(x =>
        {
            x.AddressesNavigations.ToList().ForEach(_ => { });
        });

        if (persons.Count == 0) return NoContent();
        return Ok(persons);
    }

    [HttpDelete]
    public IActionResult DeletePersonSync(int personId)
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();
        var person = context.Persons.FirstOrDefault(x => x.PersonId == personId);
        if (person is not null)
        {
            context.Remove(person);
            context.SaveChanges();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] PersonDto personDto)
    {
        Person person = new()
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            IsFromEarth = personDto.IsFromEarth,
            AddressesNavigations = personDto.Addresses.Select(x => new Address()
            {
                GalaxyName = x.GalaxyName,
                PlanetName = x.PlanetName,
                HasCosmicRadiation = x.HasCosmicRadiation,
            }).ToList(),
        };

        using var context = _dotNetLessonsContextFactory.CreateDbContext();
        context.Persons.Add(person);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        List<Person> persons = await context.Persons
            .Include(x => x.AddressesNavigations)
            .ToListAsync();

        if (persons.Count == 0) return NoContent();
        return Ok(persons);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePerson(int personId)
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();
        var person = await context.Persons.FirstOrDefaultAsync(x => x.PersonId == personId);

        if (person is not null)
        {
            context.Remove(person);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetPersonsIds(int count)
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();
        var persons = await context.Persons
            .Take(count)
            .Select(x => new { x.PersonId })
            .ToListAsync();

        if (persons.Count == 0) return NoContent();
        return Ok(persons);
    }
}