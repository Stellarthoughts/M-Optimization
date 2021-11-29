namespace MSOptimization.Models
{
    using MSOptimization.Core;
    using MSOptimization.NumericMethods;

    public class MSOptimizationModel
    {
        private MSFunction _function;
        private IOptimization _method;
        private double _eps;
        private double[] _init;
        private double _maxIter;

        public double Eps
        {
            get => _eps;
            set =>_eps = value;
        }

        public double[] InitValue
        {
            get => _init;
            set => _init = value;
        }

        public double MaximumIterations
        {
            get => _maxIter;
            set => _maxIter = value;
        }

        public MSFunction Function
        {
            get => _function;
            set => _function = value;
        }
        public IOptimization Method 
        { 
            get => _method; 
            set => _method = value; 
        }

        public MSOptimizationModel()
        {
            _method = new OptimizationMarquardt();
        }

        public OptimizationResult Optimize()
        {
            OptimizationResult res = Method.Optimize(_function,_init,_eps,_maxIter);
            return res;
        }

        public double CalcFunc(double[] args) => _function.Calculate(args);

    }
}
