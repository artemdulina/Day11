using System;

namespace MathematicalEntities.Matricies
{
    public sealed class SquareMatrix<T> : Matrix<T>
    {
        public SquareMatrix(int dimension) : base(new T[dimension, dimension]) { }

        public SquareMatrix(T[,] array) : base(array)
        {
            if (array.GetLength(0) != array.GetLength(1))
            {
                throw new ArgumentException("Array is not a square matrix");
            }
        }
    }
}
