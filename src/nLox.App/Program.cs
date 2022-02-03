using nLox.Interpreter;

using System;
using System.Text;

namespace nLox.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage nLox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                Lox.RunFile(args[0]);
            }
            else
            {
                Lox.RunPrompt();
            }
        }
    }
}