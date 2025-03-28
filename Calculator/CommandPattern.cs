using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }
    public class CalculatorCommand : ICommand 
    {
        private MainWindow calculator;
        private double result;
        private double previousResult;
        private string operationText;
        private string previousOperationText;
        public CalculatorCommand(MainWindow calculator, double result, double previousResult, string operationText, string previousOperationText)
        {
            this.calculator = calculator;
            this.result = result;
            this.previousResult = previousResult;
            this.operationText = operationText;
            this.previousOperationText = previousOperationText;
        }
        public void Execute()
        {
            calculator.CalculationTextBox.Text = result.ToString(System.Globalization.CultureInfo.InvariantCulture);
            calculator.OperationTextBlock.Text = operationText;
        }
        public void Undo()
        {
            calculator.CalculationTextBox.Text = previousResult.ToString(System.Globalization.CultureInfo.InvariantCulture);
            calculator.OperationTextBlock.Text = previousOperationText;
        }
        public void Redo()
        {
            Execute();
        }
    }
   
    class CommandPattern
    {
    }
}
