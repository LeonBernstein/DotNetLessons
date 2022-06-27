using DotNetLessons.WebApi.DbModel;
using DotNetLessons.WebApi.Entities;
using DotNetLessons.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace DotNetLessons.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class LinqLessonController : ControllerBase
{
    private readonly IDbContextFactory<DotNetLessonsContext> _dotNetLessonsContextFactory;

    public LinqLessonController(IDbContextFactory<DotNetLessonsContext> dotNetLessonsContextFactory)
    {
        _dotNetLessonsContextFactory = dotNetLessonsContextFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Select()
    {
        List<Person> persons = await GetPersonsAsync();

        IEnumerable<string> fullNames = persons.Select(x => $"{x.FirstName} {x.LastName}");

        if (fullNames.Any()) return Ok(fullNames);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> SelectMany()
    {
        List<Person> persons = await GetPersonsWithAddressesAsync();

        IEnumerable<Address> addresses = persons.SelectMany(x => x.AddressesNavigations);

        if (addresses.Any()) return Ok(addresses.ToDto());
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Aggregate()
    {
        List<Person> persons = await GetPersonsWithAddressesAsync();

        Person? personWithLongestGalaxyName = persons.Aggregate((person1, person2) =>
        {
            int longestGalaxyNameLengthP1 = person1.AddressesNavigations.Max(y => y.GalaxyName.Length);
            int longestGalaxyNameLengthP2 = person2.AddressesNavigations.Max(y => y.GalaxyName.Length);
            return longestGalaxyNameLengthP1 > longestGalaxyNameLengthP2 ? person1 : person2;
        });

        if (personWithLongestGalaxyName is not null) return Ok(personWithLongestGalaxyName.ToDto());
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> UnionBy()
    {
        Task<List<Person>> personsTask1 = GetPersonsAsync();
        Task<List<Person>> personsTask2 = GetPersonsAsync();

        IEnumerable<Person> firstTwoPersons = (await personsTask1).Take(2);
        IEnumerable<Person> firstThreePersons = (await personsTask1).Take(3);
        IEnumerable<Person> lastTwoPersons = (await personsTask2).TakeLast(2);

        IEnumerable<Person> persons = firstTwoPersons.UnionBy(lastTwoPersons, x => x.PersonId);
        persons = persons.UnionBy(firstThreePersons, x => x.PersonId);

        if (persons.Any()) return Ok(persons.ToDto(includeAddress: false));
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> DistinctBy()
    {
        Task<List<Person>> personsTask1 = GetPersonsAsync();
        Task<List<Person>> personsTask2 = GetPersonsAsync();

        IEnumerable<Person> persons = (await personsTask1).Concat(await personsTask2);

        persons = persons.DistinctBy(x => x.PersonId);

        if (persons.Any()) return Ok(persons.ToDto(includeAddress: false));
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> ContextError()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        try
        {
            Task<List<Person>> personsTask1 = context.Persons.ToListAsync();
            Task<List<Person>> personsTask2 = context.Persons.ToListAsync();

            await personsTask1;
            await personsTask2;

            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e);
        }
    }

    [HttpGet]
    public async Task<IActionResult> QuerytableWhereExp()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        Expression<Func<Person, bool>> hasCosmicRadiationPredicateExp = person => person.AddressesNavigations.Any(x => x.HasCosmicRadiation);

        try
        {
            IEnumerable<Person> persons = await context.Persons
                .Include(x => x.AddressesNavigations)
                .Where(hasCosmicRadiationPredicateExp)
                .ToListAsync();

            if (persons.Any()) return Ok(persons.ToDto());
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e);
        }
    }

    [HttpGet]
    public async Task<IActionResult> QuerytableWhereFunc()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        static bool hasCosmicRadiationPredicate(Person person)
        {
            return person.AddressesNavigations.Any(x => x.HasCosmicRadiation);
        }

        try
        {
            IEnumerable<Person> persons = await context.Persons
                .Include(x => x.AddressesNavigations)
                .Where(hasCosmicRadiationPredicate)
                .AsQueryable()
                .ToListAsync();

            if (persons.Any()) return Ok(persons.ToDto());
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e);
        }
    }

    [HttpGet]
    public IActionResult QuerytableWhereFuncSync()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        static bool hasCosmicRadiationPredicate(Person person)
        {
            return person.AddressesNavigations.Any(x => x.HasCosmicRadiation);
        }

        try
        {
            IEnumerable<Person> persons = context.Persons
                .Include(x => x.AddressesNavigations)
                .Where(hasCosmicRadiationPredicate)
                .AsQueryable()
                .ToList();

            if (persons.Any()) return Ok(persons.ToDto());
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e);
        }
    }

    [HttpGet]
    public async Task<IActionResult> QuerytableFirstOrDefault()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        Person? person = await context.Persons.FirstOrDefaultAsync();

        if (person is null) return NoContent();

        Person? samePerson = await context.Persons.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

        return Ok(person == samePerson);
    }

    [HttpGet]
    public async Task<IActionResult> QuerytableSingleOrDefault()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        Person? person = await context.Persons.FirstOrDefaultAsync();

        if (person is null) return NoContent();

        Person? samePerson = await context.Persons.SingleOrDefaultAsync(x => x.PersonId == person.PersonId);

        return Ok(person == samePerson);
    }

    [HttpGet]
    public async Task<IActionResult> QuerytableFind()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        Person? person = await context.Persons.FirstOrDefaultAsync();

        if (person is null) return NoContent();

        Person? samePerson = await context.Persons.FindAsync(person.PersonId);

        return Ok(person == samePerson);
    }

    [HttpGet]
    public async Task<IActionResult> QuerytableLoad()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        Person? person = await context.Persons.FirstOrDefaultAsync();

        if (person is null) return NoContent();

        Person? newPerson = await context.Persons.FindAsync(person.PersonId);

        if (newPerson is null) return NoContent();

        await context.Entry(newPerson)
            .Collection(x => x.AddressesNavigations)
            .LoadAsync();

        await context.Entry(newPerson)
            .Collection(x => x.AddressesNavigations)
            .LoadAsync();

        await context.Entry(newPerson.AddressesNavigations.First())
            .Reference(x => x.PersonNavigation)
            .LoadAsync();

        return Ok(newPerson);
    }

    private async Task<List<Person>> GetPersonsAsync()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        return await context.Persons
            .ToListAsync();
    }

    private async Task<List<Person>> GetPersonsWithAddressesAsync()
    {
        using var context = _dotNetLessonsContextFactory.CreateDbContext();

        return await context.Persons
            .Include(x => x.AddressesNavigations)
            .ToListAsync();
    }
}
