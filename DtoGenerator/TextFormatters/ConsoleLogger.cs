using System;

namespace TextFormatters
{
    public sealed class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
