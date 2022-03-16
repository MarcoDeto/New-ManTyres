using Microsoft.Extensions.Logging;
using System;

namespace ManTyres.COMMON.Utils
{
    public class ErrorsManager 
    {
        public virtual DataBridge<TOut> LogAndGetDataBridge<TIn, TOut>(ILogger<TIn> logger, Exception exception, string message = "Unexpected exception", DataBridge<TOut>? dataBridge = null)
        {
            logger.LogError(exception, message);
            var newDataBridge = dataBridge ?? new DataBridge<TOut>();
            newDataBridge.Message = message;
            return newDataBridge;
        }
    }
}
