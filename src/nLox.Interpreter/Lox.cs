using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nLox.Interpreter
{
    public class Lox
    {
        static bool hadError = false;

        public static void RunFile(string path)
        {
            Run(File.ReadAllText(path, Encoding.UTF8));

            if (hadError)
            {
                Environment.Exit(65);
            }
        }

        public static void RunPrompt()
        {
            while (true)
            {
                Console.WriteLine("> ");
                string? line = Console.ReadLine();
                if (line == null)
                    break;

                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {
            var scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        internal static void Error(int line, string message)
        {
            Report(line, string.Empty, message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
            hadError = true;
        }
    }
}
