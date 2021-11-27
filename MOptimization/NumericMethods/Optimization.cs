

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
            int k = 0;
            double[] approx = new double[function.ArgCount]; 
            Array.Copy(init, approx, function.ArgCount);
            double lambda = Math.Pow(10, 4);


            while(true)
            {
                double[] gradient_k = Differentiation.Gradient(function, approx, eps);
                if(MatrixOperations.VecEuqNorm(gradient_k) < eps)
                {

                }
                else if(k >= maxIter)
                {

                }
                double[] s_xl = new double[function.ArgCount];

            }

            OptimizationResult res = new(new double[1], 0);
            return res;
        }
    }
}
