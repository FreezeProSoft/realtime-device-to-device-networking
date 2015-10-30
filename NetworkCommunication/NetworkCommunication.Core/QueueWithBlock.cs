using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{

    public class QueueWithBlock<T> : Queue<T> where T: class
    {
        public new void Enqueue(T item)
        {
            lock (lockObject)
            {
                if (Count == 0)
                {
                    base.Enqueue(item);
                
                    Monitor.PulseAll(lockObject);
                }
                else
                {
                    base.Enqueue(item);
                }
            }
        }

        public new T Dequeue()
        {
            lock (lockObject)
            {
                if (Count == 0)
                {
                    Monitor.Wait(lockObject);
                }

                if (Count == 0)
                {
                    return null;
                }

                return base.Dequeue();
            }
        }

        public void Release()
        {
            lock (lockObject)
            {
                Monitor.PulseAll(lockObject); 
            }
        }

        private object lockObject = new object();
    }
}
