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

        string bootstrapServers = 
            configuration["Kafka:BootstrapServers"]!;

        string uavTopic = configuration["Kafka:Topics:UAV"]
            ?? throw new InvalidOperationException(
                "\u2717 Failed: Topic's name of 'uav' is missing.");

        string sensorTopic = configuration["Kafka:Topics:PerimeterSensor"]
            ?? throw new InvalidOperationException(
                "\u2717 Failed: Topic's name of 'PerimeterSensor' is missing.");


        // =================== Load Files ===================
        string reportsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "field_reports.json");

        var loader = new DataLoaderService();

        var reports = loader.Load<Report>(reportsPath);

        Console.WriteLine(
            $"\u2714  'field_reports.json' loaded successfully.");

        // ============= Ensure If Topics is Exist ============
        var topicManager = 
            new KafkaTopicManager(bootstrapServers);

        await topicManager.EnsureIfTopicExitsAsync(uavTopic);
        await topicManager.EnsureIfTopicExitsAsync(sensorTopic);

        // =============== Send The Reports ==============
        using var producer = 
            new ProducerService(bootstrapServers);

        foreach (Report report in reports)
        {
            if (report.AssetType == "PerimeterSensor")
            {
                await producer.ProduceAsync<Report>(
                    sensorTopic, report);
            }

            if (report.AssetType == "UAV")
            {
                await producer.ProduceAsync<Report>(
                    uavTopic, report);
            }
        }
        producer.Flush();
        Console.WriteLine("\u2714 All reports sent successfully");
    }
}
