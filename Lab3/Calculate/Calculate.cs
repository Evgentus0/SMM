using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculate
{
    public class Calculate
    {
        double[] E = new double[2];
        public Calculate()
        {
            FindE();
        }

        public double P(int i, int k)
        {
            double temp = Math.Pow((E[i] / Settings.Mu[i]), k);

            if (k <= Settings.R[i])
            {
                return temp / Enumerable.Range(1, k).Aggregate(1, (p, item) => p * item);
            }
            else
            {
                int factorial_ri = Enumerable.Range(1, Settings.R[i]).Aggregate(1, (p, item) => p * item);

                return Math.Pow(E[i] / Settings.Mu[i], k) / (factorial_ri * Math.Pow(Settings.R[i], k - Settings.R[i]));
            }
        }

        public void FindE()
        {
            E[0] = 1;

            E[1] = (1 - Settings.P[0, 0]) / Settings.P[1, 0];
        }

        public double FindC()
        {

            double result = 0;
            for (int i = 0; i <= Settings.N; i++)
            {
                result += P(0, i) * P(1, Settings.N - i);
            }

            return 1.0 / result;
        }

        public double P_SMO(int i, int j)
        {
            double cN = FindC();

            var result = 0.0;

            if (i == 0)
            {
                result += P(0, j) * P(1, Settings.N - j);
            }
            else
            {
                result += P(1, j) * P(0, Settings.N - j);
            }

            return result * cN;
        }


        //results

        //середня кількість вимог у черзі СМОi розраховується як математичне сподівання випадкової величини „кількість вимог у черзі”:
        public double Li(int i)
        {
            double sum = 0;
            for (int j = Settings.R[i] + 1; j <= Settings.N; j++)
            {
                sum += (j - Settings.R[i]) * P_SMO(i, j);
            }
            return sum;
        }

        //середня кількість зайнятих пристроїв у СМОi розраховується якзагальна кількість пристроїв мінус середня кількість вільних пристроїв:
        public double Ri(int i)
        {
            double sum = 0;
            for (int j = 0; j <= Settings.R[i] - 1; j++)
            {
                sum += (Settings.R[i] - j) * P_SMO(i, j);
            }
            return Settings.R[i] - sum;
        }

        //середня кількість вимог у СМОi розраховується як сума кількості вимог у черзі і у пристроях:
        public double Mi(int i)
        {
            return Li(i) + Ri(i);
        }

        //інтенсивність вихідного потоку вимог у СМОi:
        public double Lambda_i(int i)
        {
            return (double)(Ri(i) / Settings.Mu[i]);
        }

        //середній час перебування вимоги в СМОi:
        public double Ti(int i)
        {
            return (double)(Mi(i) / Lambda_i(i));
        }

        //середній час очікування в черзі СМОi:
        public double Qi(int i)
        {
            return (double)(Li(i) / Lambda_i(i));
        }
    }
}
