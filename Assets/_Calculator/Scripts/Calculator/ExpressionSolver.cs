using System;
using System.Globalization;

public static class ExpressionSolver
{
    private static string _expression;
    private static int _currentIndex;

    public static double Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new FormatException("Expression is empty.");

        // Reset everything before solving new expression
        _expression = expression;
        _currentIndex = 0;

        // Start solving from + and - level
        double answer = SolveAddAndSubtract();

        SkipSpaces();

        // If something is still left, then it is not a valid expression
        if (_currentIndex < _expression.Length)
            throw new FormatException("Unexpected character '" + _expression[_currentIndex] + "' in expression.");

        return answer;
    }

    private static double SolveAddAndSubtract()
    {
        // First solve * and / because they have more priority
        double answer = SolveMultiplyAndDivide();

        while (true)
        {
            SkipSpaces();

            if (_currentIndex >= _expression.Length)
                return answer;

            char symbol = _expression[_currentIndex];

            if (symbol != '+' && symbol != '-')
                return answer;

            // Move after the operator
            _currentIndex++;

            // Get next full * or / part before adding or subtracting
            double nextNumber = SolveMultiplyAndDivide();

            if (symbol == '+')
                answer += nextNumber;
            else
                answer -= nextNumber;
        }
    }

    private static double SolveMultiplyAndDivide()
    {
        // Start with first number
        double answer = ReadNumber();

        while (true)
        {
            SkipSpaces();

            // We have reached the end of expression
            if (_currentIndex >= _expression.Length)
                return answer;

            char symbol = _expression[_currentIndex];

            // If the symbol is not * or /, then we are done with this part
            if (symbol != '*' && symbol != '/')
                return answer;

            // Move after the operator
            _currentIndex++;

            double nextNumber = ReadNumber();

            if (symbol == '*')
            {
                answer *= nextNumber;
            }
            else
            {
                if (nextNumber == 0)
                    throw new DivideByZeroException("Cannot divide by zero.");

                answer /= nextNumber;
            }
        }
    }

    private static double ReadNumber()
    {
        SkipSpaces();

        if (_currentIndex >= _expression.Length)
            throw new FormatException("Expression is not complete.");

        int startIndex = _currentIndex;
        bool isNegative = false;
        bool hasDigit = false;
        bool hasDot = false;

        // This is to handle negative numbers
        if (_expression[_currentIndex] == '-')
        {
            isNegative = true;
            _currentIndex++;
            SkipSpaces();
            startIndex = _currentIndex;
        }

        // Keep reading until we reach an operator
        while (_currentIndex < _expression.Length)
        {
            char letter = _expression[_currentIndex];

            if (char.IsDigit(letter))
            {
                hasDigit = true;
                _currentIndex++;
                continue;
            }

            if (letter == '.')
            {
                // A number can have only one decimal point
                if (hasDot)
                    throw new FormatException("Too many dots in one number.");

                hasDot = true;
                _currentIndex++;
                continue;
            }

            break;
        }

        if (!hasDigit)
            throw new FormatException("Expression is not complete.");

        // Take the number text from expression
        string numberText = _expression.Substring(startIndex, _currentIndex - startIndex);

        if (isNegative)
            numberText = "-" + numberText;

        // Convert the text number into actual double value
        double number;
        bool isNumber = double.TryParse(numberText, NumberStyles.Any, CultureInfo.InvariantCulture, out number);

        if (!isNumber)
            throw new FormatException("'" + numberText + "' is not a valid number.");

        return number;
    }

    private static void SkipSpaces()
    {
        // Move index forward while there are spaces
        while (_currentIndex < _expression.Length && _expression[_currentIndex] == ' ')
        {
            _currentIndex++;
        }
    }
}
