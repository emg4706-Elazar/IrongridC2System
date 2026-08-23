using Producer.Models;
using Producer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Confluent.Kafka.Admin;

namespace Producer;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(
            " =============== IronGrid C2 System ============\n");

        // ============== Setup Configuration ================
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();




        // =================== Load Files ===================
        string reportsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "field_reports.json");

        var loader = new DataLoaderService();

        var reports = loader.Load<Report>(reportsPath);

    }
}
