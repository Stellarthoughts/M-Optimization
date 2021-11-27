namespace MOptimization.MVVM
{
    using MOptimization.MFunction;

    public class OptimizationModel
    {
        private MSFunction function;
        private double eps;
            
        public OptimizationModel()
        {
            function = new SphereFunction1(eps);
        }
    }
}
