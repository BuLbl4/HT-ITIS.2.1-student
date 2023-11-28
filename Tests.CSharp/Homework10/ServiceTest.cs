using System.Linq.Expressions;
using Hw10;
using Hw10.ErrorMessages;
using Hw10.Services.MathCalculator;
using Hw10.Services.MathCalculator.ParserAndValidator;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
namespace Tests.CSharp.Homework10;

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
    public void ParseToPosrfixForm(string expression, string expected)
    {
        var result = Parser.ConvertToPostfixForm(expression);

        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(ExpressionType.And,1,1 )]
    [InlineData(ExpressionType.Block,1,1 )]
    async Task ExpressionVisitorCalculateTest(ExpressionType expressionType, double first,double second)
    {
        var response = () => (object)ExpressionTreeVisitor.Calculate(expressionType,first,second);
        var exception = Assert.Throws<Exception>(response);
        Assert.Equal(MathErrorMessager.UnknownCharacter, exception.Message);
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