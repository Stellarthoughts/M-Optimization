namespace MSOptimization.NumericMethods
{
	using MSOptimization.Core;
	using System;

	public class OptimizationResult
	{
		private readonly double[] _point;
		private readonly double _value;
		private readonly int _iterations;

		public OptimizationResult(double[] point, double value, int iterations)
		{
			_point = point;
			_value = value;
			_iterations = iterations;
		}

		public double[] Point => _point;

		public double Value => _value;

		public double Iterations => _iterations;
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
			double[] x = new double[function.ArgCount]; 
			Array.Copy(init, x, function.ArgCount);

			// Step 2
			int k = 0;
			double lambda = Math.Pow(10, 4);

			while (true)
			{
				// Step 3
				double[] gradient_k = Differentiation.Gradient(function, x, eps);

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
					while (true)
					{
						// Step 6
						double[,] tbReversed = MatrixOperations.Add(
							Differentiation.Hessian(function, x, eps),
							MatrixOperations.Multiply(Matrix.IdentityMatrix(function.ArgCount), lambda)
							);
						tbReversed = MatrixOperations.Inverse(SLESolver.LUDecomp, tbReversed, eps);
						tbReversed = MatrixOperations.Multiply(tbReversed, -1);
						double[,] gradient_k_mForm = MatrixOperations.Transpose(MatrixOperations.VecToMat(gradient_k));
						double[,] s_xl = MatrixOperations.Multiply(tbReversed, gradient_k_mForm);

						// Step 7
						double[] x1 = MatrixOperations.MatToVec(
							MatrixOperations.Add(
								MatrixOperations.Transpose(MatrixOperations.VecToMat(x)),
								s_xl)
							);

						// Step 8
						if (function.Calculate(x1) < function.Calculate(x))
						{
							// Step 9
							x = x1;
							lambda /= 2;
							k++;
							break;
						}
						else
						{
							// Step 10
							lambda *= 2;
							continue;
						}
					}
				}
			}

			OptimizationResult res = new(x, function.Calculate(x), k);
			return res;
		}
	}
}
