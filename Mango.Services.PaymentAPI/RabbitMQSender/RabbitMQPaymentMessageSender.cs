using Mango.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.PaymentAPI.RabbitMQSender
{
    public class RabbitMQPaymentMessageSender : IRabbitMQPaymentMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private const string ExchangeName = "PublishSubscribePaymentUpdate_Exchange";
        public RabbitMQPaymentMessageSender()
        {
            _hostname= "localhost";
            _password= "guest";
            _username= "guest";
        }
        public void SendMessage(BaseMessage message)
        {
            if (ConnectionExists())
            {

                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, durable: false);
                //channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: ExchangeName,"", basicProperties: null, body: body);
            }
        }

        private void CreateConection()
        {
            try 
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password,
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex) 
            {
                //log  exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConection();
            return _connection != null;
        }
    }
}
