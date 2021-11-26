using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOptimization.MFunction
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
            checkArgs(args);
            double res = calculate(args);
            return res;
        }
        public double[] FirstDiff(double[] args)
        {
            checkArgs(args);
            double[] res = firstDiff(args);
            return res;
        }
        public double[] SecondDiff(double[] args)
        {
            checkArgs(args);
            double[] res = secondDiff(args);
            return res;
        }

        private void checkArgs(double[] args)
        {
            if (args.Length < ArgCount) throw new Exception("You screwed up with that pass");
        }

        protected abstract double calculate(double[] args);

        protected abstract double[] firstDiff(double[] args);

        protected abstract double[] secondDiff(double[] args);
    }

    public class SphereFunction1 : MSFunction
    {
        public SphereFunction1() : base(3, "(x-4)^2 + y^2 + z^2 - 2") { }

        protected override double calculate(double[] args)
        {
            return Math.Pow(args[0] - 4, 2) + Math.Pow(args[1], 2) + Math.Pow(args[2], 2) - 2;
        }

        protected override double[] firstDiff(double[] args)
        {
            double[] res = new double[ArgCount];

            for (int i = 0; i < ArgCount; i++)
            {
                double iRes = Double.PositiveInfinity;
                double prev;
                double prevAccuracy = Double.PositiveInfinity;
                double delta = 0.1;
                double factor = 10;
                double esp = 0.001;

                do
                {
                    prev = iRes;

                    double[] iterArgsLeft = (double[]) args.Clone();
                    double[] iterArgsRight = (double[]) args.Clone();

                    iterArgsLeft[i] -= delta;
                    iterArgsRight[i] += delta;

                    iRes = (calculate(iterArgsRight) - calculate(iterArgsLeft)) / delta / 2;

                    double currAccuracy = Math.Abs(iRes - prev);
                    if (currAccuracy > prevAccuracy)
                    {
                        break;
                    }
                    delta /= factor;
                    prevAccuracy = currAccuracy;
                }
                while (prevAccuracy > esp);

                res[i] = prev;
            }
            return res;
        }

        protected override double[] secondDiff(double[] args)
        {
            double[] res = new double[ArgCount];

            for (int i = 0; i < ArgCount; i++)
            {
                double iRes = Double.PositiveInfinity;
                double prev;
                double prevAccuracy = Double.PositiveInfinity;
                double delta = 0.1;
                double factor = 10;
                double esp = 0.001;

                do
                {
                    prev = iRes;

                    double[] iterArgsLeft = (double[])args.Clone();
                    double[] iterArgsRight = (double[])args.Clone();

                    iterArgsLeft[i] -= delta;
                    iterArgsRight[i] += delta;

                    iRes = (calculate(iterArgsRight) - calculate(iterArgsLeft)) / delta / 2;

                    double currAccuracy = Math.Abs(iRes - prev);
                    if (currAccuracy > prevAccuracy)
                    {
                        break;
                    }
                    delta /= factor;
                    prevAccuracy = currAccuracy;
                }
                while (prevAccuracy > esp);

                res[i] = prev;
            }
            return res;
        }
    }


}
