using DotNetLessons.WebApi.Entities;
using DotNetLessons.WebApi.Models;

namespace DotNetLessons.WebApi.Extensions;

public static class PersonExtensions
{
    public static PersonDto ToDto(this Person person, bool includeAddress = true)
    {
        return new()
        {
            FirstName = person.FirstName,
            LastName = person.LastName,
            IsFromEarth = person.IsFromEarth,
            Addresses = includeAddress ? person.AddressesNavigations.ToDto() : new(),
        };
    }
}
