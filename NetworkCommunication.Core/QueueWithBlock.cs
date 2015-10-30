using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    /// <summary>
    /// The queue with the ability to block the thread.
    /// </summary>
    public class QueueWithBlock<T> : Queue<T> where T: class
    {
        /// <summary>
        /// Enqueue the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
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

        /// <summary>
        /// Dequeue the specified item.
        /// </summary>
        /// <returns>The specified item.</returns>
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

        /// <summary>
        /// Unblock current thread.
        /// </summary>
        public void Release()
        {
            lock (lockObject)
            {
                Monitor.PulseAll(lockObject); 
            }
        }

        /// <summary>
        /// The lock object.
        /// </summary>
        private object lockObject = new object();
    }
}
