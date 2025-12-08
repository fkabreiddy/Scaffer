using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scaffer.Core
{
    public static class ConsoleHelper
    {
        public static void WriteLineColor(string text, ConsoleColor color)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = prev;
        }
    }
}