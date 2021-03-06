using System;

namespace MSOptimization.Core
{
	public class SLE
	{
		private double[,] mat;
		private int dimension;

		public SLE(int dimension)
		{
			this.dimension = dimension;
			mat = new double[dimension, dimension + 1];
		}

		public void MakeRandom(double min, double max)
		{
			if (min > max)
			{
				double t = min;
				min = max;
				max = t;
			}
			Random random = new Random();
			for (int i = 0; i < dimension; i++)
			{
				for (int j = 0; j < dimension + 1; j++)
				{
					mat[i, j] = random.NextDouble() * (max - min) + min;
				}
			}
		}

		public void FillFromMatrix(double[,] mat)
		{
			for (int i = 0; i < mat.GetLength(0); i++)
			{
				for (int j = 0; j < mat.GetLength(1); j++)
				{
					this.mat[i, j] = mat[i, j];
				}
			}
		}

		public double Get(int i, int j)
		{
			return mat[i, j];
		}

		public void Set(int i, int j, double d)
		{
			mat[i, j] = d;
		}

		public double[] VerifySolution(double[] solution)
		{
			double[] sleSum = new double[dimension];
			for (int i = 0; i < dimension; i++)
			{
				sleSum[i] = -mat[i, dimension];
				for (int j = 0; j < dimension; j++)
				{
					sleSum[i] += mat[i, j] * solution[j];
				}
			}
			return sleSum;
		}

		public double[,] GetPrimeMatrix()
		{
			double[,] pMat = new double[dimension, dimension];
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					pMat[i, j] = mat[i, j];
			return pMat;
		}

		public double[,] GetFullMatrix()
		{
			return (double[,]) mat.Clone();
		}

		public int Dimension { get => dimension; set => dimension = value; }
	}
}
