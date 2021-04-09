using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealth.Exceptions.Helpers
{
    public class FunctionOptions
    {
        public string SendGridAPIKey { get; set; }
        public string ExceptionRecipientEmail { get; set; }
        public string ExceptionRecipientName { get; set; }
        public string ServiceBusConnectionString { get; set; }
    }
}
