namespace QuoteApi.Models;

public class CreateQuote
{
    
        public required string QuoteText { get; set; }
        public required string Author { get; set; }
        public string? Category { get; set; }
        public string? DateOfQuote { get; set; }
}
