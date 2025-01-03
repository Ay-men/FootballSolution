namespace Domain.Interfaces;

public interface ITeamUniquenessChecker
{
    Task<bool> IsUnique(
        string name, 
        string? city, 
        string? country, 
        int? foundedYear, 
        Guid? excludeTeamId = null);
}