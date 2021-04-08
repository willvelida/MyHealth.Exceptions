using System;
using System.Threading.Tasks;

namespace MyHealth.Exceptions.Services
{
    public interface ISendGridService
    {
        /// <summary>
        /// Sends an email containing the provided exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        Task SendExceptionEmail(Exception exception);
    }
}
