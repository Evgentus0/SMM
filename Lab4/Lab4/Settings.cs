    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Settings
    {
        public const double DEVICE_1_MU = 0.23, 
                            DEVICE_2_TIME = 5 * TimeMeasure, 
                            DEVICE_3_TIME = 10 * TimeMeasure;

        public const int QUEUE_1_LIMIT = 9,
                         QUEUE_2_LIMIT = 3;

        public const int TimeMeasure = 100;

        private const double inputLambda = 0.22;

        public const int Delay = (int)((1.0 / inputLambda) * TimeMeasure);
    }

    public enum WorkMode
    {
        Intensity = 1,
        Time = 2
    }
}
