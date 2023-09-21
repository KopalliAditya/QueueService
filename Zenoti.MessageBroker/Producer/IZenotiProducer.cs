using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenoti.MessageBroker.Producer
{
    public interface IZenotiProducer<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// Produce the events in the message broker async.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task ProduceAsync(TKey key, TValue value);

        /// <summary>
        /// Clears buffers for the stream.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        int Flush(TimeSpan? timeout);
    }
}
