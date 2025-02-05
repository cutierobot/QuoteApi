using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteApi.Interfaces;
using QuoteApi.Models;

namespace QuoteApi.Services;

// dbcontext used here and passed to app.MapGet and stuff

// Service layer, handles buisness logic
public class QuoteService: IQuoteService
{
    private readonly QuoteDbContext _context;

    public QuoteService(QuoteDbContext context)
    {
        _context = context;
    }
    
    public async Task<Quote?> GetQuote(string uuid)
    {
        // _context.SaveChanges();
        // return _context.Quotes.Where(quote => quote.Uuid == "749160D8-F84F-48BD-B600-7B7E2F0778C1").Single();
        // return _context.Quotes.Where(quote => quote.Uuid == uuid).Single();
        try
        {
            return await _context.Quotes.FirstAsync(quote => quote.Uuid == uuid);
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

    public async Task<List<Quote>> GetAuthorQuote(string author)
    {
        try
        {
            return await _context.Quotes.Where(e => e.Author == author).ToListAsync();
        }
        catch (InvalidOperationException invalidException)
        {
            Console.WriteLine(invalidException);
            throw;
        }
    }

    public async Task<Quote> AddQuote(CreateQuote newQuote)
    {
        try
        {
            // DTO is probably to stop this, from using specifically the Qupte model itself maybe???
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

    /**
     * Format the given Quote into "quote" - author [DD/MM/YYYY]
     */
    public string FormatQuote(Quote quote)
    {
        string dateQuoteFormat;
        DateTime cake = new DateTime();
        //DateTime? dateOfQuote = quote.DateOfQuote; // Access Blah
        if (quote.DateOfQuote is not null)
        {
            // dateQuoteFormat = FormatDateOfQuote(dateOfQuote ?? quote.DateOfQuote!.());
            dateQuoteFormat = FormatDateOfQuote(quote.DateOfQuote ?? cake);
        }
        else
        {
            dateQuoteFormat = FormatDateOfQuote(new DateTime());
        }
        // var dateOfQuote = quote!.DateOfQuote!;
        var dateOfQuote = quote.DateOfQuote != null ? " [" + dateQuoteFormat + "]": string.Empty;
        return quote.QuoteText + " - " + quote.Author + dateOfQuote;
    }

    private string FormatDateOfQuote(DateTime dateOfQuote)
    {
        // TODO: still need to properly handle timeZones and shit
        return dateOfQuote!.ToString("dd/MM/yyyy") ?? "";
    }
}