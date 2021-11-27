namespace MOptimization.Models
{
    using MOptimization.Core;

    public class MSOptimizationModel
    {
        private MSFunction function;
        private double eps = 0.001;
        private double[] init = { 0, 0, 0 };

        public MSOptimizationModel()
        {
            function = new SphereFunction1(eps);
            function.Gradient(init);
            function.Hessian(init);
        }
        public double[] Calculate()
        {

        }
    }
}
