using System;

namespace MOptimization.Core
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
        public double[,] Hessian(double[] args)
        {
            checkArgs(args);
            double[,] res = hessian(args);
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

                    double[] iterArgsLeft = new double[ArgCount];
                    double[] iterArgsRight = new double[ArgCount];

                    Array.Copy(args, iterArgsLeft, ArgCount);
                    Array.Copy(args, iterArgsRight, ArgCount);

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

        protected double[,] hessian(double[] args)
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
                            double[] iterArgsLeft = new double[ArgCount];
                            double[] iterArgsRight = new double[ArgCount];

                            Array.Copy(args, iterArgsLeft, ArgCount);
                            Array.Copy(args, iterArgsRight, ArgCount);

                            iterArgsLeft[i] -= delta;
                            iterArgsRight[i] += delta;

                            iRes = (calculate(iterArgsRight) - 2 * calculate(args) + calculate(iterArgsLeft)) / Math.Pow(delta, 2);
                        }
                        else
                        {
                            double[] Term1 = new double[ArgCount];
                            double[] Term2 = new double[ArgCount];
                            double[] Term3 = new double[ArgCount];
                            double[] Term4 = new double[ArgCount];

                            Array.Copy(args, Term1, ArgCount);
                            Array.Copy(args, Term2, ArgCount);
                            Array.Copy(args, Term3, ArgCount);
                            Array.Copy(args, Term4, ArgCount);

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
