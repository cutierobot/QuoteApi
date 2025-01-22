using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuoteApi.Interfaces;
using QuoteApi.Models;

namespace QuoteApi.Services;

// dbcontext used here and passed to app.MapGet and stuff
public class QuoteService: IQuoteService
{
    private readonly QuoteDbContext _context;

    public QuoteService(QuoteDbContext context)
    {
        _context = context;
    }
    
    public Quote? GetQuote(string uuid)
    {
        // _context.SaveChanges();
        // return _context.Quotes.Where(quote => quote.Uuid == "749160D8-F84F-48BD-B600-7B7E2F0778C1").Single();
        // return _context.Quotes.Where(quote => quote.Uuid == uuid).Single();
        try
        {
            return _context.Quotes.First(quote => quote.Uuid == uuid);
        }
        catch (InvalidOperationException invalidException)
        {
            Console.WriteLine(invalidException);
            // throw new InvalidOperationException("Logfile cannot be read-only", invalidException);
            // throw new InvalidOperationException("UUID provided is either null or does not exist in the table");
            return null;
            throw;
        }
    }

    public async Task<Quote> AddQuote(CreateQuote newQuote)
    {
        try
        {
            var quote = new Quote
            {
                Uuid = Guid.NewGuid().ToString(),
                Author = newQuote.Author,
                QuoteText = newQuote.QuoteText,
                Category = newQuote.Category,
                DateOfQuote = newQuote.DateOfQuote
            };
            await _context.Quotes.AddAsync(quote);
            await _context.SaveChangesAsync();
            Console.WriteLine("Successfully added quote");
            
            return quote;
        }
        catch (InvalidOperationException invalidException)
        {
            Console.WriteLine(invalidException);
            throw;
        }
        // throw new NotImplementedException();
    }
}