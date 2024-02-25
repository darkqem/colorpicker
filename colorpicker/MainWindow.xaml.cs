using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace colorpicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            UpdateColor();
        }

        private void ApplyColorButton_Click(object sender, RoutedEventArgs e)
        {

            Color color = Color.FromArgb(0, 0, 0, 0); // Инициализация переменной color вне блоков if
            byte r = 0, g = 0, b = 0, a = 255; // Инициализация переменных r, g, b, a вне блоков if, a по умолчанию   255 (полностью непрозрачный)

            // Попытка разобрать ввод как HEX цвет
            if (colorInputTextBox.Text.StartsWith("#"))
            {
                if (ColorConverter.ConvertFromString(colorInputTextBox.Text) is Color parsedColor)
                {
                    color = parsedColor; // Присваивание значения переменной color
                }
            }
            else if (colorInputTextBox.Text.Length == 8) // Проверка на формат   8D000000
            {
                try
                {
                    a = Convert.ToByte(colorInputTextBox.Text.Substring(0, 2), 16);
                    r = Convert.ToByte(colorInputTextBox.Text.Substring(2, 2), 16);
                    g = Convert.ToByte(colorInputTextBox.Text.Substring(4, 2), 16);
                    b = Convert.ToByte(colorInputTextBox.Text.Substring(6, 2), 16);

                    color = Color.FromArgb(a, r, g, b);
                }
                catch (FormatException)
                {
                    // Если формат неверный, ничего не происходит
                }
            }
            else if (colorInputTextBox.Text.Length == 6) // Проверка на формат RRGGBB без #
            {
                try
                {
                    r = Convert.ToByte(colorInputTextBox.Text.Substring(0, 2), 16);
                    g = Convert.ToByte(colorInputTextBox.Text.Substring(2, 2), 16);
                    b = Convert.ToByte(colorInputTextBox.Text.Substring(4, 2), 16);

                    color = Color.FromArgb(a, r, g, b);
                }
                catch (FormatException)
                {
                    // Если формат неверный, ничего не происходит
                }
            }

            if (color != Color.FromArgb(0, 0, 0, 0)) // Проверка, что color был успешно установлен
            {
                ColorRectangle.Fill = new SolidColorBrush(color);
                UpdateSliders(color);
            }

        }

        private void ColorInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyColorButton_Click(sender, null);
            }
        }













        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateColor();
        }

        private void ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateColorFromTextBoxes();
        }

        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateColorFromHex();
        }

        private void UpdateColor()
        {
            byte r = (byte)RedSlider.Value;
            byte g = (byte)GreenSlider.Value;
            byte b = (byte)BlueSlider.Value;
            byte a = (byte)AlphaSlider.Value;

            Color color = Color.FromArgb(a, r, g, b);
            ColorRectangle.Fill = new SolidColorBrush(color);
            UpdateHexTextBox(color);
            UpdateTextBoxes(color);
        }

        private void UpdateColorFromHex()
        {
            string hex = colorInputTextBox.Text.Trim();
            if (hex.Length == 8) // Ensure it's a valid RGBA hex value
            {
                try
                {
                    Color color = (Color)ColorConverter.ConvertFromString(hex);
                    UpdateSliders(color);
                    ColorRectangle.Fill = new SolidColorBrush(color);
                    UpdateTextBoxes(color);
                }
                catch (FormatException)
                {
                    // Invalid hex format, ignore or notify the user
                }
            }
        }

        private void UpdateColorFromTextBoxes()
        {
            if (byte.TryParse(RedTextBox.Text, out byte r) &&
                byte.TryParse(GreenTextBox.Text, out byte g) &&
                byte.TryParse(BlueTextBox.Text, out byte b) &&
                byte.TryParse(AlphaTextBox.Text, out byte a))
            {
                Color color = Color.FromArgb(a, r, g, b);
                UpdateSliders(color);
                ColorRectangle.Fill = new SolidColorBrush(color);
                UpdateHexTextBox(color);
            }
        }

        private void UpdateHexTextBox(Color color)
        {
            colorInputTextBox.Text = $"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        private void UpdateTextBoxes(Color color)
        {
            RedTextBox.Text = color.R.ToString();
            GreenTextBox.Text = color.G.ToString();
            BlueTextBox.Text = color.B.ToString();
            AlphaTextBox.Text = color.A.ToString();
        }

        private void UpdateSliders(Color color)
        {
            RedSlider.Value = color.R;
            GreenSlider.Value = color.G;
            BlueSlider.Value = color.B;
            AlphaSlider.Value = color.A;
        }
    }
}