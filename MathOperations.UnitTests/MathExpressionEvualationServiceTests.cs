using MathOperations.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace MathOperations.UnitTests
{
  public class MathExpressionEvualationServiceTests
  {
    [Test]
    public void Throws_error_if_string_expression_is_mull()
    {
      // Arrange
      MathExpressionEvualationService service = new MathExpressionEvualationService(Mock.Of<ILogger<MathExpressionEvualationService>>());

      // Act
      void evaluateDelegate() => service.Evaluate(null);

      // Assert
      Exception ex = Assert.Throws<ArgumentException>(() => evaluateDelegate());
      Assert.That(ex.Message, Is.EqualTo("Expression is empty"));
    }

    [Test]
    public void Throws_error_if_string_expression_is_empty()
    {
      // Arrange
      MathExpressionEvualationService service = new MathExpressionEvualationService(Mock.Of<ILogger<MathExpressionEvualationService>>());

      // Act
      void evaluateDelegate() => service.Evaluate(string.Empty);

      // Assert
      Exception ex = Assert.Throws<ArgumentException>(() => evaluateDelegate());
      Assert.That(ex.Message, Is.EqualTo("Expression is empty"));
    }

    [TestCase("+1-")]
    [TestCase("ab1g")]
    [TestCase("+2")]
    [TestCase("2/-+2")]
    public void Throws_error_if_string_expression_is_not_valid(string expression)
    {
      // Arrange
      MathExpressionEvualationService service = new MathExpressionEvualationService(Mock.Of<ILogger<MathExpressionEvualationService>>());

      // Act
      void evaluateDelegate() => service.Evaluate(expression);

      // Assert
      Exception ex = Assert.Throws<ArgumentException>(() => evaluateDelegate());
      Assert.That(ex.Message, Is.EqualTo("Syntax error"));
    }

    [TestCase("1+1", 1 + 1)]
    [TestCase("1-1", 1 - 1)]
    [TestCase("1*1", 1 * 1)]
    [TestCase("1/1", 1 / 1)]
    [TestCase("1-91", 1 - 91)]
    [TestCase("1+2*3-4/3", 1.0 + (2.0 * 3.0) - (4.0 / 3.0))]
    public void Get_result_from_string_expression(string expression, decimal expectedResult)
    {
      // Arrange
      MathExpressionEvualationService service = new MathExpressionEvualationService(Mock.Of<ILogger<MathExpressionEvualationService>>());

      // Act
      decimal result = service.Evaluate(expression);

      // Assert
      Assert.That(expectedResult, Is.EqualTo(result).Within(0.00000001));
    }

    [TestCase("0/0")]
    [TestCase("1/0")]
    [TestCase("2+4-8/0")]
    [TestCase("1+3/0*7")]
    public void Throws_error_when_dividing_by_zero(string expression)
    {
      // Arrange
      MathExpressionEvualationService service = new MathExpressionEvualationService(Mock.Of<ILogger<MathExpressionEvualationService>>());

      // Act
      void evaluateDelegate() => service.Evaluate(expression);

      // Assert
      Exception ex = Assert.Throws<DivideByZeroException>(() => evaluateDelegate());
    }

    [TestCase("79228162514264337593543950335+10")]
    public void Throws_error_when_number_is_too_big(string expression)
    {
      // Arrange
      MathExpressionEvualationService service = new MathExpressionEvualationService(Mock.Of<ILogger<MathExpressionEvualationService>>());

      // Act
      void evaluateDelegate() => service.Evaluate(expression);

      // Assert
      Exception ex = Assert.Throws<OverflowException>(() => evaluateDelegate());
    }
  }
}