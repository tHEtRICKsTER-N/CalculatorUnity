using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorController : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TMP_Text _expressionText;
    [SerializeField] private TMP_Text _resultText;

    [Header("Number Buttons (0 - 9)")]
    [SerializeField] private Button[] _digitButtons;

    [Header("Operator Buttons")]
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _subtractButton;
    [SerializeField] private Button _multiplyButton;
    [SerializeField] private Button _divideButton;
    [SerializeField] private Button _moduloButton;
    [SerializeField] private Button _decimalButton;

    [Header("Action Buttons")]
    [SerializeField] private Button _equalsButton;
    [SerializeField] private Button _clearEntryButton;
    [SerializeField] private Button _allClearButton;

    private string _expression = "";
    private bool _justSolved = false;

    private void Start()
    {
        // Zero out the texts at start
        _resultText.text = "0";
        _expressionText.text = "0";

        AddListenersToButtons();
    }

    private void AddListenersToButtons()
    {
        for (int i = 0; i < _digitButtons.Length; i++)
        {
            int digit = i;
            _digitButtons[i].onClick.AddListener(() =>
            {
                AddDigit(digit.ToString());
            });
        }

        _addButton.onClick.AddListener(() => AddOperator('+'));
        _subtractButton.onClick.AddListener(() => AddOperator('-'));
        _multiplyButton.onClick.AddListener(() => AddOperator('*'));
        _divideButton.onClick.AddListener(() => AddOperator('/'));
        _moduloButton.onClick.AddListener(() => AddOperator('%'));

        _decimalButton.onClick.AddListener(AddDecimal);

        _equalsButton.onClick.AddListener(ShowAnswer);
        _clearEntryButton.onClick.AddListener(ClearEntry);
        _allClearButton.onClick.AddListener(AllClear);
    }

    private void AddDigit(string digit)
    {
        // Clear the expression if we just showed the answer to start new calculation
        if (_justSolved)
        {
            _expression = "";
            _justSolved = false;
            _resultText.text = "";
        }

        _expression += digit;
        UpdateExpressionText();
    }

    private void AddOperator(char symbol)
    {
        // This is to handle negative numbers
        if (string.IsNullOrEmpty(_expression))
        {
            if (symbol == '-')
            {
                _expression = "-";
                UpdateExpressionText();
            }

            return;
        }

        // To continue editing the expression after showing the answer
        _justSolved = false;

        // This is to handle if the user by mistake puts an operator at end
        if (LastLetterIsOperator())
            RemoveLastLetter();

        _expression += symbol;

        UpdateExpressionText();
    }

    private void AddDecimal()
    {
        // To start a new expression with decimal number
        if (_justSolved)
        {
            _expression = "0.";
            _justSolved = false;
            UpdateExpressionText();
            return;
        }

        // To handle empty text or operate any number with decimal number
        if (string.IsNullOrEmpty(_expression) || LastLetterIsOperator())
        {
            _expression += "0.";
            UpdateExpressionText();
            return;
        }

        if (CurrentNumberAlreadyHasDot())
            return;

        _expression += '.';
        UpdateExpressionText();
    }

    private void ShowAnswer()
    {
        if (string.IsNullOrEmpty(_expression))
            return;

        string expressionToSolve = _expression;

        // To handle if the user by mistake puts an operator at end 
        if (LastLetterIsOperator())
            expressionToSolve = _expression.Substring(0, _expression.Length - 1);

        try
        {
            double answer = ExpressionSolver.Evaluate(expressionToSolve);
            _resultText.text = MakePrettyNumber(answer);
            _justSolved = true;
        }
        catch (DivideByZeroException)
        {
            _resultText.text = "Divide by 0 Error";
        }
        catch (Exception)
        {
            _resultText.text = "Error";
        }
    }

    private void ClearEntry()
    {
        // After showing the answer, simply clear all
        if (_justSolved)
        {
            AllClear();
            return;
        }

        if (_expression.Length > 0)
            RemoveLastLetter();

        UpdateExpressionText();
    }

    private void AllClear()
    {
        _expression = "";
        _justSolved = false;
        _resultText.text = "";

        UpdateExpressionText();
    }

    private void UpdateExpressionText() => _expressionText.text = _expression;

    private void RemoveLastLetter() => _expression = _expression.Substring(0, _expression.Length - 1);

    private bool LastLetterIsOperator()
    {
        if (string.IsNullOrEmpty(_expression))
            return false;

        char lastLetter = _expression[_expression.Length - 1];
        return IsOperator(lastLetter);
    }

    private bool CurrentNumberAlreadyHasDot()
    {
        for (int i = _expression.Length - 1; i >= 0; i--)
        {
            char letter = _expression[i];

            if (letter == '.')
                return true;

            // if we reach an operator, that means the current last number does not have a dot
            if (letter == '+' || letter == '*' || letter == '/' || letter == '%')
                return false;

            // If we reach minus and it's not the first letter that means the current last number does not have a dot
            if (letter == '-' && i > 0)
                return false;
        }

        return false;
    }

    private string MakePrettyNumber(double value)
    {
        // If result is infinite or undefined
        if (double.IsInfinity(value) || double.IsNaN(value))
            return "Error";

        // G10 is to remove trailing zeroes
        return value % 1 == 0 ? ((long)value).ToString() : value.ToString("G10");
    }

    private bool IsOperator(char letter) => letter == '+' || letter == '-' || letter == '*' || letter == '/' || letter == '%';
}
