using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MatrixCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            for (int i = 1; i <= 10; i++)
            {
                RowsAComboBox.Items.Add(i);
                ColsAComboBox.Items.Add(i);
                RowsBComboBox.Items.Add(i);
                ColsBComboBox.Items.Add(i);
            }
            RowsAComboBox.SelectedIndex = 0;
            ColsAComboBox.SelectedIndex = 0;
            RowsBComboBox.SelectedIndex = 0;
            ColsBComboBox.SelectedIndex = 0;
        }

        private void CreateMatrixA_Click(object sender, RoutedEventArgs e)
        {
            CreateMatrix(MatrixAInput, (int)RowsAComboBox.SelectedItem, (int)ColsAComboBox.SelectedItem);
        }

        private void CreateMatrixB_Click(object sender, RoutedEventArgs e)
        {
            CreateMatrix(MatrixBInput, (int)RowsBComboBox.SelectedItem, (int)ColsBComboBox.SelectedItem);
        }

        private void CreateMatrix(StackPanel matrixInput, int rows, int cols)
        {
            matrixInput.Children.Clear();
            for (int i = 0; i < rows; i++)
            {
                var rowPanel = new StackPanel { Orientation = Orientation.Horizontal };
                for (int j = 0; j < cols; j++)
                {
                    var textBox = new TextBox { Width = 40, Margin = new Thickness(5) };
                    rowPanel.Children.Add(textBox);
                }
                matrixInput.Children.Add(rowPanel);
            }
        }

        private double[,] ParseMatrix(StackPanel matrixInput)
        {
            var rows = matrixInput.Children.OfType<StackPanel>().Select(sp => sp.Children.OfType<TextBox>().Select(tb => tb.Text).ToArray()).ToArray();
            int rowCount = rows.Length;
            int colCount = rows[0].Length;

            var matrix = new double[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (!double.TryParse(rows[i][j], out matrix[i, j]))
                    {
                        throw new FormatException($"Ошибка ввода в строке {i + 1}, столбце {j + 1}: '{rows[i][j]}' не является числом.");
                    }
                }
            }
            return matrix;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            PerformMatrixOperation(MatrixOperations.Add);
        }

        private void BtnSubtract_Click(object sender, RoutedEventArgs e)
        {
            PerformMatrixOperation(MatrixOperations.Subtract);
        }

        private void BtnMultiply_Click(object sender, RoutedEventArgs e)
        {
            var a = ParseMatrix(MatrixAInput);
            var b = ParseMatrix(MatrixBInput);

            if (a.GetLength(1) != b.GetLength(0))
            {
                Result.Text = "Ошибка: Количество столбцов первой матрицы не совпадает с количеством строк второй.";
                return;
            }

            var result = MatrixOperations.Multiply(a, b);
            DisplayResult(result);
        }

        private void BtnInverse_Click(object sender, RoutedEventArgs e)
        {
            var a = ParseMatrix(MatrixAInput);
            if (a.GetLength(0) != a.GetLength(1))
            {
                Result.Text = "Ошибка: Обратная матрица может быть найдена только для квадратных матриц.";
                return;
            }

            try
            {
                var inverse = MatrixOperations.Inverse(a);
                DisplayResult(inverse);
            }
            catch (InvalidOperationException ex)
            {
                Result.Text = ex.Message;
            }
        }

        private void BtnTranspose_Click(object sender, RoutedEventArgs e)
        {
            var a = ParseMatrix(MatrixAInput);
            var b = ParseMatrix(MatrixBInput);

            var transposedA = MatrixOperations.Transpose(a);
            var transposedB = MatrixOperations.Transpose(b);

            Result.Text += "Транспонированная матрица A:" + Environment.NewLine;
            DisplayResult(transposedA);
            Result.Text += Environment.NewLine + "Транспонированная матрица B:" + Environment.NewLine;
            DisplayResult(transposedB);
        }

        private void PerformMatrixOperation(Func<double[,], double[,], double[,]> operation)
        {
            try
            {
                var a = ParseMatrix(MatrixAInput);
                var b = ParseMatrix(MatrixBInput);

                if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                {
                    Result.Text = "Ошибка: Размеры матриц не совпадают.";
                    return;
                }

                var result = operation(a, b);
                DisplayResult(result);
            }
            catch (FormatException ex)
            {
                Result.Text = ex.Message;
            }
        }

        private void DisplayResult(double[,] matrix)
        {
            var resultBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    resultBuilder.Append(matrix[i, j].ToString("F2")); // Форматирование до двух знаков после запятой
                    if (j < matrix.GetLength(1) - 1)
                    {
                        resultBuilder.Append("\t"); // Добавление табуляции между элементами строки
                    }
                }
                resultBuilder.AppendLine(); // Переход на новую строку после каждой строки матрицы
            }
            Result.Text += resultBuilder.ToString() + Environment.NewLine; // Добавление результата в текстовое поле
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Result.Clear();
        }
    }
}