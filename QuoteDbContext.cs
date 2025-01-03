using Microsoft.EntityFrameworkCore;

namespace QuoteApi
{
    public class QuoteDbContext : DbContext
    {
        public QuoteDbContext(DbContextOptions<QuoteDbContext> options): base(options)
        {
            // record Quote(string Uuid, string Text, string Author, string Category, DateOnly? Date)
        }

        public DbSet<Quote> Quotes { get; set; }
    }
}