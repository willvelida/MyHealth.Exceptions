using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MyHealth.Exceptions.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly SendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;

        public SendGridService(
            SendGridClient sendGridClient,
            IConfiguration configuration)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
        }

        public async Task SendExceptionEmail(Exception exception)
        {
            var exceptionMessageContent = $"An Exception has been thrown in: {exception.Source}\n" +
                    $"Exception thrown is: {exception.Message}\n" +
                    $"Stack Trace: {exception.StackTrace}";

            var exceptionEmailMessage = new SendGridMessage
            {
                From = new EmailAddress(_configuration["ExceptionRecipientEmail"], _configuration["ExceptionRecipientName"]),
                Subject = "MyHealth: Exception thrown",
                PlainTextContent = exceptionMessageContent,
                HtmlContent = exceptionMessageContent
            };
            exceptionEmailMessage.AddTo(_configuration["ExceptionRecipientEmail"]);

            await _sendGridClient.SendEmailAsync(exceptionEmailMessage);
        }
    }
}
