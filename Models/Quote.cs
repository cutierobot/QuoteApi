namespace QuoteApi.Models
{
    public class Quote
    {
        public string Uuid { get; set; }
        public string QuoteText { get; set; }
        public string Author { get; set; }
        public string? Category { get; set; }
        public string? DateOfQuote { get; set; }
    }
}