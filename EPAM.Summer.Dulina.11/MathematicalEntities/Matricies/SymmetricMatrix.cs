using System;
using System.Collections.Generic;
using System.Linq;
using MathematicalEntities.Abstracts;
using MathematicalEntities.Helpers;

namespace MathematicalEntities.Matricies
{
    public sealed class SymmetricMatrix<T> : Matrix<T>
    {

        public override T this[int nIndex, int mIndex]
        {
            set
            {
                if (mIndex > nIndex)
                    base[mIndex, nIndex] = value;
                else
                    base[nIndex, mIndex] = value;
            }
        }

        public SymmetricMatrix(T[,] array) : base(CheckMatrixForSymmetric(array))
        {
            SetDimensions(array.GetLength(0), array.GetLength(0));
        }

        private static T[][] CheckMatrixForSymmetric(T[,] array)
        {
            if (array.GetLength(0) != array.GetLength(1))
            {
                throw new ArgumentException("Array is not a square matrix");
            }
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    if (Comparer<T>.Default.Compare(array[i, j], array[j, i]) != 0)
                    {
                        throw new ArgumentException("Array is not a symmetric matrix");
                    }
                }
            }

            T[][] result = new T[array.GetLength(0)][];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                result[i] = new T[i + 1];
                for (int j = 0; j < i + 1; j++)
                {
                    result[i][j] = array[i, j];
                }
            }

            return result;
        }

        protected override T GetValue(int i, int j)
        {
            return j > i ? matrix[j][i] : matrix[i][j];
        }

        public override Matrix<T> CreateMatrix(IMatrix<T> matrix)
        {
            return new SymmetricMatrix<T>(matrix.ToArray());
        }
    }
}

