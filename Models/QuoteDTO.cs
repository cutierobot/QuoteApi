using System.ComponentModel.DataAnnotations;

namespace QuoteApi.Models
{
    // This is not a DTO
    // this is a Model, it is the representation of the Quote SQL Table and all it's columns for EntityFramework to use
    // Please see this Medium post
    public class QuoteDTO
    {
        [Key]
        public required string Uuid { get; set; }
        public required string QuoteText { get; set; }
        public required string Author { get; set; }
        public string? Category { get; set; }
        public DateTime? DateOfQuote { get; set; }
    }
}