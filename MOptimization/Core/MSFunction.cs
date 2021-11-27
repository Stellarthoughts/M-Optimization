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


}
