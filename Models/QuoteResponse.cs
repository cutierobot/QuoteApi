using System.ComponentModel.DataAnnotations;

namespace QuoteApi.Models;

public class QuoteResponse
{ 
    [Key]
    public required string Uuid { get; set; }
    public required string QuoteText { get; set; }
    public required string Author { get; set; }
    public string? Category { get; set; }
    public string? DateOfQuote { get; set; }
}