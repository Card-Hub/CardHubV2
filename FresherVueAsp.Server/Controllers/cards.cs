// Sources:
// https://jasonwatmore.com/post/2022/06/23/net-6-connect-to-postgresql-database-with-entity-framework-core

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FresherVueAsp.Server.Controllers;

public class CardDbContext: DbContext
{
    public DbSet<Card> cards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseNpgsql(config.GetConnectionString("WebApiDatabase"));
    }
}

public class Card {
    public int id { get; set; }
    public string suit { get; set; }
    public string value { get; set; }
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
    public async Task<ActionResult<IEnumerable<Card>>> GetCards()
    {
        var cards = await _context.cards.ToListAsync();
        return Ok(cards);
    }
}