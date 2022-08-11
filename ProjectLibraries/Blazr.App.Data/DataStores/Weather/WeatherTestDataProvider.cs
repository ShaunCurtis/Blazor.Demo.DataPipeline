﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Data;

public class WeatherTestDataProvider
{
    private int RecordsToGenerate;

    public IEnumerable<DboWeatherForecast> WeatherForecasts { get; private set; } = Enumerable.Empty<DboWeatherForecast>();

    public IEnumerable<DboWeatherSummary> WeatherSummaries { get; private set; } = Enumerable.Empty<DboWeatherSummary>();

    public IEnumerable<DboWeatherLocation> WeatherLocations { get; private set; } = Enumerable.Empty<DboWeatherLocation>();

    private WeatherTestDataProvider()
        => this.Load();

    public void LoadDbContext<TDbContext>(IDbContextFactory<TDbContext> factory) where TDbContext : DbContext
    {
        using var dbContext = factory.CreateDbContext();

        var weatherForcasts = dbContext.Set<DboWeatherForecast>();
        var weatherSummaries = dbContext.Set<DboWeatherSummary>();
        var weatherLocations = dbContext.Set<DboWeatherLocation>();

        // Check if we already have a full data set
        // If not clear down any existing data and start again
        if (weatherSummaries.Count() == 0 || weatherForcasts.Count() == 0)
        {

            dbContext.RemoveRange(weatherSummaries.ToList());
            dbContext.RemoveRange(weatherForcasts.ToList());
            dbContext.RemoveRange(weatherLocations.ToList());
            dbContext.SaveChanges();
            dbContext.AddRange(this.WeatherSummaries);
            dbContext.AddRange(this.WeatherForecasts);
            dbContext.AddRange(this.WeatherLocations);
            dbContext.SaveChanges();
        }
    }

    public void Load(int records = 100)
    {
        RecordsToGenerate = records;

        if (WeatherSummaries.Count() == 0)
        {
            this.LoadLocations();
            this.LoadSummaries();
            this.LoadForecasts();
        }
    }

    private void LoadSummaries()
    {
        this.WeatherSummaries = new List<DboWeatherSummary> {
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Freezing"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Bracing"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Chilly"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Cool"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Mild"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Warm"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Balmy"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Hot"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Sweltering"},
            new DboWeatherSummary { Uid = Guid.NewGuid(), Summary = "Scorching"},
        };
    }

    private void LoadLocations()
    {
        this.WeatherLocations = new List<DboWeatherLocation> {
            new DboWeatherLocation { Uid = Guid.NewGuid(), Location = "Gloucester"},
            new DboWeatherLocation { Uid = Guid.NewGuid(), Location = "Capestang"},
            new DboWeatherLocation { Uid = Guid.NewGuid(), Location = "Alvor"},
            new DboWeatherLocation { Uid = Guid.NewGuid(), Location = "Adelaide"},
        };
    }

    private void LoadForecasts()
    {
        var summaryArray = this.WeatherSummaries.ToArray();
        var forecasts = new List<DboWeatherForecast>();

        foreach (var location in WeatherLocations)
        {
            forecasts
                .AddRange(Enumerable
                    .Range(1, RecordsToGenerate)
                    .Select(index => new DboWeatherForecast
                    {
                        Uid = Guid.NewGuid(),
                        WeatherSummaryId = summaryArray[Random.Shared.Next(summaryArray.Length)].Uid,
                        WeatherLocationId = location.Uid,
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                    })
                );
        }

        this.WeatherForecasts = forecasts;
    }

    public DboWeatherForecast GetForecast()
    {
        var summaryArray = this.WeatherSummaries.ToArray();

        return new DboWeatherForecast
        {
            Uid = Guid.NewGuid(),
            WeatherSummaryId = summaryArray[Random.Shared.Next(summaryArray.Length)].Uid,
            Date = DateTime.Now.AddDays(-1),
            TemperatureC = Random.Shared.Next(-20, 55),
        };
    }

    public DboWeatherForecast? GetRandomRecord()
    {
        var record = new DboWeatherForecast();
        if (this.WeatherForecasts.Count() > 0)
        {
            var ran = new Random().Next(0, WeatherForecasts.Count());
            return this.WeatherForecasts.Skip(ran).FirstOrDefault();
        }
        return null;
    }

    public DvoWeatherForecast GetDvoWeatherForecast(DboWeatherForecast record)
    {
        return new DvoWeatherForecast
        {
            Uid = record.Uid,
            WeatherLocationId = record.WeatherLocationId,
            WeatherSummaryId = record.WeatherSummaryId,
            Date = record.Date,
            TemperatureC = record.TemperatureC,
            Location = this.WeatherLocations.SingleOrDefault(item => item.Uid == record.WeatherLocationId)?.Location ?? string.Empty,
            Summary = this.WeatherSummaries.SingleOrDefault(item => item.Uid == record.WeatherSummaryId)?.Summary ?? string.Empty
        };
    }

    private static WeatherTestDataProvider? _weatherTestData;

    public static WeatherTestDataProvider Instance()
    {
        if (_weatherTestData is null)
            _weatherTestData = new WeatherTestDataProvider();

        return _weatherTestData;
    }
}
