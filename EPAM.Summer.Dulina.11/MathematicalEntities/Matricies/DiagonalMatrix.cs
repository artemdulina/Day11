using System;

namespace MathematicalEntities.Matricies
{
    public sealed class DiagonalMatrix<T> : Matrix<T>
    {
        public override T this[int nIndex, int mIndex]
        {
            set
            {
                if (nIndex == mIndex)
                {
                    base[0, nIndex] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(nIndex), "Can't set values not on diagonal");
                }
            }
        }

        public DiagonalMatrix(T[] array) : base(ConvertToMultiDimensionalArray(array))
        {
            SetDimensions(array.Length, array.Length);
        }

        protected override T GetValue(int i, int j)
        {
            return i == j ? matrix[0][i] : default(T);
        }

        private static T[,] ConvertToMultiDimensionalArray(T[] array)
        {
            T[,] converted = new T[1, array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                converted[0, i] = array[i];
            }
            return converted;
        }
    }
}
