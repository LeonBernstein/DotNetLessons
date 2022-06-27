using DotNetLessons.WebApi.Entities;
using DotNetLessons.WebApi.Models;

namespace DotNetLessons.WebApi.Extensions;

public static class AddressExtensions
{
    public static AddressDto ToDto(this Address address)
    {
        return new()
        {
            GalaxyName = address.GalaxyName,
            PlanetName = address.PlanetName,
            HasCosmicRadiation = address.HasCosmicRadiation,
        };
    }
}
