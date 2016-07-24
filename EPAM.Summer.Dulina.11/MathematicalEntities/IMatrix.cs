using System.Collections.Generic;

namespace MathematicalEntities
{
    public interface IMatrix<TValue> : IEnumerable<TValue>
    {
        TValue this[int nIndex, int mIndex] { get; set; }
        int N { get; }
        int M { get; }

        IMatrix<TValue> Multiply(IMatrix<TValue> anotherMatrix);
        IMatrix<TValue> Multiply(TValue value);
        IMatrix<TValue> Difference(IMatrix<TValue> matrix);
        IMatrix<TValue> Difference(TValue value);
    }
}
