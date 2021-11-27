namespace MSOptimization.Core
{
    public static class Matrix
    {
        public static double[,] IdentityMatrix(int dimension)
        {
            double[,] mat = new double[dimension, dimension];
            for(int i = 0; i < dimension; i++)
            {
                for(int j = 0; j < dimension; j++)
                {
                    if (i == j) mat[i, j] = 1;
                    else mat[i, j] = 0;
                }
            }
            return mat;
        }
    }
}
