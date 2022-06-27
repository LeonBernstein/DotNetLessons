using DotNetLessons.WebApi.Entities;
using DotNetLessons.WebApi.Models;

namespace DotNetLessons.WebApi.Extensions;

public static class EnumerableAddressesExtensions
{
    public static List<AddressDto> ToDto(this IEnumerable<Address> addresses)
    {
        return addresses.Select(x => x.ToDto()).ToList();
    }
}
