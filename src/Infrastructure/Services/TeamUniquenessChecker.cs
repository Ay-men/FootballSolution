namespace Infrastructure.Services;

using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class TeamUniquenessChecker : ITeamUniquenessChecker
{
    private readonly ApplicationDbContext _context;

    public TeamUniquenessChecker(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUnique(
        string name,
        string? city,
        string? country,
        int? foundedYear,
        Guid? excludeTeamId = null)
    {
        var query = _context.Teams.AsQueryable();

        if (excludeTeamId.HasValue)
        {
            query = query.Where(t => t.Id != excludeTeamId.Value);
        }

        // Check for potential duplicates based on provided criteria
        var possibleDuplicate = await query
            .Where(t =>
                t.Name.ToLower() == name.ToLower() &&
                (city == null || t.City == city) &&
                (country == null || t.Country == country) &&
                (foundedYear == null || t.FoundedYear == foundedYear))
            .FirstOrDefaultAsync();

        return possibleDuplicate == null;
    }
}