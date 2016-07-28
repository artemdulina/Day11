using MathematicalEntities.Visitors;

namespace MathematicalEntities.Abstracts
{
    public abstract class MatrixAccept<T>
    {
        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitMatrix((dynamic)this);
        }
    }
}
