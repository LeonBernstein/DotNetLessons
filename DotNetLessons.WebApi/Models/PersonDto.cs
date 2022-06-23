namespace DotNetLessons.WebApi.Models;

public class PersonDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsFromEarth { get; set; }

    public List<AddressDto> Addresses { get; set; } = null!;
}
