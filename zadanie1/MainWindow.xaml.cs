using Microsoft.Win32;
using System;
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

            else if (RemoveWordRadio.IsChecked == true)
            {
                OutputTextBox.Text = RemoveWord(inputText);
            }

            else if (MoveRadio.IsChecked == true) {
                OutputTextBox.Text = ShiftCharacters(inputText);
            }
        }
        private string ModifyTextToUppercase(string text)
        {
            return text.ToUpper();
        }

        private string ModifyTextToTitleCase(string text)
        {
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

        private string ShiftCharacters(string text)
        {
            var window2 = new DialogBox2()
            {
                Title = "Dialog Box",
                Topmost = true,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Owner = this
            };
            window2.ShowDialog();
            int shiftAmount;
            var zero = 0;
            bool success = int.TryParse(window2.NumberInput.Text, out zero);
            if (success)
            {
                shiftAmount = int.Parse(window2.NumberInput.Text);
            }
            else
            {
                return text;
            }

            char[] inputCharacters = text.ToCharArray();

            for (int i = 0; i < inputCharacters.Length; i++)
            {
                if (char.IsLetter(inputCharacters[i]))
                {
                    char baseChar = char.IsUpper(inputCharacters[i]) ? 'A' : 'a';
                    inputCharacters[i] = (char)(baseChar + (inputCharacters[i] - baseChar + shiftAmount) % 26);
                }
            }
            
            return text = new string(inputCharacters);
        }

        private string RemoveWord(string text) {
            var window2 = new DialogBox()
            {
                Title = "Dialog Box",
                Topmost = true,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Owner = this
            };
            window2.ShowDialog();
            if (window2.RemoveWordInput.Text.Length != 0) { 
                var word = window2.RemoveWordInput.Text;
                text = text.Replace(word, "");
                return text;
            }
            else
            {
                return text;
            }

        }



        private void SearchWord_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            string inputText = InputTextBox.Text;
            int index = inputText.IndexOf(searchText);
            int count = CountTextOccurrences(inputText, searchText);

            if (index != -1)
            {
                FirstOccurence.Text = "First occurence of '" + searchText + "' on position " + index;
                NumberOfOccurences.Text = "Number of occurences '" + searchText + "': " + count;
            }
            else
            {
                FirstOccurence.Text = "Nie znaleziono tekstu.";
            }


            NumberOfOccurences.Text = "Liczba wystąpień wyrazu '" + searchText + "': " + count;


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

        private void OutputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}

