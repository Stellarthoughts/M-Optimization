using System;

namespace MOptimization.MFunction
{
    // multivar scalar
    public abstract class MSFunction
    {
        protected readonly int _argc;
        protected readonly string _notation;
        protected readonly double _eps;

        public MSFunction(double eps, int argc, string notation)
        {
            _eps = eps;
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

        public double Eps
        {
            get => _eps;
        }

        public double Calculate(double[] args)
        {
            checkArgs(args);
            double res = calculate(args);
            return res;
        }
        public double[] Gradient(double[] args)
        {
            checkArgs(args);
            double[] res = gradient(args);
            return res;
        }
        public double[,] Hesse(double[] args)
        {
            checkArgs(args);
            double[,] res = hesse(args);
            return res;
        }

        private void checkArgs(double[] args)
        {
            if (args.Length != ArgCount) throw new Exception("You screwed up with that pass (args.Length != argc)");
        }

        protected abstract double calculate(double[] args);

        protected double[] gradient(double[] args)
        {
            double[] res = new double[ArgCount];

            for (int i = 0; i < ArgCount; i++)
            {
                double iRes = Double.PositiveInfinity;
                double prev;
                double prevAccuracy = Double.PositiveInfinity;
                double delta = 0.1;
                double factor = 10;

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
                while (prevAccuracy > Eps);

                res[i] = prev;
            }
            return res;
        }

        protected double[,] hesse(double[] args)
        {
            double[,] res = new double[ArgCount,ArgCount];

            for (int i = 0; i < ArgCount; i++)
            {
                for (int j = i; j < ArgCount; j++)
                {
                    double iRes = Double.PositiveInfinity;
                    double prev;
                    double prevAccuracy = Double.PositiveInfinity;
                    double delta = 0.1;
                    double factor = 10;

                    do
                    {
                        prev = iRes;

                        if(i == j)
                        {
                            double[] iterArgsLeft = (double[]) args.Clone();
                            double[] iterArgsRight = (double[]) args.Clone();

                            iterArgsLeft[i] -= delta;
                            iterArgsRight[i] += delta;

                            iRes = (calculate(iterArgsRight) - 2 * calculate(args) + calculate(iterArgsLeft)) / Math.Pow(delta, 2);
                        }
                        else
                        {
                            double[] Term1 = (double[]) args.Clone();
                            double[] Term2 = (double[])args.Clone();
                            double[] Term3 = (double[])args.Clone();
                            double[] Term4 = (double[])args.Clone();

                            Term1[i] += delta; Term1[j] += delta;
                            Term2[i] += delta; Term2[j] -= delta;
                            Term3[i] -= delta; Term3[j] += delta;
                            Term4[i] -= delta; Term4[j] -= delta;

                            iRes = (calculate(Term1) - calculate(Term2) - calculate(Term3) + calculate(Term4)) / (4 * Math.Pow(delta, 2));
                        }

                        double currAccuracy = Math.Abs(iRes - prev);
                        if (currAccuracy > prevAccuracy)
                        {
                            break;
                        }
                        delta /= factor;
                        prevAccuracy = currAccuracy;
                    }
                    while (prevAccuracy > Eps);

                    res[i,j] = prev;
                }
            }

            for(int i = 1; i < ArgCount; i++)
            {
                for(int j = 0; j < i; j++)
                {
                    res[i, j] = res[j, i];
                }
            }
            return res;
        }
    }

    public class SphereFunction1 : MSFunction
    {
        public SphereFunction1(double eps) : base(eps, 3, "(x-4)^2 + y^2 + z^2 - 2") { }

        protected override double calculate(double[] args)
        {
            return Math.Pow(args[0] - 4, 2) + Math.Pow(args[1], 2) + Math.Pow(args[2], 2) - 2;
        }
    }


}
