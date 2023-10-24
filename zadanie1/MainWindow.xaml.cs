using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Windows;
namespace zadanie1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                try
                {
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        InputTextBox.Text = reader.ReadToEnd();
                    }
                }
                catch (IOException)
                {
                    System.Windows.MessageBox.Show("Błąd odczytu pliku.");
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                try
                {
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        writer.Write(OutputTextBox.Text);
                    }
                }
                catch (IOException)
                {
                    System.Windows.MessageBox.Show("Błąd zapisu do pliku.");
                }
            }
        }

        private void ModifyText_Click(object sender, RoutedEventArgs e)
        {
            string inputText = InputTextBox.Text;

            if (UppercaseRadio.IsChecked == true)
            {
                OutputTextBox.Text = ModifyTextToUppercase(inputText);
            }
            else if (TitleCaseRadio.IsChecked == true)
            {
                OutputTextBox.Text = ModifyTextToTitleCase(inputText);
            }
            else if (ReplacePolishRadio.IsChecked == true)
            {
                OutputTextBox.Text = ReplacePolishCharacters(inputText);
            }
        }
        private string ModifyTextToUppercase(string text)
        {
            return text.ToUpper();
        }

        private string ModifyTextToTitleCase(string text)
        {
            // Możesz użyć metody TextInfo.ToTitleCase do zamiany na wielkie początkowe litery.
            TextInfo textInfo = new CultureInfo("pl-PL", false).TextInfo;
            return textInfo.ToTitleCase(text.ToLower());
        }

        private string ReplacePolishCharacters(string text)
        {
            string[] polishChars = { "ą", "ć", "ę", "ł", "ń", "ó", "ś", "ź", "ż" };
            string[] replacementChars = { "a", "c", "e", "l", "n", "o", "s", "z", "z" };

            for (int i = 0; i < polishChars.Length; i++)
            {
                text = text.Replace(polishChars[i], replacementChars[i]);
                text = text.Replace(polishChars[i].ToUpper(), replacementChars[i].ToUpper());
            }
            return text;
        }
    


        private void SearchWord_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            string inputText = InputTextBox.Text;
            int index = inputText.IndexOf(searchText);

            if (index != -1)
            {
                SearchTextBox.Text = "Pierwsze wystąpienie wyrazu '" + searchText + "' na pozycji " + index;
            }
            else
            {
                SearchTextBox.Text = "Nie znaleziono tekstu.";
            }
        }

        private void CountOccurrences_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            string inputText = InputTextBox.Text;

            int count = CountTextOccurrences(inputText, searchText);
            OutputTextBox.Text = "Liczba wystąpień wyrazu '" + searchText + "': " + count;
        }

        private void TrimText_Click(object sender, RoutedEventArgs e)
        {
            string maxLengthText = SearchTextBox.Text;
            if (int.TryParse(maxLengthText, out int maxLength))
            {
                string inputText = InputTextBox.Text;
                string trimmedText = TrimTextToMaxLength(inputText, maxLength);
                OutputTextBox.Text = trimmedText;
            }
            else
            {
                OutputTextBox.Text = "Podaj prawidłową długość do przycięcia.";
            }
        }

        private int CountTextOccurrences(string input, string searchText)
        {
            int count = 0;
            int index = input.IndexOf(searchText);
            while (index != -1)
            {
                count++;
                index = input.IndexOf(searchText, index + 1);
            }
            return count;
        }

        private string TrimTextToMaxLength(string input, int maxLength)
        {
            if (input.Length <= maxLength)
            {
                return input;
            }
            else
            {
                return input.Substring(0, maxLength);
            }
        }
        private void PerformAction_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

