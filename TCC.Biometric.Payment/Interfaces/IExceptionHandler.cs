using Orchestrator.Core.Interfaces;

namespace TCC.DigitalID.Services.Interfaces
{
    public interface IExceptionHandler
    {
        IErrorResponse Handle(Exception exception);
    }

}