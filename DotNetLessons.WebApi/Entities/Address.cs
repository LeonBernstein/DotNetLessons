
namespace DotNetLessons.WebApi.Entities;

public partial class Address
{
    public Address()
    {
        PersonsNavigation = new HashSet<Person>();
    }

    public int AddressId { get; set; }
    public int PersonId { get; set; }
    public string GalaxyName { get; set; } = null!;
    public string PlanetName { get; set; } = null!;
    public bool HasConsmicRadiation { get; set; }

    public virtual ICollection<Person> PersonsNavigation { get; set; }
}
