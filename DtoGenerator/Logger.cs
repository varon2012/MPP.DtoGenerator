using System;

namespace DtoGenerator
{
    internal class Logger
    {
        private string ClassName { get; }

        internal static Logger GetLogger(string className)
        {
            return new Logger(className);
        }

        private Logger(string className)
        {
            ClassName = className;
        }

        internal void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {ClassName}: {message}");
        }

    }
}
