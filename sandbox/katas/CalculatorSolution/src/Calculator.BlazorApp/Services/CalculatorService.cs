namespace Calculator.BlazorApp.Services
{
    public class CalculatorService
    {
        private CalculatorState _state = CalculatorState.Initial;
        private string _displayText = "0";
        private double _storedValue = 0;
        private string? _pendingOperation = null;
        private string? _errorMessage = null;

        public string Display => _errorMessage ?? _displayText;
        public bool HasError => _errorMessage != null;

        public void AppendNumber(string input)
        {
            if (_state == CalculatorState.Error)
            {
                _errorMessage = null;
                _displayText = "0";
                _storedValue = 0;
                _state = CalculatorState.Initial;
            }

            if (_state == CalculatorState.ResultDisplayed)
            {
                _displayText = "0";
                _state = CalculatorState.Initial;
            }

            if (_state == CalculatorState.OperationSet)
            {
                _displayText = "0";
                _state = CalculatorState.EnteringNumber;
            }

            if (input == ".")
            {
                if (_displayText.Contains("."))
                    return;

                if (_state == CalculatorState.Initial)
                    _displayText = "0";

                _displayText += ".";
                _state = CalculatorState.EnteringNumber;
                return;
            }

            if (_displayText == "0" || _state == CalculatorState.Initial)
                _displayText = input;
            else
                _displayText += input;

            _state = CalculatorState.EnteringNumber;
        }

        public void SetOperation(string operation)
        {
            if (_state == CalculatorState.Error)
            {
                _errorMessage = null;
                _displayText = "0";
                _storedValue = 0;
                _state = CalculatorState.Initial;
            }


            double currentNumber = ParseDisplayValue();

            if (_pendingOperation != null && _state == CalculatorState.EnteringNumber)
            {
                _storedValue = PerformOperation(_storedValue, currentNumber, _pendingOperation);

                if (HasError)
                {
                    _state = CalculatorState.Error;
                    return;
                }

                _displayText = _storedValue.ToString("G15");
            }
            else
            {
                _storedValue = currentNumber;
            }

            _pendingOperation = operation;
            _state = CalculatorState.OperationSet;
        }

        public void Calculate()
        {
            if (_state == CalculatorState.Error || _pendingOperation == null)
                return;

            double currentNumber = ParseDisplayValue();
            double result = PerformOperation(_storedValue, currentNumber, _pendingOperation);

            if (HasError)
            {
                _state = CalculatorState.Error;
                return;
            }

            _displayText = result.ToString("G15");
            _storedValue = result;
            _pendingOperation = null;
            _state = CalculatorState.ResultDisplayed;
        }

        public void Clear()
        {
            _state = CalculatorState.Initial;
            _displayText = "0";
            _storedValue = 0;
            _pendingOperation = null;
            _errorMessage = null;
        }

        public void ClearEntry()
        {
            if (_state == CalculatorState.Error)
            {
                Clear();
            }
            else
            {
                _displayText = "0";
                _state = CalculatorState.Initial;
            }
        }

        private double ParseDisplayValue()
        {
            if (double.TryParse(_displayText, out double result))
                return result;
            return 0;
        }

        private double PerformOperation(double left, double right, string operation)
        {
            try
            {
                return operation switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    "*" => left * right,
                    "/" => right == 0
                        ? throw new DivideByZeroException()
                        : left / right,
                    _ => left
                };
            }
            catch (DivideByZeroException)
            {
                _errorMessage = "Nelze dělit nulou";
                return 0;
            }
            catch (OverflowException)
            {
                _errorMessage = "Číslo je příliš velké";
                return 0;
            }
        }
    }
}
