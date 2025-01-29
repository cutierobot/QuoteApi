using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuoteApi;
using QuoteApi.Interfaces;
using QuoteApi.Models;
using QuoteApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        options.SupportNonNullableReferenceTypes();
    }
);

// please see below Microsoft LEARN as to what this does
// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
// QuoteDatabase is coming from app settings.development.json
// TODO: don't store hardcoded password and stuff here in QuoteDatabase connectionString be more secure
var connectionString =
    builder.Configuration.GetConnectionString("QuoteDatabase")
    ?? throw new InvalidOperationException("Connection string" + "'QuoteDatabase' not found.");

builder.Services.AddDbContext<QuoteDbContext>(
    options =>
        options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IQuoteService, QuoteService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.models.openapioperation


/**
 * In order to return stuatus codes and define status codes iin swagger need to provide either "TypedResults" or "Results".
 * More info on this can be found here
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0#typedresults-vs-results
 *
 * TypedResults seems to be the preferred way so that is what I will be doing. To use TypedResults correctly I must use Task<>.
 * So this is where Task<> comes from, it's so we can use TypeResults for the swagger
 * @statusCode 200
 * @StatusCode 404
 */
app.MapGet("/quote{uuid}", async(string uuid, IQuoteService quoteService) =>
    {
        var result = await quoteService.GetQuote(uuid);
        return result == null ? Results.NotFound(): Results.Ok(result);
    })
    // .Produces<Quote>()
    .WithName("GetQuoteWithID")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Retrieves the quote that matches the id",
        Description = "Takes in a UUID and will return the first matching Quote structure found"
        // Parameters = ["id"]
    });



app.MapPost("/setQuote", async ( [FromBody]CreateQuote createQuote, IQuoteService quoteService) =>
    {
        var result = await quoteService.AddQuote(createQuote);
        return Results.Created($"/quote/{result.Uuid}", result);
    })
    .WithName("SetQuote")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Set a quote",
        Description = "Set's a quote to the database for storage and retrieval for other API calls"
        // Parameters = ["id"]
    });
    // .WithOpenApi(openApi =>
    // {
    //     return openApi;
    // });

app.MapGet("/quote/{author}", async (string author, IQuoteService quoteService) =>
    {
        var result = await quoteService.GetAuthorQuote(author);
        if (result.Count > 0)
        {
            return result;
        }
        else
        {
            throw new KeyNotFoundException("No quotes found matching your author");
        }
        // return result != null ? TypedResults.Ok(result) : TypedResults.NotFound();

    })
    .WithName("GetAuthorQuote")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Retrieves All authors quotes",
        Description = "Retrieves all quotes from the database matching author provided"
        // Parameters = ["id"]
    });
    
app.MapGet("/quote/{uuid}/display", async (string uuid, IQuoteService quoteService) =>
    {
        var result = await quoteService.GetQuote(uuid);
        return result == null ? Results.NotFound(): Results.Ok(quoteService.FormatQuote(result));
    })
    .WithName("GetDisplayAuthorQuote")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Retrieve quote and format it",
        Description = "Retrieves a quote using the uuid and format its in expected quote format"
    });

app.MapPut("quote/update/{uuid}", async (string uuid, IQuoteService quoteService) =>
    {
        throw new NotImplementedException();
    })
    .WithName("UpdateQuote")
    .WithTags("Not Implemented")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Update a single quote",
        Description = "Update a single quote using the uuid"
    });

app.Run();