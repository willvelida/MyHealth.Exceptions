using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MyHealth.Exceptions.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MyHealth.Exceptions.Functions
{
    public class ProcessExceptionMessage
    {
        private readonly ILogger<ProcessExceptionMessage> _logger;
        private readonly ISendGridService _sendGridService;

        public ProcessExceptionMessage(
            ILogger<ProcessExceptionMessage> logger,
            ISendGridService sendGridService)
        {
            _logger = logger;
            _sendGridService = sendGridService;
        }

        [FunctionName(nameof(ProcessExceptionMessage))]
        public async Task Run([ServiceBusTrigger("myhealthexceptiontopic", "myhealthexceptionsubscription", Connection = "FunctionOptions:ServiceBusConnectionString")] string incomingExceptionMessage)
        {
            try
            {
                var exception = JsonConvert.DeserializeObject<Exception>(incomingExceptionMessage);

                await _sendGridService.SendExceptionEmail(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {(nameof(ProcessExceptionMessage))}: {ex.Message}");
            }
        }
    }
}
