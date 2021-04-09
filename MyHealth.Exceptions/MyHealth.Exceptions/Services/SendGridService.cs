using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyHealth.Exceptions.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MyHealth.Exceptions.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly SendGridClient _sendGridClient;
        private readonly FunctionOptions _settings;

        public SendGridService(
            SendGridClient sendGridClient,
            IOptions<FunctionOptions> options)
        {
            _sendGridClient = sendGridClient;
            _settings = options.Value;
        }

        public async Task SendExceptionEmail(Exception exception)
        {
            var exceptionMessageContent = $"An Exception has been thrown in: {exception.Source}\n" +
                    $"Exception thrown is: {exception.Message}\n" +
                    $"Stack Trace: {exception.StackTrace}";

            var exceptionEmailMessage = new SendGridMessage
            {
                From = new EmailAddress(_settings.ExceptionRecipientEmail, _settings.ExceptionRecipientName),
                Subject = "MyHealth: Exception thrown",
                PlainTextContent = exceptionMessageContent,
                HtmlContent = exceptionMessageContent
            };
            exceptionEmailMessage.AddTo(_settings.ExceptionRecipientEmail);

            await _sendGridClient.SendEmailAsync(exceptionEmailMessage);
        }
    }
}
