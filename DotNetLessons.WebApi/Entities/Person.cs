
namespace DotNetLessons.WebApi.Entities;

public class Person
{
    public Person()
    {
        AddressesNavigation = new HashSet<Address>();
    }

    public int PersonId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsFromEarth { get; set; }

    public virtual ICollection<Address> AddressesNavigation { get; set; }
}
