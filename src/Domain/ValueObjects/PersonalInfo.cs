namespace Domain.Entities.ValueObjects;

using Exceptions;
using Primitives;

public class PersonalInfo : ValueObject
{
    protected PersonalInfo()
    {
    }

    private PersonalInfo(string firstName, string lastName, DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (age is < 16 or > 45)
            throw new DomainException("Player must be between 16 and 45 years old");

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        
    }

    public string FirstName { get; }
    public string LastName { get; }
    public DateOnly DateOfBirth { get; }
    

    public static PersonalInfo Create(string firstName, string lastName, DateOnly dateOfBirth)
    {
        return new PersonalInfo(firstName, lastName, dateOfBirth);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
        yield return DateOfBirth;
    }
}