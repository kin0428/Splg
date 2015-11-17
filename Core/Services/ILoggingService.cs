using System.Web.Mvc;

namespace Splg.Core.Services
{
    public interface ILoggingService
    {
        void WriteLog(ControllerContext controllerContext);
    }
}