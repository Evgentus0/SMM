﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4
{
    public delegate void NextAction(Customer customer);

    public class Device : ControllerBase
    {

        CustomQueue queue;

        double value;

        WorkMode mode;

        public NextAction NextAction;

        public string Name { get; set; }

        public Device(string name, double value, CustomQueue queue, NextAction action, WorkMode mode) 
        {
            Name = name;
            NextAction = action;
            this.value = value;
            resetEvent = new AutoResetEvent(true);
            this.queue = queue;
            this.mode = mode;
        }

        public int Process() 
        {
            int time = 0;
            if(mode == WorkMode.Intensity)
            {
                time = (int)(1.0 / value * Settings.TimeMeasure);
            }
            else if(mode == WorkMode.Time)
            {
                time = (int)value;
            }

            return time;
        }

        public void ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var customer = queue.FirstCustomerOfQueue();

                customer.CreateMessage($"Processed by {Name}");
                customer.WriteToFile($"Processed by, {Name}");

                int time = Process();
                Thread.Sleep(time);

                NextAction(customer);
            }
        }

    }
}
