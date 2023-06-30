using System.Text;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Mango.MessageBus;
using Azure.Messaging.ServiceBus;
using PaymentProcessor;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;

namespace Mango.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        public readonly string serviceBusConnectionString;
        public readonly string subscriptionPayment;
        public readonly string orderPaymentProcessTopic;
        public readonly string orderupdatepaymentresulttopic;

        private ServiceBusProcessor orderPaymentProcessor;
        private readonly IProcessPayment _processPayment;
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        public AzureServiceBusConsumer(IProcessPayment processPayment, IConfiguration configuration, IMessageBus messageBus) 
        {  
            _processPayment = processPayment;
            _configuration = configuration;
            _messageBus = messageBus;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionPayment = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
            orderupdatepaymentresulttopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessTopic, subscriptionPayment);
        }

        public async Task Start()
        {
            orderPaymentProcessor.ProcessMessageAsync += ProcessPayments;
            orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await orderPaymentProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await orderPaymentProcessor.StopProcessingAsync();
            await orderPaymentProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args) 
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessPayments(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email
            };

            try 
            {
                await _messageBus.PublishMessage(updatePaymentResultMessage, orderupdatepaymentresulttopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex) 
            {
                throw;
            }
        }


    }
}
