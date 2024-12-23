using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;


namespace FishStorageMonitor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FishTypeTextBox.Clear();
            MaxTempTextBox.Clear();
            MaxTimeExceedTextBox.Clear();
            MinTempTextBox.Clear();
            MinTimeViolationTextBox.Clear();
            StartDateTextBox.Clear();
            TemperaturesTextBox.Clear();
            ResultTextBlock.Text = "";
            MessageBox.Show("Всё очищено");
        }

        private void CheckConditions_Click(object sender, RoutedEventArgs e)
        {
            string fishType = FishTypeTextBox.Text;
            double maxTemp = double.Parse(MaxTempTextBox.Text);
            double minTemp = double.Parse(MinTempTextBox.Text);
            string[] tempStrings = TemperaturesTextBox.Text.Split(',');
            double[] temperatures = tempStrings.Select(double.Parse).ToArray();

            bool isValid = true;
            string report = $"Отчет по {fishType}:\n";

            for (int i = 0; i < temperatures.Length; i++)
            {
                double temp = temperatures[i];

                if (temp > maxTemp)
                {
                    isValid = false;
                    report += $"Нарушение: Превышение максимальной температуры на замере {i + 1}. Значение: {temp}\n";
                }
                if (temp < minTemp)
                {
                    isValid = false;
                    report += $"Нарушение: Невыполнение минимальной температуры на замере {i + 1}. Значение: {temp}\n";
                }
            }

            ResultTextBlock.Text = isValid ? "Условия хранения соблюдены." : "Условия хранения нарушены!";
            report += isValid ? "Все условия соблюдены." : "Есть нарушения.";

            File.WriteAllText("Report.txt", report);
            MessageBox.Show("Отчет сохранен в файл: Report.txt");
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите файл с данными"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var lines = File.ReadAllLines(openFileDialog.FileName);
                if (lines.Length >= 2)
                {
                    StartDateTextBox.Text = lines[0];
                    TemperaturesTextBox.Text = lines[1];
                    MaxTempTextBox.Text = "5"; 
                    MinTempTextBox.Text = "-3"; 
                    MaxTimeExceedTextBox.Text = "20"; 
                    MinTimeViolationTextBox.Text = "60"; 
                }
                else
                {
                    MessageBox.Show("Файл должен содержать хотя бы две строки.", "Ошибка");
                }
            }
        }
    }
}
