

namespace MSOptimization.NumericMethods
{
	using MSOptimization.Core;
	using System;

	public class OptimizationResult
	{
		private readonly double[] _point;
		private readonly double _value;

		public OptimizationResult(double[] point, double value)
		{
			_point = point;
			_value = value;
		}

		public double[] Point => _point;

		public double Value => _value;
	}
	
	public interface IOptimization
	{
		public OptimizationResult Optimize(MSFunction function, double[] init, double eps, double maxIter);
	}

	public class OptimizationMarquardt : IOptimization
	{
		public OptimizationResult Optimize(MSFunction function, double[] init, double eps, double maxIter)
		{
			// Step 1
			double[] temp = new double[function.ArgCount]; 
			Array.Copy(init, approx, function.ArgCount);
			double[,] approx = MatrixOperations.VecToMat(temp);

			// Step 2
			int k = 0;
			double lambda = Math.Pow(10, 4);

			while (true)
			{
				// Step 3
				double[] gradient_k = Differentiation.Gradient(function, approx, eps);


				// Step 4
				if(MatrixOperations.VecEuqNorm(gradient_k) < eps)
				{
					break;
				}
				// Step 5
				else if(k >= maxIter)
				{
					break;
				}
				else
				{
					// Step 6
					double[,] tbReversed = MatrixOperations.Add(
						Differentiation.Hessian(function, approx, eps),
						MatrixOperations.Multiply(Matrix.IdentityMatrix(function.ArgCount), lambda)
						);
					tbReversed = MatrixOperations.Inverse(SLESolver.LUDecomp, tbReversed, eps);
					tbReversed = MatrixOperations.Multiply(tbReversed, -1);
					double[,] gradient_k_mForm = MatrixOperations.Transpose(MatrixOperations.VecToMat(gradient_k));
					double[,] s_xl = MatrixOperations.Multiply(tbReversed, gradient_k_mForm);

					double[,] x1 = MatrixOperations.Add(approx, s_xl);

				}
				
			}

			OptimizationResult res = new(new double[1], 0);
			return res;
		}
	}
}
