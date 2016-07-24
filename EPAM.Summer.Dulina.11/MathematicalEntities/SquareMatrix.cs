using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities
{
    public sealed class SquareMatrix<T> : IMatrix<T>
    {
        private readonly T[,] matrix;
        private int dimension;
        private const int DefaultDemension = 5;

        private readonly ParameterExpression paramX = Expression.Parameter(typeof(double), "x");
        private readonly ParameterExpression paramY = Expression.Parameter(typeof(double), "y");

        private Func<T, T, T> MultiplyF;
        private Func<T, T, T> AdditionF;
        private Func<T, T, T> DifferenceF;

        public event EventHandler<IndexChangeEventArgs<T>> NewChange = delegate { };

        public T this[int nIndex, int mIndex]
        {
            get
            {
                return matrix[nIndex, mIndex];
            }
            set
            {
                T oldValue = matrix[nIndex, mIndex];
                matrix[nIndex, mIndex] = value;
                EventHandler<IndexChangeEventArgs<T>> temp = NewChange;
                temp(this, new IndexChangeEventArgs<T>(nIndex, mIndex, oldValue, value));
            }
        }

        public int SquareDimension
        {
            get
            {
                return dimension;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Can't be less than 1", nameof(value));
                }

                dimension = value;
            }
        }

        public int N
        {
            get
            {
                return SquareDimension;
            }
        }

        public int M
        {
            get
            {
                return SquareDimension;
            }
        }

        public SquareMatrix() : this(DefaultDemension)
        {

        }

        public SquareMatrix(int dimension) : this(dimension, new T[dimension, dimension])
        {

        }

        public SquareMatrix(int dimension, T[,] array)
        {
            if (array.GetLength(0) != array.GetLength(1))
            {
                throw new ArgumentException("array is not a square matrix");
            }

            if (dimension != array.GetLength(0))
            {
                throw new ArgumentException("dimension is not equal to array dimension");
            }

            SquareDimension = dimension;
            matrix = new T[dimension, dimension];
            Array.Copy(array, matrix, array.Length);
            Expression<Func<T, T, T>> expressionMultiply = Expression.Lambda<Func<T, T, T>>(Expression.Multiply(paramX, paramY), paramX, paramY);
            Expression<Func<T, T, T>> expressionAddition = Expression.Lambda<Func<T, T, T>>(Expression.Add(paramX, paramY), paramX, paramY);
            Expression<Func<T, T, T>> expressionDifference = Expression.Lambda<Func<T, T, T>>(Expression.Subtract(paramX, paramY), paramX, paramY);
            MultiplyF = expressionMultiply.Compile();
            AdditionF = expressionAddition.Compile();
            DifferenceF = expressionDifference.Compile();
            NewChange += Change;
        }

        public IMatrix<T> Multiply(IMatrix<T> anotherMatrix)
        {
            DimensionCheck(anotherMatrix);
            SquareMatrix<T> result = new SquareMatrix<T>(dimension);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    for (int k = 0; k < M; k++)
                    {
                        result[i, j] = AdditionF(result[i, j], MultiplyF(matrix[i, k], anotherMatrix[k, j]));
                    }
                }
            }

            return result;
        }

        public IMatrix<T> Multiply(T value)
        {
            SquareMatrix<T> result = new SquareMatrix<T>(dimension, matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = MultiplyF(result[i, j], value);
                }
            }

            return result;
        }

        public IMatrix<T> Difference(IMatrix<T> anotherMatrix)
        {
            DimensionCheck(anotherMatrix);
            SquareMatrix<T> result = new SquareMatrix<T>(dimension, matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = DifferenceF(result[i, j], anotherMatrix[i, j]);
                }
            }

            return result;
        }

        public IMatrix<T> Difference(T value)
        {
            SquareMatrix<T> result = new SquareMatrix<T>(dimension, matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = DifferenceF(result[i, j], value);
                }
            }

            return result;
        }

        public void Change(object sender, IndexChangeEventArgs<T> eventArgs)
        {
            Console.WriteLine($"Matrix value on index [{eventArgs.I},{eventArgs.J}] was changed from {eventArgs.OldValue} to {eventArgs.NewValue}");
        }

        private bool DimensionCheck(IMatrix<T> anotherMatrix)
        {
            if (N != anotherMatrix.N && M != anotherMatrix.M)
            {
                return false;
            }

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in matrix)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
