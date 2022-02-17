using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nLox.Interpreter
{
    internal class AstPrinter : Expression.IVisitor<string>
    {
        static void Main(string[] args)
        {
            Expression expression = new Expression.Binary(
                new Expression.Unary(
                    new Token(TokenType.MINUS, "-", null, 1),
                    new Expression.Literal(123)),
                new Token(TokenType.STAR, "*", null, 1),
                new Expression.Grouping(
                    new Expression.Literal(45.67)));

            Console.WriteLine(new AstPrinter().Print(expression));
        }

        public string Print(Expression expression)
        {
            return expression.Accept(this);
        }

        public string VisitBinaryExpression(Expression.Binary expression)
        {
            return Parenthesize(expression.OperatorToken.Lexeme, expression.Left, expression.Right);
        }

        public string VisitGroupingExpression(Expression.Grouping expression)
        {
            return Parenthesize("group", expression.Expression);
        }

        public string VisitLiteralExpression(Expression.Literal expression)
        {
            if (expression.Value == null)
                return "nil";

            return expression.Value.ToString()!;
        }

        public string VisitUnaryExpression(Expression.Unary expression)
        {
            return Parenthesize(expression.OperatorToken.Lexeme, expression.Right);
        }

        private string Parenthesize(string name, params Expression[] expressions)
        {
            var builder = new StringBuilder();
            
            builder.Append("(").Append(name);
            foreach(var expression in expressions)
            {
                builder.Append(" ");
                builder.Append(expression.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
