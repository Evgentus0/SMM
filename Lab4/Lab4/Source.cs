using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab4
{
    public class Source
    {
        private double customerCount;
        private double queue1Length;
        private double queue2Length;

        private CustomQueue queue1;
        private CustomQueue queue2;

        private Device device1;
        private Device device2;
        private Device device3;

        private List<Customer> servedCustomers;

        private CustomQueue helpQueue;
        private Device helpDevice;

        private object loker = new object();

        private string queue1Name = "Queue1";
        private string queue2Name = "Queue2";

        public Source()
        {
            customerCount = 0;
            queue1Length = 0;
            queue2Length = 0;
        }


        public void ExecuteAsync(CancellationToken stoppingToken)
        {
            void Init()
            {
                servedCustomers = new List<Customer>();

                queue1 = new CustomQueue(queue1Name, Settings.QUEUE_1_LIMIT, stoppingToken);
                queue2 = new CustomQueue(queue2Name, Settings.QUEUE_2_LIMIT, stoppingToken);

                helpQueue = new CustomQueue("HelpQueue", 1, stoppingToken);

                device1 = new Device("Device1", Settings.DEVICE_1_MU, queue1, x => helpQueue.MoveToQueue(x), WorkMode.Intensity);
                device2 = new Device("Device2", Settings.DEVICE_2_TIME, helpQueue, x => queue2.MoveToQueue(x), WorkMode.Time);
                device3 = new Device("Device3", Settings.DEVICE_3_TIME, queue2, x =>
                {
                    EndWork(x);
                }, WorkMode.Time);

                helpDevice = new Device("HelpDevice", 0, helpQueue, x => queue2.MoveToQueue(x), WorkMode.Time);
            }

            Init();

            var thread1 = new Thread(() => device1.ExecuteAsync(stoppingToken));
            thread1.Name = "Device1";

            var thread2 = new Thread(() => device2.ExecuteAsync(stoppingToken));
            thread2.Name = "Device2";

            var thread3 = new Thread(() => device3.ExecuteAsync(stoppingToken));
            thread3.Name = "Device3";

            var thread4 = new Thread(() => helpDevice.ExecuteAsync(stoppingToken));
            thread4.Name = "HelpDevice";

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            int delayTime = Settings.Delay;
            int count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                Customer customer = new Customer($"Customer {count}");
                customerCount++;

                customer.WriteToFile("Has been created");
                customer.CreateMessage("Has been created");

                if (queue1.Count >= Settings.QUEUE_1_LIMIT)
                {
                    customer.WriteToFile("Has been rejected");
                    customer.CreateMessage("Has been rejected");
                    customer.IsRejected = true;

                    EndWork(customer);
                }
                else
                {
                    queue1.MoveToQueue(customer);
                }

                Thread.Sleep(delayTime);
                count++;
            }
        }

        private void EndWork(Customer customer)
        {
            servedCustomers.Add(customer);
            customer.WriteToFile("End Work");
            customer.CreateMessage("End Work");

            lock (loker)
            {
                customer.WriteFinalMessage();
            }
        }

        public void GetInformation()
        {
            double rejectionProcent = servedCustomers.Where(x => x.IsRejected).Count() / customerCount;
            queue1Length = queue1.AverageLentgth;
            queue2Length = queue2.AverageLentgth;

            var queue1AvrTimeAsReal = servedCustomers.Where(x => !x.IsRejected).Select(x => x.Timings[queue1Name]).Average();
            var queue2AvrTimeAsReal = servedCustomers.Where(x => !x.IsRejected).Select(x => x.Timings[queue2Name]).Average();

            var queue1AvrTime = queue1.LengthOfQueuePerTime / queue1.AllServed;
            var queue2AvrTime = queue2.LengthOfQueuePerTime / queue2.AllServed;
        }
    }
}
