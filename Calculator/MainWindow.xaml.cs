﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OperationHistoryTextBlock.Text = "0";
            CalculationTextBox.Text = "0";
        }
        private double first;
        private double second;
        private char operation;
        private CommandInvoker commandInvoker = new CommandInvoker();
        private bool lastActionWasOperation = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (lastActionWasOperation)
            {
                CalculationTextBox.Clear();
                lastActionWasOperation = false;
            }
            OperationHistoryTextBlock.Text = CalculationTextBox.Text + " " + btn.Content.ToString();
            CalculationTextBox.Text += btn.Content.ToString();
        }
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (!string.IsNullOrEmpty(CalculationTextBox.Text))
            {
                if (double.TryParse(CalculationTextBox.Text, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out first))
                {
                    operation = btn.Content.ToString()[0];
                    OperationHistoryTextBlock.Text = first.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + operation.ToString();
                    lastActionWasOperation = true;
                    CalculationTextBox.Clear();
                }
                else
                {
                    CalculationTextBox.Text = "Invalid number format";
                    return;
                }
            }
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CalculationTextBox.Text)) return;
            if (double.TryParse(CalculationTextBox.Text, System.Globalization.NumberStyles.Any,
               System.Globalization.CultureInfo.InvariantCulture, out second))
            {
                double result = 0;
                string operationText = first.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                " " + operation.ToString() + " " + second.ToString(System.Globalization.CultureInfo.InvariantCulture) + " =";
                OperationHistoryTextBlock.Text = operationText;
                switch (operation)
                {
                    case '+': result = first + second; break;
                    case '-': result = first - second; break;
                    case '*': result = first * second; break;
                    case '/':
                        if (second != 0)
                        {
                            result = first / second;
                        }
                        else
                        {
                            CalculationTextBox.Text = "Divided by zero";
                            return;
                        }
                        break;
                    case '^': result = Math.Pow(first, second); break;
                    default: return;
                }
                string previousOperationText = OperationHistoryTextBlock.Text;
                CalculatorCommand command = new CalculatorCommand(this, result, first, operationText, previousOperationText);
                commandInvoker.ExecuteCommand(command);
                CalculationTextBox.Text = result.ToString(System.Globalization.CultureInfo.InvariantCulture);
                lastActionWasOperation = false;
            }
            else
            {
                CalculationTextBox.Text = "Invalid number format";
            }
        }
    }
}
