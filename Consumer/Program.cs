using Consumer.Models;
using Consumer.Data;
using Consumer.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Consumer;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(
            "============= Irongrid c2 System - Consumer ==========");

        // ============== Setup Configuration ==============
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string bootstrapServers =
            configuration["Kafka:BootstrapServers"]!;

        string uavTopic = configuration["Kafka:Topics:UAV"]
            ?? throw new InvalidOperationException(
                "\u2717 Failed: Topic's name of 'uav' is missing.");

        string sensorTopic = configuration["Kafka:Topics:PerimeterSensor"]
            ?? throw new InvalidOperationException(
                "\u2717 Failed: Topic's name of 'PerimeterSensor' is missing.");

        var connectionString = configuration
            .GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                $"ConnectionString is missing.");
            

        // ================ Inject Depenencies =============
        var services = new ServiceCollection();

        // Register the DbContext
        services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString)));

        // Register the ProcessorService
        services.AddScoped<ProcessorService>();

        // Create the DI container
        var serviceProvider = 
            services.BuildServiceProvider();


        // ============ Ensure The Database exists =========
        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync();
            Console.WriteLine("\u2714  Database ready.");
        }

        // =============== Jenerate the Consumer ============
        var config = new ConsumerConfig()
        {
            BootstrapServers = bootstrapServers,
            GroupId = configuration["Kafka:Groupid"],
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(
            config).Build();

        consumer.Subscribe([uavTopic, sensorTopic]);

        // ============== Consume Loop ==================
        try
        {
            while (true)
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(1));

                if (result?.Message?.Value is null)
                    continue;

                string jsonMessage = result.Message.Value;

                // Jenerate the processor
                using (var scope = serviceProvider.CreateScope())
                {
                    var processor = scope.ServiceProvider
                        .GetRequiredService<ProcessorService>();

                    var success = await processor.ProcessReportAsync(jsonMessage);

                    if (success)
                    {
                        consumer.Commit(result);
                        Console.WriteLine(
                            "\u2713 Save successfully into 'AssetsLiveStatus'.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "\u2717 Failed to save.");
                    }
                }
            }
        }
        finally
        {
            consumer.Close();
            Console.WriteLine(
                "Consumer Closed.");
        }
    }   
}