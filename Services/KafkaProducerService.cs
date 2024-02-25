using Confluent.Kafka;
using KafkaTestProducer.Models;
using System.Text.Json;

namespace KafkaTestProducer.Services
{
    public class KafkaProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly ProducerConfig _producerConfig;

        public KafkaProducerService(IConfiguration configuration)
        {
            _configuration = configuration;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _configuration.GetSection("KafkaConfig").GetSection("BootstrapServer").Value
            };
        }

        public async Task<string> SendMessage(User user)
        {
            var topic = _configuration.GetSection("KafkaConfig").GetSection("TopicName").Value;

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
                {
                    var message = JsonSerializer.Serialize(user);
                    var result = await producer.ProduceAsync(topic: topic, new() { Value = message });

                    var statusResult = result.Status.ToString();

                    return $"{statusResult} - {message}";
                }
            }
            catch (Exception e)
            {
                return $"Error while sending message {e.Message}";
            }
        }
    }
}
