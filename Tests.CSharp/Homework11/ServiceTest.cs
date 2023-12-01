using System.Linq.Expressions;
using Hw11.ErrorMessages;
using Hw11.Services.MathCalculator.ExpressionBuilder;
using Hw11.Services.MathCalculator.ParseAndValidate;

namespace Tests.CSharp.Homework11;

public class ServiceTest
{
    [Theory]
    [InlineData("-2 - 7", "-2 7 - ")]
    [InlineData("(-2) - (-7)", "-2 -7 - ")]
    [InlineData("(-2 + 2)", "-2 2 + ")]
    [InlineData("-4 - (-4)", "-4 -4 - ")]
    [InlineData("1 - 1 - 2", "1 1 - 2 - ")]
    [InlineData("-3 + 5", "-3 5 + ")]
    [InlineData("(-3) + 5", "-3 5 + ")]
    [InlineData("-3 + (5 + 2)", "-3 5 2 + + ")]
    [InlineData("-3 + (5 + (-2))", "-3 5 -2 + + ")]
    [InlineData("-3 + (-5 + 2)", "-3 -5 2 + + ")]
    [InlineData("-3 + (-5 + (-2))", "-3 -5 -2 + + ")]
    public void ParseToPosrfixForm(string expression, string expected)
    {
        var result = Parser.ConvertToPostfixForm(expression);

        Assert.Equal(expected, result);
    }
    [Fact]
    public void Calculate_ShouldThrowExceptionOnUnknownCharacter()
    {
        // Arrange
        var binExpr = (ExpressionType)100; // Using a value that is not recognized
        var constLeft = 3.0;
        var constRight = 2.0;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight));
        Assert.Equal(MathErrorMessager.UnknownCharacter, exception.Message);
    }
}