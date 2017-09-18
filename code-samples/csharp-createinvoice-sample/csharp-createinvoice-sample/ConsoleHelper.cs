using System;

namespace CreateInvoice
{
    internal static class ConsoleHelper
    {
        #region Internal Methods

        internal static void WriteErrorLine(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        #endregion
    }
}
