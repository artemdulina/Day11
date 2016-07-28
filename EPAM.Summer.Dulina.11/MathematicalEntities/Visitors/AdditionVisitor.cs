using System;
using MathematicalEntities.Helpers;
using MathematicalEntities.Matricies;

namespace MathematicalEntities.Visitors
{
    public class AdditionVisitor<T> : IVisitor<T>
    {
        public Matrix<T> Result { get; private set; }
        public Matrix<T> MatrixToAdd { get; }

        public AdditionVisitor(Matrix<T> matrixToAdd)
        {
            MatrixToAdd = matrixToAdd;
        }
        public void VisitMatrix(Matrix<T> matrix)
        {
            if (matrix.N != MatrixToAdd.N || matrix.M != MatrixToAdd.M)
            {
                throw new ArgumentException("Inappropriate dimension of the matrices");
            }

            Result = new Matrix<T>(matrix.N, matrix.M);
            for (int i = 0; i < matrix.N; i++)
            {
                for (int j = 0; j < matrix.M; j++)
                {
                    Result[i, j] = OperationHelper<T>.AdditionFunction(matrix[i, j], MatrixToAdd[i, j]);
                }
            }
        }
    }
}
