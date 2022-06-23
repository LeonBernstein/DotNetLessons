using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DotNetLessons.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AsyncLessonController : ControllerBase
{
    private const int NUM_OF_ITERATIONS = 1_000_000_000;

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
}