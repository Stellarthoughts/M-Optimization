namespace MSOptimization.NumericMethods
{
	using MSOptimization.Core;
	using System;
    using System.Collections.Generic;

    public class Iteration
    {
		private readonly double[] _point;
		private readonly double _value;
		private readonly double[] _gradient;
		private readonly double[,] _hessian;
		private readonly double _lambda;
		private readonly int _sequenceNumber;

        public Iteration(double[] point, double value, double[] gradient, double[,] hessian, double lambda, int sequenceNumber)
        {
            _point = point;
            _value = value;
            _gradient = gradient;
            _hessian = hessian;
            _lambda = lambda;
            _sequenceNumber = sequenceNumber;
        }

		public double[] Point => _point;

        public double Value => _value;

        public double[] Gradient => _gradient;

        public double[,] Hessian => _hessian;

        public double Lambda => _lambda;

        public int SequenceNumber => _sequenceNumber;
    }

	public class OptimizationResult
	{
		private readonly double[] _point;
		private readonly double _value;
		private readonly int _iterationsCount;
		private readonly bool _isAccuracyAchived;
		private readonly List<Iteration> _iterations;

        public OptimizationResult(double[] point, double value, int iterationsCount, bool isAccuracyAchived, List<Iteration> iterations)
        {
            _point = point;
            _value = value;
            _iterationsCount = iterationsCount;
            _isAccuracyAchived = isAccuracyAchived;
			_iterations = iterations;
        }

        public double[] Point => _point;

        public double Value => _value;

        public int IterationsCount => _iterationsCount;

        public bool IsAccuracyAchived => _isAccuracyAchived;

		public List<Iteration> Iterations => _iterations;
    }
	
	public interface IOptimization
	{
		public OptimizationResult Optimize(MSFunction function, double[] init, double eps, double maxIter);
	}

	public class OptimizationMarquardt : IOptimization
	{
		public OptimizationResult Optimize(MSFunction function, double[] init, double eps, double maxIter)
		{
			bool _isAccuracyAchived = false;
			List<Iteration> _iterations = new();
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
					_isAccuracyAchived = true;
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
						double[,] hessian = Differentiation.Hessian(function, x, eps);
						double[,] tbReversed = MatrixOperations.Add(
							hessian,
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
							_iterations.Add(new Iteration(x, function.Calculate(x), gradient_k, hessian, lambda, k));
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

			OptimizationResult res = new(x, function.Calculate(x), k, _isAccuracyAchived, _iterations);
			return res;
		}
	}
}
