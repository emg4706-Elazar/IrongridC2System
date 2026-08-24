using System;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Producer.Services;

public class KafkaTopicManager
{
    private readonly string _bootstrapServers;
    public KafkaTopicManager(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;
    }

    public async Task EnsureIfTopicExitsAsync(
        string topicName,
        int numPartitions = 1,
        short replicationFactor = 1)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = _bootstrapServers,
            ClientId = "irongrid-producer"
        };

        try
        {
            using var adminClient =
                new AdminClientBuilder(config).Build();

            await adminClient.CreateTopicsAsync(
            [
                new TopicSpecification()
                {
                    Name = topicName,
                    NumPartitions = numPartitions,
                    ReplicationFactor = replicationFactor
                }
            ]);

            Console.WriteLine(
                $"\u2714  Topic '{topicName}' created successfully.");
        }
        catch (CreateTopicsException ex)
        when (ex.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
        {
            Console.WriteLine(
                $"\u2714 Topic '{topicName}' already exists.");
        }
    }
}
