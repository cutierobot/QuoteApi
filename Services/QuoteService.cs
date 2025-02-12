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
    
    // public async Task<Quote?> GetQuote(string uuid)
    public async Task<QuoteDTO?> GetQuote(string uuid)
    {
        // _context.SaveChanges();
        // return _context.Quotes.Where(quote => quote.Uuid == "749160D8-F84F-48BD-B600-7B7E2F0778C1").Single();
        // return _context.Quotes.Where(quote => quote.Uuid == uuid).Single();
        try
        {
            // return await _context.Quotes.FirstAsync(quote => quote.Uuid == uuid);
            
            var result = await _context.Quotes.FirstAsync(quote => quote.Uuid == uuid);
            return new QuoteDTO
            {
                Uuid = result.Uuid,
                QuoteText = result.QuoteText,
                Author = result.Author,
                Category = result.Category,
                DateOfQuote = result.DateOfQuote
            };
        }
        catch (InvalidOperationException invalidException)
        {
            Console.WriteLine(invalidException);
            return null;
        }
    }

    public async Task<List<QuoteDTO>> GetAuthorQuote(string author)
    {
        try
        {
            // return await _context.Quotes.Where(e => e.Author == author).ToListAsync();
            return await _context.Quotes.Where(e => e.Author == author)
                .Select(quote => new QuoteDTO
                {
                    Uuid = quote.Uuid,
                    QuoteText = quote.QuoteText,
                    Author = quote.Author,
                    Category = quote.Category,
                    DateOfQuote = quote.DateOfQuote
                }).ToListAsync();
        }
        catch (InvalidOperationException invalidException)
        {
            Console.WriteLine(invalidException);
            throw;
        }
    }

    public async Task<QuoteDTO> AddQuote(CreateQuote newQuote)
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
            
            return new QuoteDTO
            {
                Uuid = quote.Uuid,
                QuoteText = quote.QuoteText,
                Author = quote.Author,
                Category = quote.Category,
                DateOfQuote = quote.DateOfQuote
            };
        }
        catch (InvalidOperationException invalidException)
        {
            Console.WriteLine(invalidException);
            throw;
        }
    }

    /**
     * Format the given Quote into "quote" - author [DD/MM/YYYY]
     */
    public string FormatQuote(QuoteDTO quote)
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