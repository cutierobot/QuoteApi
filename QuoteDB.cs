using Microsoft.EntityFrameworkCore;

namespace QuoteApi
{
    public class QuoteDb : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }

        public QuoteDb(DbContextOptions<QuoteDb> options)
        {
            // record Quote(string Uuid, string Text, string Author, string Category, DateOnly? Date)
        }
    }
}