using Microsoft.EntityFrameworkCore;

namespace QuoteApiTest;

public class QuoteDB : DbContext
{
    public DbSet<Quote> Quotes { get; set; }

    public QuoteDB(DbContextOptions<QuoteDB> options)
    {
        // record Quote(string Uuid, string Text, string Author, string Category, DateOnly? Date)
    }
}