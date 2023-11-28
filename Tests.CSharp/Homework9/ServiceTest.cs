using System.Linq.Expressions;
using Hw9;
using Hw9.ErrorMessages;
using Hw9.Services.MathCalculator;
using Hw9.Services.ParserAndValidator;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.RunLogic.Attributes;
using Xunit;
namespace Tests.CSharp.Homework9;

public class ServicesTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ServicesTest(WebApplicationFactory<Program> fixture)
    {
        _client = fixture.CreateClient();
    }

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
    public async Task ParseToPosrfixForm(string expression, string result)
    {
        var response = Parser.ConvertToPostfixForm(expression);

        Assert.Equal(result, response);
    }
    [Theory]
    [InlineData(ExpressionType.Add, 3.0, 2.0, 5.0)]
    [InlineData(ExpressionType.Subtract, 3.0, 2.0, 1.0)]
    [InlineData(ExpressionType.Multiply, 3.0, 2.0, 6.0)]
    [InlineData(ExpressionType.Divide, 4.0, 2.0, 2.0)]
    public void Calculate_ShouldReturnCorrectResult(ExpressionType binExpr, double constLeft, double constRight, double expectedResult)
    {
        // Arrange
        var result = ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight);

        // Assert
        Assert.Equal(expectedResult, result);
    }
    [Homework(Homeworks.HomeWork9)]
    public void Calculate_ShouldThrowExceptionOnDivisionByZero()
    {
        // Arrange
        var binExpr = ExpressionType.Divide;
        var constLeft = 3.0;
        var constRight = 0.0;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight));
        Assert.Equal(MathErrorMessager.DivisionByZero, exception.Message);
    }
    
    [Homework(Homeworks.HomeWork9)]
    public void Calculate_ShouldThrowExceptionOnUnknownCharacter()
    {
        // Arrange
        var binExpr = (ExpressionType)100; 
        var constLeft = 3.0;
        var constRight = 2.0;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight));
        Assert.Equal(MathErrorMessager.UnknownCharacter, exception.Message);
    }
    
}