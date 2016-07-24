using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MathematicalEntities
{
    public class SquareMatrix<T> : Matrix<T>
    {
        public SquareMatrix(int dimension) : base(new T[dimension, dimension]) { }

        public SquareMatrix(T[,] array) : base(array)
        {
            if (array.GetLength(0) != array.GetLength(1))
            {
                throw new ArgumentException("array is not a square matrix");
            }
        }
    }
}
