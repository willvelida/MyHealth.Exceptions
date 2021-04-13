using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealth.Exceptions.Helpers
{
    public class FunctionOptions
    {
        public string SendGridApiSetting { get; set; }
        public string ExceptionRecipientSetting { get; set; }
        public string ExceptionEmailSetting { get; set; }
        public string ServiceBusConnectionSetting { get; set; }
    }
}
