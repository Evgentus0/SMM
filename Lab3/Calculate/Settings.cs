using System;
using System.Collections.Generic;
using System.Text;

namespace Calculate
{
    public class Settings
    {
        public static  int N = 10;

        public static  double[] Mu = new double[2] { 0.8, 0.4 };

        public static  double[,] P = new double[2, 2] { { 0.4, 0.6 }, 
                                                        { 0.7, 0.3 } };
        public static readonly  int[] R = new int[2] { 3, 3 };
    }
}
