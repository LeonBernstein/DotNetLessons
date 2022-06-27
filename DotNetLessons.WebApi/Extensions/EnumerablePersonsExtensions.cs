using DotNetLessons.WebApi.Entities;
using DotNetLessons.WebApi.Models;

namespace DotNetLessons.WebApi.Extensions;

public static class EnumerablePersonsExtensions
{
    public static List<PersonDto> ToDto(this IEnumerable<Person> persons, bool includeAddress = true)
    {
        return persons.Select(x => x.ToDto(includeAddress)).ToList();
    }
}
