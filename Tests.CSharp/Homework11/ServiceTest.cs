using System.Linq.Expressions;
using Hw11.ErrorMessages;
using Hw11.Services.MathCalculator;
using Hw11.Services.MathCalculator.ExpressionBuilder;
using Hw11.Services.MathCalculator.ParseAndValidate;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.RunLogic.Attributes;
using Xunit;
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
        var result = ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight);

        Assert.Equal(expectedResult, result);
    }
    [Homework(Homeworks.HomeWork11)]
    public void Calculate_ShouldThrowExceptionOnDivisionByZero()
    {
        
        var binExpr = ExpressionType.Divide;
        var constLeft = 3.0;
        var constRight = 0.0;

        var exception = Assert.Throws<Exception>(() => ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight));
        Assert.Equal(MathErrorMessager.DivisionByZero, exception.Message);
    }
    
    [Homework(Homeworks.HomeWork11)]
    public void Calculate_ShouldThrowExceptionOnUnknownCharacter()
    {
        var binExpr = (ExpressionType)100; 
        var constLeft = 3.0;
        var constRight = 2.0;

        var exception = Assert.Throws<Exception>(() => ExpressionTreeVisitor.Calculate(binExpr, constLeft, constRight));
        Assert.Equal(MathErrorMessager.UnknownCharacter, exception.Message);
    }
    
    [Theory]
    [InlineData("2 3 +", typeof(Expression))]
    [InlineData("5.5 2.2 *", typeof(Expression))]
    [InlineData("7.5 3.2 -", typeof(Expression))]
    [InlineData("10.0 2.0 /", typeof(Expression))]
    public void CreateExpressionTree_ShouldPushConstants(string input, Type expectedType)
    {
        var expression = ExpressionTreeBuilder.CreateExpressionTree(input);

        Assert.IsType(expectedType, expression);
    }

    [Theory]
    [InlineData("2 3 +", typeof(BinaryExpression))]
    [InlineData("5.5 2.2 *", typeof(BinaryExpression))]
    [InlineData("7.5 3.2 -", typeof(BinaryExpression))]
    [InlineData("10.0 2.0 /", typeof(BinaryExpression))]
    public void CreateExpressionTree_ShouldPushBinaryExpressions(string input, Type expectedType)
    {
        var expression = ExpressionTreeBuilder.CreateExpressionTree(input);

        Assert.IsType(expectedType, expression);
    }

    [Homework(Homeworks.HomeWork11)]
    public void CreateExpressionTree_ShouldThrowExceptionOnUnknownCharacter()
    {
        var input = "2 3 $";

        Assert.Throws<Exception>(() => ExpressionTreeBuilder.CreateExpressionTree(input));
    }
}

