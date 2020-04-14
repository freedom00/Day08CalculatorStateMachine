using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Day08CalculatorStateMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum CalcState { Inputing1stValue, Inputing2ndValue, DispalyingResult }

        private CalcState calcState;
        private double firstValue;
        private double secondValue;
        private double result;

        public MainWindow()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            tb1stValue.Text = "";
            lblOperation.Content = "?";
            tb2ndValue.Text = "";
            tbResult.Text = "";

            tb1stValue.IsEnabled = true;
            tb2ndValue.IsEnabled = false;

            Keyboard.Focus(tb1stValue);

            setNumButtonEnabled(true);
            setOperButtonEnabled(true);
            btResult.IsEnabled = false;

            calcState = CalcState.Inputing1stValue;

            firstValue = secondValue = result = 0;
        }

        private void setNumButtonEnabled(bool isEnabled)
        {
            bt0.IsEnabled = isEnabled;
            bt1.IsEnabled = isEnabled;
            bt2.IsEnabled = isEnabled;
            bt3.IsEnabled = isEnabled;
            bt4.IsEnabled = isEnabled;
            bt5.IsEnabled = isEnabled;
            bt6.IsEnabled = isEnabled;
            bt7.IsEnabled = isEnabled;
            bt8.IsEnabled = isEnabled;
            bt9.IsEnabled = isEnabled;
            btDot.IsEnabled = isEnabled;
            btSign.IsEnabled = isEnabled;
        }

        private void setOperButtonEnabled(bool isEnabled)
        {
            btAdd.IsEnabled = isEnabled;
            btSubtract.IsEnabled = isEnabled;
            btMultiply.IsEnabled = isEnabled;
            btDivide.IsEnabled = isEnabled;
        }

        private void btNumber_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = null;
            switch (calcState)
            {
                case CalcState.Inputing1stValue:
                    textBox = tb1stValue;
                    break;

                case CalcState.Inputing2ndValue:
                    textBox = tb2ndValue;

                    break;

                case CalcState.DispalyingResult:
                    textBox = tbResult;
                    break;
            }
            Button button = (Button)sender;
            string text = button.Content.ToString();
            switch (text)
            {
                case ".":
                    textBox.Text += text;
                    btDot.IsEnabled = false;
                    break;

                case "+/-":
                    textBox.Text = textBox.Text.Contains("-") ? textBox.Text.Substring(1, textBox.Text.Length - 1) : "-" + textBox.Text;
                    break;

                default:
                    textBox.Text += text;
                    break;
            }
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btOperation_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tb1stValue.Text, out firstValue))
            {
                MessageBox.Show(this, "The first value must be a number", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tb1stValue.Text = "";
                return;
            }

            lblOperation.Content = ((Button)sender).Content.ToString();

            tb1stValue.IsEnabled = false;
            tb2ndValue.IsEnabled = true;

            Keyboard.Focus(tb2ndValue);

            setOperButtonEnabled(false);
            btResult.IsEnabled = true;

            calcState = CalcState.Inputing2ndValue;
        }

        private void btResult_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tb2ndValue.Text, out secondValue))
            {
                MessageBox.Show(this, "The second value must be a number", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tb2ndValue.Text = "";
                return;
            }
            if (0 == secondValue && "/" == lblOperation.Content.ToString())
            {
                MessageBox.Show(this, "Can't divide by zero", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tb2ndValue.Text = "";
                return;
            }

            switch (lblOperation.Content)
            {
                case "+":
                    result = firstValue + secondValue;
                    break;

                case "-":
                    result = firstValue - secondValue;
                    break;

                case "*":
                    result = firstValue * secondValue;
                    break;

                case "/":
                    result = firstValue / secondValue;
                    break;
            }
            tbResult.Text = string.Format("{0:G}", result);

            tb2ndValue.IsEnabled = false;

            Keyboard.Focus(tbResult);

            setNumButtonEnabled(false);
            btResult.IsEnabled = false;

            calcState = CalcState.DispalyingResult;
        }
    }
}