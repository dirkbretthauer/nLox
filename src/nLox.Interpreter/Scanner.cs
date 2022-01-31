namespace nLox.Interpreter
{
    public class Scanner
    {
        private string source;
        private List<Token> tokens = new List<Token> ();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        
        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> ScanTokens()
        {
            while(!IsAtEnd())
            {
                // We are at the beginning of the next lexeme
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, string.Empty, null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                default:
                    Lox.Error(line, "Unexpected chracter.");
                    break;
            }
        }

        private char Advance()
        {
            return source.ElementAt(current++);
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object? literal)
        {
            string text = source.Substring(start, current);
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
                return false;
            if (source.ElementAt(current) != expected)
                return false;

            current++;
            return true;
        }
    }
}