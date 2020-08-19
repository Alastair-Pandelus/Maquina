using System;
using System.Collections.Generic;
using System.Text;

namespace Legatto.RulesDomain
{
    public interface IMessageBusService
    {
        public void Publish(object message);
    }
}
