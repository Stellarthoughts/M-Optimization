using System;

namespace MSOptimization.Core
{
    // multivar scalar
    public abstract class MSFunction
    {
        protected readonly int _argc;
        protected readonly string _notation;

        public MSFunction(int argc, string notation)
        {
            _argc = argc;
            _notation = notation;
        }

        public int ArgCount
        {
            get => _argc;
        }

        public string Notation
        {
            get => _notation;
        }

        public double Calculate(double[] args)
        {
            CheckArgs(args);
            double res = FunctionValue(args);
            return res;
        }

        private void CheckArgs(double[] args)
        {
            if (args.Length != ArgCount) throw new Exception("You screwed up with that pass (args.Length != argc)");
        }

        protected abstract double FunctionValue(double[] args);
    }

    public class SphereFunction1 : MSFunction
    {
        public SphereFunction1() : base(3, "(x-4)^2 + y^2 + z^2 - 2") { }

        protected override double FunctionValue(double[] args)
        {
            return Math.Pow(args[0] - 4, 2) + Math.Pow(args[1], 2) + Math.Pow(args[2], 2) - 2;
        }
    }

    public class RosenbrockFunction : MSFunction
    {
        public RosenbrockFunction() : base(2, "100 * (y - x^2)^2 + (1 - x)^2") {}

        protected override double FunctionValue(double[] args)
        {
            return 10 * Math.Pow((args[1] - Math.Pow(args[0], 2)), 2) + Math.Pow((1 - args[0]),2);
        }
    }

    public class PowellFunction : MSFunction
    {
        public PowellFunction() : base(4, "(x1 + 10 * x2)^2 + 5(x3 - x4)^2 + (x2 - 2 * x3)^4 + 10(x1 - x4)^4") { }
        protected override double FunctionValue(double[] args)
        {
            return Math.Pow(args[0] + 10 * args[1], 2) 
                + 5 * Math.Pow(args[2] - args[3], 2) 
                + Math.Pow(args[1] - 2 * args[2],4) 
                + 10 * Math.Pow(args[0] - args[3], 4);
        }
    }

    /*public class D2Exponential : MSFunction
    {
        public D2Exponential() : base(2, "(x1 + 10 * x2)^2 + 5(x3 - x4)^2 + (x2 - 2 * x3)^4 + 10(x1 - x4)^4") { }
        protected override double FunctionValue(double[] args)
        {
            throw new NotImplementedException();
        }
    }*/

}
