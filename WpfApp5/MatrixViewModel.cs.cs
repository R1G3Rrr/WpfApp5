using System.Collections.Generic;
using System.Linq;

namespace MatrixCalculator
{
    public class MatrixViewModel
    {
        public double[,] MatrixA { get; set; }
        public double[,] MatrixB { get; set; }

        public string DisplayResult(double[,] matrix)
        {
            return string.Join("\n", Enumerable.Range(0, matrix.GetLength(0)).Select(i =>
                string.Join(" ", Enumerable.Range(0, matrix.GetLength(1)).Select(j => matrix[i, j]))));
        }

        public double[,] ParseMatrix(IEnumerable<IEnumerable<string>> input)
        {
            int rowCount = input.Count();
            int colCount = input.First().Count();
            var matrix = new double[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                var row = input.ElementAt(i).Select(double.Parse).ToArray();
                for (int j = 0; j < colCount; j++)
                {
                    matrix[i, j] = row[j];
                }
            }

            return matrix;
        }
    }
}