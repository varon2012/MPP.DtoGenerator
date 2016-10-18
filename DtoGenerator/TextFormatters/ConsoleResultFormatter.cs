using System;

namespace TextFormatters
{
    public sealed class ConsoleResultFormatter : IResultFormatter
    {
        public void Format(string result)
        {
            Console.WriteLine(result);
        }
    }
}
