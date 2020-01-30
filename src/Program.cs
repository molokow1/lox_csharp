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

    class Lox
    {
        static bool hadError = false;
        public static void runFile(string filePath)
        {
            string source = System.IO.File.ReadAllText(filePath);
            run(source);
            if (hadError)
            {
                Environment.Exit(65);
            }
            // Console.WriteLine("Got file " + filePath);
        }

        public static void runPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                run(Console.ReadLine());
                hadError = false;
            }
        }

        private static void run(string source)
        {
            Scanner scanner = new Scanner(source);
            var tokens = scanner.scanTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token.ToString());
            }

            // Console.WriteLine(source);
        }

        public static void error(int line, string message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            hadError = true;
        }
    }
}
