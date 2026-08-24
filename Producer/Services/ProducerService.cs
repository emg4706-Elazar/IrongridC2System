using System;
using System.Text.Json;
using Confluent.Kafka;

namespace Producer.Services;

public class ProducerService : IDisposable
{
    private readonly IProducer<Null, string> _producer;

    public ProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = bootstrapServers
        };

        _producer = 
            new ProducerBuilder<Null, string>(config)
            .Build();

    }

    public async Task ProduceAsync<T>(
        string topicName, T entity)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        var value = JsonSerializer.Serialize(entity);

        var message = new Message<Null, string>
        {
            Value = value
        };

        await _producer.ProduceAsync(topicName, message);
    }

    public void Flush()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}


