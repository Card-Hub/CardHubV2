// Sources:
// https://jasonwatmore.com/post/2022/06/23/net-6-connect-to-postgresql-database-with-entity-framework-core

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers;

public class CardDbContext: DbContext
{
    public DbSet<PlayingCard> cards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseNpgsql(config.GetConnectionString("WebApiDatabase"));
    }
}

[ApiController]
[Route("[controller]")]
public class CardsController : ControllerBase
{
    private readonly CardDbContext _context;

    public CardsController(CardDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayingCard>>> GetCards()
    {
        var cards = await _context.cards.ToListAsync();
        return Ok(cards);
    }
}