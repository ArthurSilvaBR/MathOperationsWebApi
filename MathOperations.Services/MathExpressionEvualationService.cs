using MathOperations.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace MathOperations.Services
{
  public class MathExpressionEvualationService(ILogger<MathExpressionEvualationService> logger) : IMathExpressionEvualationService
  {
    struct Token
    {
      public string Text;
      public decimal? Result;
    }

    private const int MULTIPLY_AND_DIVIDE_OPERATOR_PRECEDENCE = 2;
    private const int ADD_AND_SUBTRACT_OPERATOR_PRECEDENCE = 1;

    private readonly ILogger _logger = logger;

    private List<Token> TokenList { get; set; }
    private string Expression { get; set; }

    public decimal Evaluate(string expression)
    {
      Expression = expression;
      TokenList = ParseTokens(expression);
      
      EvaluateStep(MULTIPLY_AND_DIVIDE_OPERATOR_PRECEDENCE);

      return TokenList[0].Result ?? 0;
    }

    private void EvaluateStep(int opPrecedenceLevel)
    {
      var idxOperator = IndexOfTokenUsingOperator(opPrecedenceLevel);
      if (idxOperator == -1)
      {
        if (opPrecedenceLevel == MULTIPLY_AND_DIVIDE_OPERATOR_PRECEDENCE)
        {
          opPrecedenceLevel = ADD_AND_SUBTRACT_OPERATOR_PRECEDENCE;
          
          EvaluateStep(opPrecedenceLevel);

          return;
        }
        else
        {
          SyntaxError();
        }
      }
      // if operator is at invalid position (beginning or end of expression)
      else if (idxOperator == 0 || idxOperator == TokenList.Count - 1)
      {
        SyntaxError();
      }

      var tkResult = ExecuteOperation(TokenList[idxOperator - 1], TokenList[idxOperator], TokenList[idxOperator + 1]);

      //replaces the operands and operators by this result - and keep going on evaluation;
      var index = idxOperator - 1;
      TokenList.Insert(index, tkResult);

      TokenList.RemoveAt(index + 1);
      TokenList.RemoveAt(index + 1);
      TokenList.RemoveAt(index + 1);

      if (TokenList.Count > 1)
        EvaluateStep(opPrecedenceLevel);
    }

    private Token ExecuteOperation(Token tokenOperand1, Token tokenOperator, Token tokenOperand2)
    {
      decimal op1 = TokenDecimalValue(tokenOperand1);
      decimal op2 = TokenDecimalValue(tokenOperand2);
      Token tokenResult = new Token();

      switch (tokenOperator.Text)
      {
        case "+": tokenResult.Result = op1 + op2; break;
        case "-": tokenResult.Result = op1 - op2; break;
        case "*": tokenResult.Result = op1 * op2; break;
        case "/": tokenResult.Result = op1 / op2; break;
      }

      return tokenResult;
    }

    private decimal TokenDecimalValue(Token token)
    {
      if (token.Result.HasValue) return token.Result.Value;

      decimal value = 0;
      if (!decimal.TryParse(token.Text, out value))
      {
        SyntaxError();
      }

      return value;
    }

    private void SyntaxError()
    {
      _logger.LogError($"[{nameof(MathExpressionEvualationService)}] - Syntax error - Expression: {Expression}");

      throw new ArgumentException("Syntax error");
    }

    private void EmptyExpressionError()
    {
      _logger.LogError($"[{nameof(MathExpressionEvualationService)}] - Expression is empty");

      throw new ArgumentException("Expression is empty");
    }

    private int IndexOfTokenUsingOperator(int operatorPrecedence)
    {
      for (int i = 0; i < TokenList.Count; i++)
      {
        if (operatorPrecedence == MULTIPLY_AND_DIVIDE_OPERATOR_PRECEDENCE && (TokenList[i].Text == "*" || TokenList[i].Text == "/")) return i;
        if (operatorPrecedence == ADD_AND_SUBTRACT_OPERATOR_PRECEDENCE && (TokenList[i].Text == "+" || TokenList[i].Text == "-")) return i;
      }

      return -1;
    }

    private List<Token> ParseTokens(string expression)
    {
      expression = ExpressionCleanup(expression);
      if (string.IsNullOrEmpty(expression)) EmptyExpressionError();

      List<Token> result = new List<Token>();

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < expression.Length; i++)
      {
        sb.Append(expression[i]);

        if (i == expression.Length - 1 || IsOperator(expression[i]) || IsOperator(expression[i + 1]))
        {
          Token token = new Token();
          token.Text = sb.ToString();
          result.Add(token);
          sb.Clear();
        }
      }

      return result;
    }

    private string ExpressionCleanup(string expression)
    {
      if (!string.IsNullOrEmpty(expression))
        expression = expression.Replace(" ", "");

      return expression;
    }

    private bool IsOperator(char ch)
    {
      return ch == '+' || ch == '-' || ch == '*' || ch == '/';
    }
  }
}
