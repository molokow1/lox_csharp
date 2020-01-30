using System;

namespace lox_csharp_interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: cslox [script]");
            }
            else if (args.Length == 1)
            {
                Lox.runFile(args[0]);
            }
            else
            {
                Lox.runPrompt();
            }
        }
    }
}
