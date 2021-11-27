namespace MSOptimization.Core
{
    using System;
    public static class Differentiation
    {
        public static double[] Gradient(MSFunction function, double[] args, double eps)
        {
            int ArgCount = function.ArgCount;
            Func<double[],double> calculate = function.Calculate;

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
                while (prevAccuracy > eps);

                res[i] = prev;
            }
            return res;
        }

        public static double[,] Hessian(MSFunction function, double[] args, double eps)
        {
            int ArgCount = function.ArgCount;
            Func<double[], double> calculate = function.Calculate;

            double[,] res = new double[ArgCount, ArgCount];

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

                        if (i == j)
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
                    while (prevAccuracy > eps);

                    res[i, j] = prev;
                }
            }

            for (int i = 1; i < ArgCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    res[i, j] = res[j, i];
                }
            }
            return res;
        }
    }
}
