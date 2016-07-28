using MathematicalEntities.Matricies;
using MathematicalEntities.Visitors;

namespace MathematicalEntities.Extensions
{
    public static class MatrixExtensions
    {
        public static Matrix<T> Addition<T>(this Matrix<T> matrix, Matrix<T> anotherMatrix)
        {
            AdditionVisitor<T> visitor = new AdditionVisitor<T>(anotherMatrix);
            matrix.Accept(visitor);
            return visitor.Result;
        }
    }
}
