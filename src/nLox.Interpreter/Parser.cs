using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nLox.Interpreter
{
    internal class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public Expression? Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseException)
            {
                return null;
            }
        }

        private Expression Expression()
        {
            return Equality();
        }

        private Expression Equality()
        {
            Expression expression = Comparison();
            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token operatorToken = Previous();
                Expression right = Comparison();
                expression = new Expression.Binary(expression, operatorToken, right);
            }

            return expression;
        }

        private Expression Comparison()
        {
            Expression expression = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token operatorToken = Previous();
                Expression right = Term();
                expression = new Expression.Binary(expression, operatorToken, right);
            }

            return expression;
        }

        private Expression Term()
        {
            Expression expression = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token operatorToken = Previous();
                Expression right = Factor();
                expression = new Expression.Binary(expression, operatorToken, right);
            }

            return expression;
        }

        private Expression Factor()
        {
            Expression expression = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token operatorToken = Previous();
                Expression right = Unary();
                expression = new Expression.Binary(expression, operatorToken, right);
            }

            return expression;
        }

        private Expression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token operatorToken = Previous();
                Expression right = Unary();
                return new Expression.Unary(operatorToken, right);
            }

            return Primary();
        }

        private Expression Primary()
        {
            if (Match(TokenType.FALSE))
                return new Expression.Literal(false);

            if (Match(TokenType.TRUE))
                return new Expression.Literal(true);

            if (Match(TokenType.NIL))
                return new Expression.Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Expression.Literal(Previous().Literal);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expression = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
                return new Expression.Grouping(expression);
            }

            throw Error(Peek(), "Expect expression.");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw Error(Peek(), message);
        }

        private void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON)
                    return;

                switch(Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FOR:
                    case TokenType.FUN:
                    case TokenType.IF:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                    case TokenType.VAR:
                    case TokenType.WHILE:
                        return;
                }

                Advance();
            }
        }     

        private ParseException Error(Token token, string message)
        {
            Lox.Error(token, message);
            return new ParseException();
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
                current++;

            return Previous();
        }

        private Token Previous()
        {
            return tokens[current-1];
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
                return false;

            return Peek().Type == type;
        }

        private class ParseException : Exception
        {

            public ParseException()
            {
            }

            public ParseException(string message)
                : base(message)
            {
            }

            public ParseException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
