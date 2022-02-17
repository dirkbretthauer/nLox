namespace nLox.Interpreter

{
  public abstract class Expression
  {
    public interface IVisitor<R>
    {
      R VisitBinaryExpression(Binary expression);
      R VisitGroupingExpression(Grouping expression);
      R VisitLiteralExpression(Literal expression);
      R VisitUnaryExpression(Unary expression);
    }
    public class Binary : Expression
    {
      public Expression Left { get; }
      public Token OperatorToken { get; }
      public Expression Right { get; }

      public Binary(Expression Left, Token OperatorToken, Expression Right)
      {
        this.Left = Left;
        this.OperatorToken = OperatorToken;
        this.Right = Right;
      }

      public override R Accept<R>(IVisitor<R> visitor)
      {
        return visitor.VisitBinaryExpression(this);
      }
    }
    public class Grouping : Expression
    {
      public Expression Expression { get; }

      public Grouping(Expression Expression)
      {
        this.Expression = Expression;
      }

      public override R Accept<R>(IVisitor<R> visitor)
      {
        return visitor.VisitGroupingExpression(this);
      }
    }
    public class Literal : Expression
    {
      public Object? Value { get; }

      public Literal(Object? Value)
      {
        this.Value = Value;
      }

      public override R Accept<R>(IVisitor<R> visitor)
      {
        return visitor.VisitLiteralExpression(this);
      }
    }
    public class Unary : Expression
    {
      public Token OperatorToken { get; }
      public Expression Right { get; }

      public Unary(Token OperatorToken, Expression Right)
      {
        this.OperatorToken = OperatorToken;
        this.Right = Right;
      }

      public override R Accept<R>(IVisitor<R> visitor)
      {
        return visitor.VisitUnaryExpression(this);
      }
    }

    public abstract R Accept<R>(IVisitor<R> visitor);
  }
}
