using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities
{
    public class Matrix<T> : IMatrix<T>
    {
        protected readonly ParameterExpression paramX = Expression.Parameter(typeof(T), "x");
        protected readonly ParameterExpression paramY = Expression.Parameter(typeof(T), "y");

        protected Func<T, T, T> MultiplyF;
        protected Func<T, T, T> AdditionF;
        protected Func<T, T, T> DifferenceF;

        protected readonly T[,] matrix;
        protected readonly T[] vector;

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
                OnChange(new IndexChangeEventArgs<T>(nIndex, mIndex, oldValue, value));
            }
        }

        protected virtual void OnChange(IndexChangeEventArgs<T> e)
        {
            EventHandler<IndexChangeEventArgs<T>> temp = NewChange;
            temp(this, e);
        }

        public int N
        {
            get
            {
                return matrix.GetLength(0);
            }
        }

        public int M
        {
            get
            {
                return matrix.GetLength(1);
            }
        }

        private Matrix() { }

        public Matrix(int rowCount, int columnCount)
        {
            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "Can't be less than 1");
            }

            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "Can't be less than 1");
            }

            matrix = new T[rowCount, columnCount];
        }

        public Matrix(T[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            matrix = new T[array.GetLength(0), array.GetLength(1)];
            Array.Copy(array, matrix, array.Length);
            NewChange += Change;

            Expression<Func<T, T, T>> expressionMultiply = Expression.Lambda<Func<T, T, T>>(Expression.Multiply(paramX, paramY), paramX, paramY);
            Expression<Func<T, T, T>> expressionAddition = Expression.Lambda<Func<T, T, T>>(Expression.Add(paramX, paramY), paramX, paramY);
            Expression<Func<T, T, T>> expressionDifference = Expression.Lambda<Func<T, T, T>>(Expression.Subtract(paramX, paramY), paramX, paramY);
            MultiplyF = expressionMultiply.Compile();
            AdditionF = expressionAddition.Compile();
            DifferenceF = expressionDifference.Compile();
        }

        public virtual IMatrix<T> Multiply(IMatrix<T> anotherMatrix)
        {
            if (M != anotherMatrix.N)
            {
                throw new ArgumentException("Inappropriate dimension of of the matrices");
            }

            Matrix<T> result = new Matrix<T>(N, M);
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

        public virtual IMatrix<T> Multiply(T value)
        {
            Matrix<T> result = new Matrix<T>(matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = MultiplyF(result[i, j], value);
                }
            }

            return result;
        }

        public virtual IMatrix<T> Difference(IMatrix<T> anotherMatrix)
        {
            if (N != anotherMatrix.N || M != anotherMatrix.M)
            {
                throw new ArgumentException("Inappropriate dimension of of the matrices");
            }

            Matrix<T> result = new Matrix<T>(matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = DifferenceF(result[i, j], anotherMatrix[i, j]);
                }
            }

            return result;
        }

        public virtual IMatrix<T> Difference(T value)
        {
            Matrix<T> result = new Matrix<T>(matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = DifferenceF(result[i, j], value);
                }
            }

            return result;
        }

        protected void Change(object sender, IndexChangeEventArgs<T> eventArgs)
        {
            Console.WriteLine($"Matrix value on index [{eventArgs.I},{eventArgs.J}] was changed from {eventArgs.OldValue} to {eventArgs.NewValue}");
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
