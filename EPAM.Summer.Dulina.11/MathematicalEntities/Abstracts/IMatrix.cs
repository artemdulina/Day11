using System.Collections.Generic;

namespace MathematicalEntities.Abstracts
{
    public interface IMatrix<TValue> : IEnumerable<TValue>
    {
        TValue this[int nIndex, int mIndex] { get; set; }
        int N { get; }
        int M { get; }

        IMatrix<TValue> Multiply(IMatrix<TValue> anotherMatrix);
        IMatrix<TValue> Multiply(TValue value);

        TValue[,] ToArray();
    }
}
