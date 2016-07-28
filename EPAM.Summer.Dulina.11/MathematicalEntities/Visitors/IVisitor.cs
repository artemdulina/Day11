using MathematicalEntities.Matricies;

namespace MathematicalEntities.Visitors
{
    public interface IVisitor<T>
    {
        void VisitMatrix(Matrix<T> matrix);
    }
}
