using MSOptimization.NumericMethods;
using System;
using System.Text;

namespace MSOptimization.Core
{
    public static class MatrixOperations
	{
		public static double[,] Inverse(SLESolvingMethod method, double[,] mat, double eps)
		{
			int N = mat.GetLength(0);
			int M = mat.GetLength(1);
			if (N != M) return null;

			double[,] inverse = new double[N, N];
			for (int j = 0; j < N; j++)
			{
				SLE row = new(N);
				row.FillFromMatrix(mat);
				row.Set(j, row.Dimension, 1);
				SLESolverResult solved = method(new SLESolverSettings()
				{
					sle = row,
					eps = eps
				});
				if (!solved.convergence)
					throw new Exception("Одна или несколько СЛАУ для вычисления обратной матрицы не сходится.");
				for (int i = 0; i < N; i++)
					inverse[i, j] = solved.solution[i];
			}
			return inverse;
		}

		public static double[,] Multiply(double[,] left, double[,] right)
		{
			if (left.GetLength(1) != right.GetLength(0))
				throw new Exception("Умножаемые матрицы имеют несоответсвующие размерности.");
			double[,] multiple = new double[left.GetLength(0), right.GetLength(1)];

			for (int i = 0; i < left.GetLength(0); i++)
			{
				for (int j = 0; j < right.GetLength(1); j++)
				{
					double sum = 0;
					for (int k = 0; k < left.GetLength(1); k++)
					{
						sum += left[i, k] * right[k, j];
					}
					multiple[i, j] = sum;
				}
			}

			return multiple;
		}

		public static double[,] VecToMat(double[] vec)
        {
			double[,] mat = new double[1, vec.Length];
			for(int i = 0; i < vec.Length; i++)
            {
				mat[0, i] = vec[i];
            }
			return mat;
        }

		public static double[] MatToVec(double[,] mat)
        {
			int height = mat.GetLength(0);
			int width = mat.GetLength(1);
			double[] vec = new double[height > width ? height : width];
			for(int i = 0; i < (height > width ? height : width); i++)
            {
				double[ ]
            }
			return vec;
        }

		public static double[,] Transpose(double[,] mat)
        {
			double[,] res = new double[mat.GetLength(1), mat.GetLength(0)];
			for (int i = 0; i < mat.GetLength(0); i++)
			{
				for (int j = 0; j < mat.GetLength(1); j++)
				{
					res[j,i] = mat[i, j];
				}
			}
			return res;
        }

		public static double[,] Multiply(double[,] mat, double k)
        {
			double[,] res = new double[mat.GetLength(0), mat.GetLength(1)];
			for (int i = 0; i < mat.GetLength(0); i++)
			{
				for (int j = 0; j < mat.GetLength(1); j++)
				{
					res[i, j] = mat[i, j] * k;
				}
			}
			return res;
		}

		public static double[,] Add(double[,] left, double [,] right)
        {
			double[,] res = new double[left.GetLength(0), right.GetLength(1)];
			for(int i = 0; i < left.GetLength(0); i++)
            {
				for(int j = 0; j < left.GetLength(1); j++)
                {
					res[i, j] = left[i, j] + right[i, j];
                }
            }
			return res;
        }

		public static string Stringify(double[,] mat)
		{
			StringBuilder str = new();
			for (int i = 0; i < mat.GetLength(0); i++)
			{
				for (int j = 0; j < mat.GetLength(1); j++)
				{
					if (j + 1 != mat.GetLength(1))
						str.Append(mat[i, j] + " ");
					else
						str.Append(mat[i, j]);
				}
				if (i + 1 != mat.GetLength(0))
					str.Append('\n');
			}
			return str.ToString();
		}

		public static string Stringify(double[,] mat, int digits)
		{
			StringBuilder str = new();
			for (int i = 0; i < mat.GetLength(0); i++)
			{
				for (int j = 0; j < mat.GetLength(1); j++)
				{
					if (j + 1 != mat.GetLength(1))
						str.Append(Math.Round(mat[i, j], digits) + " ");
					else
						str.Append(Math.Round(mat[i, j], digits));
				}
				if (i + 1 != mat.GetLength(0))
					str.Append('\n');
			}
			return str.ToString();
		}

		public static double VecEuqNorm(double[] vec)
        {
			double res = 0;
			for(int i = 0; i < vec.Length; i++)
            {
				res += Math.Pow(vec[i], 2);
            }
			return Math.Sqrt(res);
        }

		// TODO: ВСЕ ОЧЕНЬ ПЛОХО. Сделать так, чтобы SLE зависел от MatrixOperation, но не наоборот. Универсиализация зависимостей.
		public static double MatFirstNorm(double[,] mat)
        {
			double width = mat.GetLength(0);
			double height = mat.GetLength(1);

			double normMax = Double.NegativeInfinity;
			for (int i = 0; i < height; i++)
			{
				double normNext = 0;
				for (int j = 0; j < width; j++)
				{
					if (i == j) continue;
					normNext += Math.Abs(mat[i,j] / mat[i,i]);
				}
				if (normNext > normMax) normMax = normNext;
			}

			return normMax;
		}

		public static double MatSecondNorm(double [,] mat)
        {
			double width = mat.GetLength(0);
			double height = mat.GetLength(1);

			double normMax = Double.NegativeInfinity;
			for (int i = 0; i < height; i++)
			{
				double normNext = 0;
				for (int j = 0; j < width; j++)
				{
					if (i == j) continue;
					normNext += Math.Abs(mat[j,i] / mat[j,j]);
				}
				if (normNext > normMax) normMax = normNext;
			}

			return normMax;
		}
	}
}
