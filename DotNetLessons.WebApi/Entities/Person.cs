
namespace DotNetLessons.WebApi.Entities;

public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsFromEarth { get; set; }

    public virtual Address AddressNavigation { get; set; } = null!;
}
