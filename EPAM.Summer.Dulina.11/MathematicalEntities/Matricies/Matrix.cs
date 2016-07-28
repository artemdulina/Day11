using System;
using System.Collections;
using System.Collections.Generic;
using MathematicalEntities.Abstracts;
using MathematicalEntities.Helpers;

namespace MathematicalEntities.Matricies
{
    public class Matrix<T> : MatrixAccept<T>, IMatrix<T>
    {
        protected T[][] matrix;

        public event EventHandler<IndexChangeEventArgs<T>> NewChange = delegate { };

        protected int rowCount;
        protected int columnCount;

        public virtual T this[int nIndex, int mIndex]
        {
            get
            {
                if (nIndex < 0 || nIndex >= rowCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(nIndex));
                }

                if (mIndex < 0 || mIndex >= columnCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(mIndex));
                }

                return GetValue(nIndex, mIndex);
            }
            set
            {
                T oldValue = matrix[nIndex][mIndex];
                matrix[nIndex][mIndex] = value;
                OnChange(new IndexChangeEventArgs<T>(nIndex, mIndex, oldValue, value));
            }
        }

        protected virtual void OnChange(IndexChangeEventArgs<T> e)
        {
            EventHandler<IndexChangeEventArgs<T>> temp = NewChange;
            temp(this, e);
        }

        public virtual int N
        {
            get
            {
                return rowCount;
            }
        }

        public virtual int M
        {
            get
            {
                return columnCount;
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

            this.rowCount = rowCount;
            this.columnCount = columnCount;
            matrix = new T[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                matrix[i] = new T[columnCount];
            }
            NewChange += Change;
        }

        public Matrix(T[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            NewChange += Change;
            rowCount = array.GetLength(0);
            columnCount = array.GetLength(1);
            matrix = new T[array.GetLength(0)][];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                matrix[i] = new T[array.GetLength(1)];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] = array[i, j];
                }
            }
        }

        protected void SetDimensions(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
        }

        protected Matrix(T[][] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            NewChange += Change;
            matrix = new T[array.GetLength(0)][];

            int currentColumnsCount = array[0].Length;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                matrix[i] = new T[array[i].Length];

                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] = array[i][j];
                }
            }
        }

        private Matrix<T> MatrixConvert(Matrix<T> matrix)
        {
            Matrix<T> matrixConverted = new Matrix<T>(matrix.N, matrix.M);
            for (int i = 0; i < matrix.N; i++)
            {
                for (int j = 0; j < matrix.M; j++)
                {
                    matrixConverted[i, j] = matrix[i, j];
                }
            }

            return matrixConverted;
        }

        protected virtual T GetValue(int i, int j)
        {
            return matrix[i][j];
        }

        public virtual IMatrix<T> Multiply(IMatrix<T> anotherMatrix)
        {
            if (M != anotherMatrix.N)
            {
                throw new ArgumentException("Inappropriate dimension of the matrices");
            }

            Matrix<T> result = new Matrix<T>(N, M);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < anotherMatrix.M; j++)
                {
                    for (int k = 0; k < anotherMatrix.N; k++)
                    {
                        result[i, j] = OperationHelper<T>.AdditionFunction(result[i, j], OperationHelper<T>.MultiplyFunction(this[i, k], anotherMatrix[k, j]));
                    }
                }
            }

            return result;
        }

        public virtual IMatrix<T> Multiply(T value)
        {
            Matrix<T> result = MatrixConvert(this);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    result[i, j] = OperationHelper<T>.MultiplyFunction(result[i, j], value);
                }
            }

            return result;
        }

        protected void Change(object sender, IndexChangeEventArgs<T> eventArgs)
        {
            //Console.WriteLine($"Matrix value on index [{eventArgs.I},{eventArgs.J}] was changed from {eventArgs.OldValue} to {eventArgs.NewValue}");
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T[] item in matrix)
            {
                foreach (T value in item)
                {
                    yield return value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
