namespace Parbleu.DynamicJson;

public class Evaluator
{
    public object Evaluate(string expression)
    {
        if (expression.StartsWith("="))
        {
            return expression.Substring(1);
        }

        throw new InvalidOperationException();
    }


    public bool IsExpression(string expression)
    {
        return expression.StartsWith("=");
    }
}