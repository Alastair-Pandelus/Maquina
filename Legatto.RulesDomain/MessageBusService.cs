using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Legatto.RulesDomain
{
    class MessageBusService : IMessageBusService
    {
        public void Publish(object message)
        {
            Debug.WriteLine(message.ToString());
        }
    }
}
