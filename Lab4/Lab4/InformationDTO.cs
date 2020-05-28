using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class InformationDTO
    {
        public double RejectionProcent { get; set; }

        public double Queue1AvrLength { get; set; }
        public double Queue2AvrLength { get; set; }

        public double Queue1AvrTimeAsReal { get; set; }
        public double Queue2AvrTimeAsReal { get; set; }

        public double Queue1AvrTime { get; set; }
        public double Queue2AvrTime { get; set; }

        public double Device1Loading { get; set; }
        public double Device2Loading { get; set; }
        public double Device3Loading { get; set; }
    }
}
