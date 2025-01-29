using QuoteApi.Models;

namespace QuoteApi.Interfaces;

// Dependency Injection Abstraction part
// think the blueprint
public interface IQuoteService
{
    // this one is only for testing, delete after added other ones
    Task<Quote?> GetQuote(string uuid);
    // QuoteResponse AddQuote(CreateQuote quote);
    Task<Quote> AddQuote(CreateQuote quote);
    
    Task<List<Quote>> GetAuthorQuote(string author);

    string FormatQuote(Quote quote);
}