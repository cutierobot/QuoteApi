using Microsoft.EntityFrameworkCore;
using QuoteApi.Models;

namespace QuoteApi
{
    public class QuoteDbContext : DbContext
    {
        public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options)
        {
            // record Quote(string Uuid, string Text, string Author, string Category, DateOnly? Date)
        }

        // DbSet<> correspondes to DB table 
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>()
                .HasKey(quote => quote.Uuid)
                .HasName("PK_Quotes");
        }
    }
}