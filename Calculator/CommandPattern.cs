﻿using System;
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
            calculator.OperationHistoryTextBlock.Text = operationText;
        }
        public void Undo()
        {
            calculator.CalculationTextBox.Text = previousResult.ToString(System.Globalization.CultureInfo.InvariantCulture);
            calculator.OperationHistoryTextBlock.Text = previousOperationText;
        }
    }
    public class CommandInvoker
    {
        private Stack<ICommand> undoStack= new Stack<ICommand>();
        private Stack<ICommand> redoStack = new Stack<ICommand>();
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void UndoCommand()
        {
            if (undoStack.Count > 0)
            {
                ICommand command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }
        public void RedoCommand()
        {
            if (redoStack.Count > 0)
            {
                ICommand command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }
    }
    class CommandPattern
    {
    }
}
