using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumerOne
{
    //this code produce up to 5 items and consume while posible
    class Program
    {
        static Queue<int> _itemsHolder = new Queue<int>();
        static int item = 0;
        static object _lock = new object();
        static Random _random = new Random();

        static void Main()
        {
            Program pg = new Program();

            Thread producer = new Thread(pg.Producer);
            Thread consumer = new Thread(pg.Consumer);
            
            producer.Start();
            consumer.Start();
        }
        public void Producer()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    if (_itemsHolder.Count < 5)
                    {
                        _itemsHolder.Enqueue(item);
                        Console.WriteLine(_itemsHolder.Count + " producer");
                        Monitor.PulseAll(_lock);
                    }
                    else
                    {
                        Monitor.Wait(_lock);
                        Console.WriteLine("Producer is wating");
                    }

                    //Thread.Sleep(_random.Next(200, 1000));
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
            }
        }

        public void Consumer()
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    {
                        if (_itemsHolder.Count > 0)
                        {
                            _itemsHolder.Dequeue();
                            Console.WriteLine(_itemsHolder.Count + " consumer");
                            Monitor.PulseAll(_lock);
                        }
                        else
                        {
                            Monitor.Wait(_lock);
                            Console.WriteLine("Consumer is wating");
                        }
                    }

                    Thread.Sleep(_random.Next(200, 1000));
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
            }
        }
    }
}