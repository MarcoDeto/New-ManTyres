using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Utils
{
    public static class LoggerHelper
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static string GetActualMethodName([CallerMemberName] string name = null) => name;
        public static void CheckArgument(params object[] args)
        {
            StringBuilder str = new StringBuilder("Parameters: ");
            foreach (var item in args)
            {
                str.Append(item ?? "null" + " ");
                argumentBeingNull(item);
            }
            _logger.Trace(str.ToString());
        }
        private static void argumentBeingNull<T>(T argumentValue)
        {
            if (argumentValue != null)
            {
                return;
            }
            _logger.Warn("Null parameter intercepted");
            //throw new ArgumentNullException();
        }
    }
}
