using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLabs
{
    public class Matrix
    {
        public float[,] matrix;

        public int Row { get; protected set; }
        public int Column { get; protected set; }

        public Matrix(int row, int column)
        {
            Row = row;
            Column = column;
            matrix = new float[row, column];
        }

        public Matrix Multiple(Matrix value)
        {
            Matrix result = new Matrix(Row, value.Column);
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < value.Column; j++)
                    for (int k = 0; k < value.Row; k++)
                        result.matrix[i, j] += matrix[i, k] * value.matrix[k, j];
            return result;
        }

        public void Add(int x, int y)
        {
            for (int i = 0; i < Row; i++)
            {
                matrix[i, 0] += x;
                matrix[i, 1] += y;
            }
        }
    }
}
