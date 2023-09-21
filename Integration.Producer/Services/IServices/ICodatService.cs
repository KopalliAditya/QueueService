using Integration.Models;
using System.Threading.Tasks;

namespace Integration.Producer.Services.IServices
{
    public interface ICodatService
    {
        /// <summary>
        /// Perform Action using the codat request and codat body
        /// </summary>
        /// <param name="codatRequest"></param>
        /// <param name="codatBody"></param>
        /// <returns></returns>
        bool PerformAction(CodatEvent codatEvent);
    }
}
