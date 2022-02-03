using System;
using System.Text;

namespace nLox.Expressions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: nLox.Expressions <output directory>");
                Environment.Exit(64);
            }

            string outdir = args[0];
            DefineAst(outdir, "Expression", new[]
            {
                "Binary     : Expression left, Token operatorToken, Expression right",
                "Grouping   : Expression expression",
                "Literal    : Object value",
                "Unary      : Token operatorToken, Expression right",
            });
        }

        private static void DefineAst(string outdir, string baseName, IEnumerable<string> types)
        {
            string path = $"{outdir}/{baseName}.cs";
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine("namespace nLox.Interpreter");
                writer.WriteLine("");
                writer.WriteLine("{");
                writer.WriteLine($"  public abstract class {baseName}");
                writer.WriteLine("  {");

                DefineVisitor(writer, baseName, types);

                foreach (var type in types)
                {
                    string className = type.Split(":")[0].Trim();
                    string fields = type.Split(":")[1].Trim();
                    DefineType(writer, baseName, className, fields);
                }

                // The base Accept method
                writer.WriteLine();
                writer.WriteLine("    public abstract R Accept<R>(IVisitor<R> visitor);");

                writer.WriteLine("  }");
                writer.WriteLine("}");
            }
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, IEnumerable<string> types)
        {
            writer.WriteLine("    public interface IVisitor<R>");
            writer.WriteLine("    {");

            foreach(var type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine($"      R Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            writer.WriteLine("    }");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fields)
        {
            var fieldList = fields.Split(", ");

            writer.WriteLine($"    public class {className} : {baseName}");
            writer.WriteLine("    {");

            // Fields
            foreach(var field in fieldList)
            {
                writer.WriteLine($"      private {field.Trim()};");
            }

            writer.WriteLine();

            // Constructor
            writer.WriteLine($"      public {className}({fields.Trim()})");
            writer.WriteLine("      {");

            foreach(var field in fieldList)
            {
                string name = field.Split(' ')[1].Trim();
                writer.WriteLine($"        this.{name} = {name};");
            }
            writer.WriteLine("      }");

            // Visitor pattern
            writer.WriteLine();
            writer.WriteLine($"      public override R Accept<R>(IVisitor<R> visitor)");
            writer.WriteLine("      {");
            writer.WriteLine($"        return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("      }");

            writer.WriteLine("    }");
        }
    }
}