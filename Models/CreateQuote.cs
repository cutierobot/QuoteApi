namespace QuoteApi.Models;

// representation of the Quote SQL Table and all it's columns for EntityFramework to use when trying to CREATE a quote

public class CreateQuote
{
    
        public required string QuoteText { get; set; }
        public required string Author { get; set; }
        public string? Category { get; set; }
        public DateTime? DateOfQuote { get; set; } = null;
}
