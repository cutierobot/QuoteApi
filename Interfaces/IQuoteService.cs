using QuoteApi.Models;

namespace QuoteApi.Interfaces;

// Dependency Injection Abstraction part
// Dependency Inversion Principle
// think the blueprint
public interface IQuoteService
{
    // this one is only for testing, delete after added other ones
    Task<QuoteDTO?> GetQuote(string uuid);
    // QuoteResponse AddQuote(CreateQuote quote);
    Task<QuoteDTO> AddQuote(CreateQuote quote);
    
    Task<List<QuoteDTO>> GetAuthorQuote(string author);

    string FormatQuote(QuoteDTO quote);

    // string FormatQuote(Quote quote); Task<Quote?> GetQuote(string uuid);
    // // QuoteResponse AddQuote(CreateQuote quote);
    // Task<Quote> AddQuote(CreateQuote quote);
    //
    // Task<List<Quote>> GetAuthorQuote(string author);
    //
    // string FormatQuote(Quote quote);
}