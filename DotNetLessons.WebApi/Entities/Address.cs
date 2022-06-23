
namespace DotNetLessons.WebApi.Entities;

public partial class Address
{
    public int AddressId { get; set; }
    public int PersonId { get; set; }
    public string GalaxyName { get; set; } = null!;
    public string PlanetName { get; set; } = null!;
    public bool HasCosmicRadiation { get; set; }

    public virtual Person PersonNavigation { get; set; } = null!;
}
