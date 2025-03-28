using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                string operationText = first.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                operation.ToString();
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

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            CalculationTextBox.Clear();
            OperationHistoryTextBlock.Text = string.Empty;
            first = 0;
            second = 0;
            operation = '\0';
        }

        private void ClearElementButton_Click(object sender, RoutedEventArgs e)
        {
            if (CalculationTextBox.Text.Length > 0)
            {
                CalculationTextBox.Text = CalculationTextBox.Text.Remove(CalculationTextBox.Text.Length - 1);
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            commandInvoker.UndoCommand();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            commandInvoker.RedoCommand();
           
        }

        private void SandwichButton_Click(object sender, RoutedEventArgs e)
        {
            if (SandwichButton.Visibility == Visibility.Visible)
            {
                SandwichButton.Visibility = Visibility.Hidden;
                BackButton.Visibility = Visibility.Visible;
                AdditionalColumn.Width = new GridLength(1, GridUnitType.Star);
                PIButton.Visibility = Visibility.Visible;
                SqrtButton.Visibility = Visibility.Visible;
                PowButton.Visibility = Visibility.Visible;
                LogarithmButton.Visibility = Visibility.Visible;
                ConstantEButton.Visibility = Visibility.Visible;
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (BackButton.Visibility == Visibility.Visible)
            {
                SandwichButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Hidden;
                AdditionalColumn.Width = new GridLength(0);
                PIButton.Visibility = Visibility.Collapsed;
                SqrtButton.Visibility = Visibility.Collapsed;
                PowButton.Visibility = Visibility.Collapsed;
                LogarithmButton.Visibility = Visibility.Collapsed;
                ConstantEButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
