using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator.Types;

namespace Generator
{
    public class Logger
    {
        private static readonly Lazy<Logger> lazy =
            new Lazy<Logger>(() => new Logger());

        public static Logger GetInstance()
        {
            return lazy.Value;
        }

        private Logger()
        {

        }

        public List<string> ExceptionList { get; private set; }

        public void Log(string message)
        {
            if (ExceptionList != null)
                ExceptionList.Add(message);
            else
            {
                ExceptionList = new List<string>();
                ExceptionList.Add(message);
            }
        }
    }
}
