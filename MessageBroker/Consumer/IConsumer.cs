using System.Threading;
using System.Threading.Tasks;

namespace Zenoti.MessageBroker.Consumer
{
    public interface IConsumer
    {
        /// <summary>
        /// Consume the message async.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        Task ConsumeAsync(CancellationToken stoppingToken);
    }
}
