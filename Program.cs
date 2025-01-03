using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuoteApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);

// please see below Microsoft LEARN as to what this does
// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
// QuoteDatabase is coming from appsettings.development.json
// TODO: don't store hardcoded password and stuff here in QuoteDatabase connectionString be more secure
var connectionString =
    builder.Configuration.GetConnectionString("QuoteDatabase")
    ?? throw new InvalidOperationException("Connection string" + "'QuoteDatabase' not found.");

builder.Services.AddDbContext<QuoteDbContext>(
    options =>
        options.UseSqlServer(connectionString)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.models.openapioperation


app.MapGet("/weatherforecast", () =>
    {
        // loop from 1 - 5,
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    // creates a Date using today's date and adding one day.
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    // random number from -20 - 55
                    Random.Shared.Next(-20, 55),
                    // randomly choose a option from the summaries array
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();


app.MapGet("/myTurn", () =>
    {
        return new WeatherForecast
        (
            // creates a Date using today's date and adding one day.
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            // random number from -20 - 55
            Random.Shared.Next(-20, 55),
            // randomly choose a option from the summaries array
            summaries[Random.Shared.Next(summaries.Length)]
        );
    })
    .WithName("MyTurn")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "This is fucking documentation you dig :)"
    });


app.MapGet("/quote{uuid}", (string uuid) =>
    {
        // return new WeatherForecast
        // (
        //     // creates a Date using today's date and adding one day.
        //     DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
        //     // random number from -20 - 55
        //     Random.Shared.Next(-20, 55),
        //     // randomly choose a option from the summaries array
        //     summaries[Random.Shared.Next(summaries.Length)]
        // );
        return "The quote id is " + uuid;
    })
    .WithName("GetQuoteWithID")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Retrieves the quote that matches the id",
        // Parameters = ["id"]
    });


app.MapPost("/setQuote", (Quote quote) =>
    {
        // return "The quote id is " + id;
        return "This is where we set the Quote record to be in the db";
    })
    .WithName("SetQuote")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Takes Quote class and send to DBcontext to be set to db",
        // Parameters = ["id"]
    });

app.Run();

namespace QuoteApi
{
    /*
     * Record is a new data type. Is a Class or Struct for special specific data models. Tells compiler that things under
     * the 'record' modifier are useful for storing data.
     * Records by defualt are immutable (for this dummy that means think const, can't be changed once set.)
     * Value-Based Equality - for this dummy that means "bee" === 'bee', compares by values and not by reference. Im thinking
     *  back to C with pointers shit.
     *
     * there is Record and record class types. the thing we are seeing below I think ig a Record Class.
     *
     */
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }


    public record Quote(string Uuid, string QuoteText, string Author, string Category, DateOnly? DateOfQuote)
    {
        public string Display => $"{QuoteText} - {Author} {DateOfQuote}";
    }
}