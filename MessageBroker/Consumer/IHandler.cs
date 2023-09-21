using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenoti.MessageBroker.Consumer
{
    /// <summary>
    /// Handler to consume events from the message broker
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IHandler<in TKey, in TValue>
    {
        Task HandleAsync(TKey key, TValue value);
    }
}
